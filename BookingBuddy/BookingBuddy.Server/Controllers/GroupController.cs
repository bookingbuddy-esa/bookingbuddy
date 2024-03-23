using System.Net.WebSockets;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property = BookingBuddy.Server.Models.Property;

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
        // private static readonly WebSocketWrapper<Group> WebSocketWrapper = new();

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

            var addedProperty = model.propertyId != null
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
                group.AddedPropertyIds.ForEach(userAddedPropertyId =>
                {
                    var userAddedProperty = _context.UserAddedProperty
                        .Include(uap => uap.Property)
                        .Include(uap => uap.ApplicationUser)
                        .FirstOrDefault(uap =>
                            uap.UserAddedPropertyId == userAddedPropertyId);
                    if (userAddedProperty == null) return;

                    addedProperties.Add(userAddedProperty);
                });

                group.AddedProperties = addedProperties;

                List<ReturnUser> users = [];

                group.MembersId.ForEach(memberId =>
                {
                    var member = _context.Users.FirstOrDefault(u => u.Id == memberId);
                    if (member != null)
                    {
                        users.Add(new ReturnUser
                        {
                            Id = member.Id,
                            Name = member.Name
                        });
                    }
                });

                group.Members = users;
                var owner = _context.Users.FirstOrDefault(u => u.Id == group.GroupOwnerId);

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
                    ChosenProperty = group.ChosenProperty,
                    ChatId = group.ChatId,
                    GroupAction = group.GroupAction.AsString()
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

        [HttpPut("setGroupAction")]
        [Authorize]
        public async Task<IActionResult> SetGroupAction(string groupId, string groupAction)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (groupAction != "none" && groupAction != "voting" && groupAction != "booking" && groupAction != "paying")
            {
                return BadRequest("Ação inválida.");
            }

            switch (groupAction.ToLower())
            {
                case "none":
                    group.GroupAction = GroupAction.None;
                    break;
                case "voting":
                    group.GroupAction = GroupAction.Voting;
                    break;
                case "booking":
                    group.GroupAction = GroupAction.Booking;
                    break;
                case "paying":
                    group.GroupAction = GroupAction.Paying;
                    break;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Ação do grupo atualizada com sucesso.");
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

            group.AddedPropertyIds.Add(propertyId);
            try
            {
                await _context.SaveChangesAsync();

                return Ok("Propriedade adicionada com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Remove um propriedade a um grupo existente.
        /// </summary>
        /// <param name="groupId">O ID do grupo ao qual a propriedade será adicionada.</param>
        /// <param name="propertyId">O ID da propriedade a ser adicionada.</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [HttpPut("removeProperty")]
        [Authorize]
        public async Task<IActionResult> RemoveProperty(string groupId, string propertyId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }


            if (!group.AddedPropertyIds.Contains(propertyId))
            {
                return BadRequest("A propriedade não existe no grupo!");
            }

            group.AddedPropertyIds.Remove(propertyId);
            try
            {
                await _context.SaveChangesAsync();

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
        /// <param name="userId">O ID do utilizador a ser adicionado como membro.</param>
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
                return CreatedAtAction("GetGroup", new { groupId = group.GroupId }, group);
                //return Ok("Membro adicionado ao grupo com sucesso.");
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
            if (user == null || group.GroupOwnerId != user.Id)
            {
                return Unauthorized();
            }

            var groupMessages = _context.GroupMessage.Where(m => m.GroupId == groupId);

            // Remover todas as mensagens associadas
            _context.GroupMessage.RemoveRange(groupMessages);

            _context.Groups.Remove(group);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Grupo eliminado com sucesso.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            // await WebSocketWrapper.HandleAsync(group, webSocket, async groupReceived => {
            //     Console.WriteLine("Group received: " + JsonSerializer.Serialize(groupReceived));
            //     await WebSocketWrapper.NotifyAllAsync(groupReceived);
            // });
        }
    }

    /// <summary>
    /// Modelo que representa a criação de um grupo.
    /// </summary>
    /// <param name="name">O nome do grupo.</param>
    /// <param name="propertyId">O ID da propriedade associada ao grupo (opcional).</param>
    /// <param name="memberEmails">Uma lista de endereços de e-mail dos membros a serem adicionados ao grupo (opcional).</param>
    public record GroupInputModel(string name, string? propertyId, List<string>? memberEmails);

    public record NewGroupMessage(string message);

    public record ReturnGroup
    {
        public string GroupId { get; set; }
        public string? GroupBookingId { get; set; }
        public ReturnUser GroupOwner { get; set; }
        public string Name { get; set; }
        public List<ReturnUser> Members { get; set; }
        public List<ReturnProperty> Properties { get; set; }
        public string? ChosenProperty { get; set; }
        public string? ChatId { get; set; }
        public string GroupAction { get; set; }
    }

    public record ReturnProperty
    {
        public string PropertyId { get; set; }
        public string Name { get; set; }
        public decimal PricePerNight { get; set; }
        public List<string> ImagesUrl { get; set; }
        public string Location { get; set; }

        public ReturnUser AddedBy { get; set; }
    }
}