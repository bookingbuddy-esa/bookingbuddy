using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Controlador para os grupos.
    /// </summary>
    [Route("api/groups")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private static readonly WebSocketWrapper WebSocketWrapper = new();
        private static readonly Dictionary<string, List<WebSocket>> GroupSockets = new();

        /// <summary>
        /// Construtor da classe PropertyController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public GroupController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Método para criar um novo grupo.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna o resultado da criação do grupo.</returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup(GroupInputModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var addedProperty = !string.IsNullOrEmpty(model.propertyId)
                ? new UserAddedProperty
                {
                    UserAddedPropertyId = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    PropertyId = model.propertyId
                }
                : null;

            var group = new Group
            {
                GroupId = Guid.NewGuid().ToString()[..16],
                GroupOwnerId = user.Id,
                Name = model.name,
                MembersId = [user.Id],
                AddedPropertyIds = addedProperty != null ? [addedProperty.UserAddedPropertyId] : [],
                GroupAction = GroupAction.None
            };

            if (model.memberEmails != null)
            {
                group.Members = new List<ReturnUser>
                {
                    new()
                    {
                        Id = user.Id,
                        Name = user.Name
                    }
                };
                foreach (var email in model.memberEmails)
                {
                    var member = await _userManager.FindByEmailAsync(email);
                    if (member == null) continue;
                    group.MembersId.Add(member.Id);
                    group.Members.Add(new ReturnUser
                    {
                        Id = member.Id,
                        Name = member.Name
                    });
                    var groupReservationLink =
                        $"{_configuration.GetSection("Front-End-Url").Value!}/groups?groupId={group.GroupId}";
                    await EmailSender.SendTemplateEmail(_configuration.GetSection("MailAPIKey").Value!,
                        "d-d42dbf24249347e98a2e869043c21b26", email, member.Name,
                        new { groupReservationLink });
                }
            }

            try
            {
                var chat = _context.Chat.Add(new Chat
                {
                    ChatId = Guid.NewGuid().ToString(),
                    Name = group.Name
                }).Entity;
                group.ChatId = chat.ChatId;
                _context.Groups.Add(group);
                if (addedProperty != null)
                {
                    _context.UserAddedProperty.Add(addedProperty);
                }

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGroup),
                    new { groupId = group.GroupId },
                    (await GetGroup(group.GroupId) as OkObjectResult)?.Value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Método que retorna um grupo que contenha o id passado por parâmetro.
        /// Caso não exista retorna que não foi encontrado.
        /// </summary>
        /// <param name="groupId">Identificador do grupo</param>
        /// <returns>O grupo, caso exista ou não encontrado, caso contrário</returns>
        [HttpGet("{groupId}")]
        [Authorize]
        public async Task<IActionResult> GetGroup(string groupId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var group = await _context.Groups.FindAsync(groupId);
                if (group == null)
                {
                    return NotFound(
                        "Não foi encontrado nenhum grupo com este ID. Certifique-se que o URL está correto.");
                }

                List<UserAddedProperty> addedProperties = [];
                foreach (var userAddedPropertyId in group.AddedPropertyIds)
                {
                    var userAddedProperty = await _context.UserAddedProperty
                        .Include(uap => uap.Property)
                        .Include(uap => uap.ApplicationUser)
                        .FirstOrDefaultAsync(uap =>
                            uap.UserAddedPropertyId == userAddedPropertyId);
                    if (userAddedProperty == null) continue;

                    addedProperties.Add(userAddedProperty);
                }

                group.AddedProperties = addedProperties;

                List<ReturnUser> users = [];

                foreach (var memberId in group.MembersId)
                {
                    var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == memberId);
                    if (member != null)
                    {
                        users.Add(new ReturnUser
                        {
                            Id = member.Id,
                            Name = member.Name
                        });
                    }
                }

                group.Members = users;
                var owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == group.GroupOwnerId);

                var userVote = await _context.UserVote.Where(uv => group.UserVoteIds.Contains(uv.UserVoteId))
                    .ToListAsync();

                return Ok(new ReturnGroup
                {
                    GroupId = group.GroupId,
                    GroupBookingId = group.GroupBookingId,
                    GroupOwner = new ReturnUser
                    {
                        Id = owner?.Id ?? "Unknown",
                        Name = owner?.Name ?? "Unknown"
                    },
                    Name = group.Name,
                    Members = group.Members,
                    Properties = addedProperties.Select(
                        uap => new ReturnProperty()
                        {
                            PropertyId = uap.PropertyId,
                            Name = uap.Property?.Name ?? "Unknown",
                            PricePerNight = uap.Property?.PricePerNight ?? 0,
                            ImagesUrl = uap.Property?.ImagesUrl ?? [],
                            Location = uap.Property?.Location ?? "Unknown",
                            AddedBy = new ReturnUser
                            {
                                Id = uap.ApplicationUser?.Id ?? "Unknown",
                                Name = uap.ApplicationUser?.Name ?? "Unknown"
                            }
                        }).ToList(),
                    Votes = userVote.Select(uv => new ReturnVote
                    {
                        UserId = uv.ApplicationUserId,
                        PropertyId = uv.PropertyId
                    }).ToList(),
                    ChosenProperty = group.ChosenProperty,
                    ChatId = group.ChatId,
                    GroupAction = group.GroupAction.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Método que obtém os grupos de um utilizador.
        /// </summary>
        /// <param name="userId">Identificador do utilizador</param>
        /// <returns>Lista com os grupos do utilizador, caso exista ou não encontrada, caso contrário</returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetGroupsByUserId(string userId)
        {
            try
            {
                var groups = await _context.Groups.Where(g => g.MembersId.Contains(userId)).ToListAsync();
                if (groups.Count == 0)
                {
                    return Ok(new List<ReturnGroup>());
                }

                List<ReturnGroup> groupsList = [];
                foreach (var group in groups)
                {
                    var groupResult = await GetGroup(group.GroupId);
                    if (groupResult is OkObjectResult { Value: ReturnGroup groupResultOk })
                    {
                        groupsList.Add(groupResultOk);
                    }
                }

                return Ok(groupsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Método que atualiza a ação de um grupo.
        /// </summary>
        /// <param name="model">Modelo com o identificador do grupo e a ação a ser atualizada</param>
        /// <returns>Retorna o resultado da atualização da ação do grupo.</returns>
        [HttpPut("updateGroupAction")]
        [Authorize]
        public async Task<IActionResult> UpdateGroupAction([FromBody] UpdateGroupActionModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var group = await _context.Groups.FindAsync(model.GroupId);

            if (group == null)
            {
                return NotFound();
            }

            if (group.GroupOwnerId != user.Id)
            {
                return Forbid();
            }

            var groupAction = model.GroupAction;
            GroupAction? parsedGroupAction;

            try
            {
                parsedGroupAction = Enum.Parse<GroupAction>(groupAction, true);
            }
            catch
            {
                return BadRequest("A ação do grupo não é válida.");
            }

            if (parsedGroupAction == group.GroupAction)
            {
                NoContent();
            }

            group.GroupAction = parsedGroupAction.Value;
            try
            {
                await _context.SaveChangesAsync();
                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "GroupActionUpdated",
                        Content = JsonSerializer.Serialize(new
                        {
                            groupId = group.GroupId,
                            groupAction = group.GroupAction.ToString()
                        })
                    });
                }

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut("setChoosenProperty")]
        [Authorize]
        public async Task<IActionResult> SetChoosenProperty(string groupId, string propertyId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (group.ChosenProperty != null)
            {
                return BadRequest("Já existe uma propriedade escolhida para este grupo.");
            }

            var addedProperty =
                await _context.UserAddedProperty.FirstOrDefaultAsync(uap =>
                    group.AddedPropertyIds.Contains(uap.UserAddedPropertyId));

            if (!addedProperty?.PropertyId.Contains(propertyId) ?? false)
            {
                return BadRequest("A propriedade não existe no grupo.");
            }

            group.ChosenProperty = propertyId;
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Propriedade escolhida com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Adiciona uma propriedade a um grupo existente.
        /// </summary>
        /// <param name="groupId">O ID do grupo ao qual a propriedade será adicionada.</param>
        /// <param name="propertyId">O ID da propriedade a ser adicionada.</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [HttpPut("addProperty")]
        [Authorize]
        public async Task<IActionResult> AddProperty(string groupId, string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            var addedProperties =
                await _context.UserAddedProperty.Where(uap =>
                    group.AddedPropertyIds.Contains(uap.UserAddedPropertyId)).ToListAsync();
            if (addedProperties.Count >= 6)
            {
                return BadRequest("O Grupo ja tem 6 propriedades na votação!");
            }

            if (addedProperties.Any(uap => uap.PropertyId == propertyId))
            {
                return BadRequest("A propriedade ja existe no grupo!");
            }

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound();
            }

            var addedProperty = new UserAddedProperty
            {
                UserAddedPropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                PropertyId = propertyId
            };
            group.AddedPropertyIds.Add(addedProperty.UserAddedPropertyId);
            try
            {
                _context.UserAddedProperty.Add(addedProperty);
                await _context.SaveChangesAsync();

                var returnProperty = new ReturnProperty
                {
                    PropertyId = addedProperty.PropertyId,
                    Name = property.Name,
                    PricePerNight = property.PricePerNight,
                    ImagesUrl = property.ImagesUrl,
                    Location = property.Location,
                    AddedBy = new ReturnUser
                    {
                        Id = user.Id,
                        Name = user.Name
                    }
                };

                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "PropertyAdded",
                        Content = JsonSerializer.Serialize(new
                        {
                            groupId,
                            property = returnProperty
                        })
                    });
                }

                return Ok(returnProperty);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Remove uma propriedade a um grupo existente.
        /// </summary>
        /// <param name="model">Modelo com o identificador da propriedade e do grupo</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [HttpPut("removeProperty")]
        [Authorize]
        public async Task<IActionResult> RemoveProperty([FromBody] PropertyRemoveModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var group = await _context.Groups.FindAsync(model.GroupId);

            if (group == null)
            {
                return NotFound("O grupo não existe.");
            }

            var addedProperties = _context.UserAddedProperty.Where(uap =>
                group.AddedPropertyIds.Contains(uap.UserAddedPropertyId)).ToList();

            var addedProperty = addedProperties.FirstOrDefault(uap => uap.PropertyId == model.PropertyId);

            if (addedProperty == null)
            {
                return NotFound("A propriedade não existe no grupo.");
            }

            if (addedProperty.ApplicationUserId != user.Id && group.GroupOwnerId != user.Id)
            {
                return Forbid();
            }

            group.AddedPropertyIds.Remove(addedProperty.UserAddedPropertyId);

            try
            {
                await _context.SaveChangesAsync();
                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "PropertyRemoved",
                        Content = JsonSerializer.Serialize(new
                        {
                            groupId = group.GroupId,
                            propertyId = addedProperty.PropertyId
                        })
                    });
                }

                return Ok("Propriedade removida com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adiciona um utilizador como membro de um grupo existente.
        /// </summary>
        /// <param name="groupId">O ID do grupo ao qual o utilizador será adicionado como membro.</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [Authorize]
        [HttpPut("addMember")]
        public async Task<IActionResult> AddMember(string groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            group.MembersId.Add(user.Id);
            try
            {
                await _context.SaveChangesAsync();
                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "MemberAdded",
                        Content = JsonSerializer.Serialize(new
                        {
                            groupId = group.GroupId,
                            member = new ReturnUser
                            {
                                Id = user.Id,
                                Name = user.Name
                            }
                        })
                    });
                }

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Remove um utilizador do grupo de reserva.
        /// </summary>
        /// <param name="groupId">O ID do grupo ao qual o utilizador será removido.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("leaveGroup")]
        public async Task<IActionResult> LeaveGroup(string groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            group.MembersId.Remove(user.Id);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetGroup", new { groupId = group.GroupId }, group);
                //return Ok("Membro adicionado ao grupo com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Remove um grupo de reserva.
        /// </summary>
        /// <param name="groupId">O ID do grupo a ser removido.</param>
        /// <returns>Rertorna o resultado da remoção do grupo.</returns>
        [HttpDelete("delete/{groupId}")]
        [Authorize]
        public async Task<IActionResult> DeleteGroup(string groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (group.GroupOwnerId != user.Id)
            {
                return Forbid();
            }

            _context.Groups.Remove(group);
            try
            {
                await _context.SaveChangesAsync();
                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "GroupDeleted",
                        Content = JsonSerializer.Serialize(new
                        {
                            groupId = group.GroupId
                        })
                    });
                }

                return Ok("Grupo eliminado com sucesso.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addVote")]
        [Authorize]
        public async Task<IActionResult> AddVote([FromBody] AddVoteModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var group = await _context.Groups.FindAsync(model.GroupId);
            if (group == null)
            {
                return NotFound();
            }

            if (!group.MembersId.Contains(user.Id))
            {
                return Forbid();
            }

            if (group.GroupAction != GroupAction.Voting)
            {
                return BadRequest("O grupo não está em votação.");
            }

            var property = await _context.Property.FindAsync(model.PropertyId);
            if (property == null)
            {
                return NotFound();
            }

            List<UserVote> votes = [];
            foreach (var userVoteId in group.UserVoteIds)
            {
                var uv = await _context.UserVote.FindAsync(userVoteId);
                if (uv == null) continue;
                votes.Add(uv);
            }

            if (votes.Any(uv => uv.ApplicationUserId == user.Id))
            {
                return BadRequest("O utilizador já votou.");
            }

            var userVote = new UserVote
            {
                UserVoteId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                PropertyId = property.PropertyId
            };
            group.UserVoteIds.Add(userVote.UserVoteId);

            try
            {
                _context.UserVote.Add(userVote);
                await _context.SaveChangesAsync();
                foreach (var socket in GroupSockets.Where(gs => gs.Key == group.GroupId).SelectMany(gs => gs.Value))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                        {
                            Code = "VoteAdded",
                            Content = JsonSerializer.Serialize(new
                            {
                                groupId = group.GroupId,
                                vote = new ReturnVote
                                {
                                    UserId = userVote.ApplicationUserId,
                                    PropertyId = userVote.PropertyId
                                }
                            })
                        }
                    );
                }

                return Ok("Voto adicionado com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Manipula a comunicação WebSocket para um grupo específico.
        /// </summary>
        /// <param name="groupId">O ID do grupo para o qual a comunicação WebSocket será manipulada.</param>
        /// <param name="webSocket">O objeto WebSocket que será manipulado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        [NonAction]
        public async Task HandleWebSocketAsync(string groupId, WebSocket webSocket)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null) return;

            WebSocketWrapper.AddOnConnectListener(webSocket, (_, _) =>
            {
                if (GroupSockets.TryGetValue(groupId, out var value))
                {
                    value.Add(webSocket);
                }
                else
                {
                    GroupSockets.Add(groupId, [webSocket]);
                }

                return Task.CompletedTask;
            });
            WebSocketWrapper.AddOnCloseListener(webSocket, (_, _) =>
            {
                if (GroupSockets.TryGetValue(groupId, out var value))
                {
                    value.Remove(webSocket);
                }

                return Task.CompletedTask;
            });

            await WebSocketWrapper.HandleAsync(webSocket);
        }
    }

    /// <summary>
    /// Modelo que representa a atualização de uma ação de grupo.
    /// </summary>
    public class UpdateGroupActionModel
    {
        /// <summary>
        /// O identificador do grupo.
        /// </summary>
        public required string GroupId { get; set; }

        /// <summary>
        /// A ação do grupo.
        /// </summary>
        public required string GroupAction { get; set; }
    }

    /// <summary>
    /// Modelo que representa a adição de um voto.
    /// </summary>
    public record AddVoteModel
    {
        /// <summary>
        /// O identificador do grupo.
        /// </summary>
        public required string GroupId { get; set; }

        /// <summary>
        /// O identificador da propriedade.
        /// </summary>
        public required string PropertyId { get; set; }
    }

    /// <summary>
    /// Modelo que representa a criação de um grupo.
    /// </summary>
    /// <param name="name">O nome do grupo.</param>
    /// <param name="propertyId">O ID da propriedade associada ao grupo (opcional).</param>
    /// <param name="memberEmails">Uma lista de endereços de e-mail dos membros a serem adicionados ao grupo (opcional).</param>
    public record GroupInputModel(string name, string? propertyId, List<string>? memberEmails);

    /// <summary>
    /// Modelo que representa a remoção de uma propriedade de um grupo.
    /// </summary>
    public record PropertyRemoveModel
    {
        /// <summary>
        /// O identificador do grupo.
        /// </summary>
        public required string GroupId { get; set; }

        /// <summary>
        /// O identificador da propriedade.
        /// </summary>
        public required string PropertyId { get; set; }
    }

    /// <summary>
    /// Modelo que representa um grupo a ser retornado.
    /// </summary>
    public record ReturnGroup
    {
        /// <summary>
        /// O identificador do grupo.
        /// </summary>
        [JsonPropertyName("groupId")]
        public required string GroupId { get; set; }

        /// <summary>
        /// O identificador da reserva do grupo.
        /// </summary>
        [JsonPropertyName("groupBookingId")]
        public string? GroupBookingId { get; set; }

        /// <summary>
        /// O utilizador que é dono do grupo.
        /// </summary>
        [JsonPropertyName("groupOwner")]
        public required ReturnUser GroupOwner { get; set; }

        /// <summary>
        /// O nome do grupo.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// A lista de membros do grupo.
        /// </summary>
        [JsonPropertyName("members")]
        public List<ReturnUser> Members { get; set; } = [];

        /// <summary>
        /// A lista de propriedades adicionadas ao grupo.
        /// </summary>
        [JsonPropertyName("properties")]
        public List<ReturnProperty> Properties { get; set; } = [];

        /// <summary>
        /// A lista de votos dos utilizadores.
        /// </summary>
        [JsonPropertyName("votes")]
        public List<ReturnVote> Votes { get; set; } = [];

        /// <summary>
        /// A propriedade escolhida para o grupo.
        /// </summary>
        [JsonPropertyName("chosenProperty")]
        public string? ChosenProperty { get; set; }

        /// <summary>
        /// O identificador do chat associado ao grupo.
        /// </summary>
        [JsonPropertyName("chatId")]
        public string? ChatId { get; set; }

        /// <summary>
        /// A ação do grupo.
        /// </summary>
        [JsonPropertyName("groupAction")]
        public required string GroupAction { get; set; }
    }

    /// <summary>
    /// Modelo que representa uma propriedade a ser retornada.
    /// </summary>
    public record ReturnProperty
    {
        /// <summary>
        /// O identificador da propriedade.
        /// </summary>
        [JsonPropertyName("propertyId")]
        public required string PropertyId { get; set; }

        /// <summary>
        /// O nome da propriedade.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// O preço por noite da propriedade.
        /// </summary>
        [JsonPropertyName("pricePerNight")]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// A lista de URLs das imagens da propriedade.
        /// </summary>
        [JsonPropertyName("imagesUrl")]
        public List<string> ImagesUrl { get; set; } = [];

        /// <summary>
        /// A localização da propriedade.
        /// </summary>
        [JsonPropertyName("location")]
        public required string Location { get; set; }

        /// <summary>
        /// O utilizador que adicionou a propriedade.
        /// </summary>
        [JsonPropertyName("addedBy")]
        public required ReturnUser AddedBy { get; set; }
    }

    /// <summary>
    /// Modelo que representa um voto de um utilizador.
    /// </summary>
    public record ReturnVote
    {
        /// <summary>
        /// Propriedade que diz respeito ao identificador do voto de um utilizador numa propriedade.
        /// </summary>
        [JsonPropertyName("userId")]
        public required string UserId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador da propriedade votada.
        /// </summary>
        [JsonPropertyName("propertyId")]
        public required string PropertyId { get; set; }
    }
}