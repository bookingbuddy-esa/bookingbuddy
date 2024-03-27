using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        /// Construtor da classe GroupController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public GroupController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Cria um novo grupo com o utilizador autenticado como proprietário.
        /// </summary>
        /// <param name="model">Os dados necessários para criar o grupo.</param>
        /// <returns>
        /// Um código de estado 201 (Criado) com os detalhes do grupo recém-criado, se o grupo for criado com sucesso.
        /// Um código de estado 401 (Não Autorizado) se o utilizador não estiver autenticado.
        /// Um código de estado 400 (Pedido Inválido) se ocorrer um erro durante a criação do grupo.
        /// </returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup(GroupInputModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var group = new Models.Group
            {
                GroupId = Guid.NewGuid().ToString().Substring(0, 16),
                GroupOwnerId = user.Id,
                Name = model.name,
                MembersId = [user.Id],
                PropertiesId = [],
                MessagesId = [],
                VotesId = [],
                GroupAction = GroupAction.None
            };

            if (model.propertyId != null)
            {
                group.PropertiesId.Add(model.propertyId);
            }

            if(model.memberEmails != null)
            {
                foreach (var email in model.memberEmails)
                {
                    var member = await _userManager.FindByEmailAsync(email);
                    if (member != null)
                    {
                        var groupReservationLink = $"{_configuration.GetSection("Front-End-Url").Value!}/groups?groupId={group.GroupId}";
                        await EmailSender.SendTemplateEmail(_configuration.GetSection("MailAPIKey").Value!,
                            "d-d42dbf24249347e98a2e869043c21b26", email, member.Name,
                            new { groupReservationLink });
                    }
                }
            }

            try
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                //return Ok("Grupo criado com sucesso.");
                return CreatedAtAction(nameof(GetGroup), new { groupId = group.GroupId }, group);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Obtém os detalhes de um grupo com base no seu identificador único.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo.</param>
        /// <returns>
        /// Um objeto ActionResult contendo os detalhes do grupo, se encontrado.
        /// Um código de estado 404 (Não Encontrado) se nenhum grupo for encontrado com o ID fornecido.
        /// </returns>
        [HttpGet("{groupId}")]
        [Authorize]
        public async Task<ActionResult<Models.Group>> GetGroup(string groupId)
        {
            try
            {
                var group = await _context.Groups.FindAsync(groupId);
                if (group == null)
                {
                    return NotFound("Não foi encontrado nenhum grupo com este ID. Certifique-se que o URL está correto.");
                }

                List<Property> properties = [];
                group.PropertiesId?.ForEach(propertyId => {
                    var property = _context.Property.Where(p => p.PropertyId == propertyId).FirstOrDefault();
                    if (property != null)
                    {
                        properties.Add(new Property
                        {
                            PropertyId = property.PropertyId,
                            Name = property.Name,
                            PricePerNight = property.PricePerNight,
                            ImagesUrl = property.ImagesUrl,
                            Location = property.Location
                        });
                    }
                });

                group.Properties = properties;

                List<ReturnUser> users = [];

                group.MembersId?.ForEach(memberId => {
                    var user = _context.Users.FirstOrDefault(u => u.Id == memberId);
                    if(user != null){
                        users.Add(new ReturnUser(){
                            Id = user.Id,
                            Name = user.Name
                        });
                    }
                });

                group.Members = users;

                List<GroupMessage> messages = [];
                group.MessagesId?.ForEach(messageId => {
                    var message = _context.GroupMessage.FirstOrDefault(m => m.MessageId == messageId);
                    if (message != null)
                    {
                        messages.Add(message);
                    }
                });

                group.Messages = messages;

                List<GroupVote> votes = [];
                group.VotesId?.ForEach(votesId => {
                    var vote = _context.GroupVote.FirstOrDefault(v => v.VoteId == votesId);
                    if (vote != null)
                    {
                        votes.Add(vote);
                    }
                });

                group.Votes = votes;
                return group;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Obtém todos os grupos aos quais um utilizador está associado com base no seu identificador único.
        /// </summary>
        /// <param name="userId">O identificador único do utilizador.</param>
        /// <returns>
        /// Um objeto IActionResult contendo a lista de grupos associados ao utilizador, se encontrados.
        /// Um código de estado 200 (OK) com uma lista vazia se o utilizador não estiver associado a nenhum grupo.
        /// </returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetGroupsByUserId(string userId)
        {
            try {
                var groups = await _context.Groups.Where(g => g.MembersId.Contains(userId)).ToListAsync();
                if (groups == null || groups.Count == 0)
                {
                    return Ok(new List<Group>());
                }

                List<Group> groupsList = [];
                foreach (var group in groups)
                {
                    var groupResult = await GetGroup(group.GroupId);
                    if (groupResult != null)
                    {
                        groupsList.Add((Group)groupResult.Value);
                    }
                }

                return Ok(groupsList);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Define a ação de grupo para um grupo específico com base no identificador único do grupo e na ação fornecida.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo.</param>
        /// <param name="groupAction">A ação a ser definida para o grupo. Pode ser "none", "voting", "booking" ou "paying".</param>
        /// <returns>
        /// Um objeto IActionResult contendo uma mensagem de sucesso se a ação do grupo for atualizada com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 400 (Pedido inválido) se a ação fornecida for inválida.
        /// </returns>
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

            switch(groupAction.ToLower())
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

        /// <summary>
        /// Define a propriedade escolhida para um grupo específico com base no identificador único do grupo e no identificador único da propriedade.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo.</param>
        /// <param name="propertyId">O identificador único da propriedade a ser definida como escolhida para o grupo.</param>
        /// <returns>
        /// Um objeto IActionResult contendo uma mensagem de sucesso se a propriedade escolhida for definida com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 400 (Pedido inválido) se já houver uma propriedade escolhida para o grupo ou se a propriedade não existir no grupo.
        /// </returns>
        [HttpPut("setChoosenProperty")]
        [Authorize]
        public async Task<IActionResult> SetChoosenProperty(string groupId, string propertyId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (group.ChoosenProperty != null)
            {
                return BadRequest("Já existe uma propriedade escolhida para este grupo.");
            }

            if (!group.PropertiesId.Contains(propertyId))
            {
                return BadRequest("A propriedade não existe no grupo.");
            }

            group.ChoosenProperty = propertyId;
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
        /// Adiciona uma propriedade ao grupo especificado com base nos identificadores únicos do grupo e da propriedade.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo.</param>
        /// <param name="propertyId">O identificador único da propriedade a ser adicionada ao grupo.</param>
        /// <returns>
        /// Um objeto IActionResult contendo uma mensagem de sucesso se a propriedade for adicionada com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 400 (Pedido inválido) se o grupo já tiver 6 propriedades na votação ou se a propriedade já existir no grupo.
        /// </returns>
        [HttpPut("addProperty")]
        [Authorize]
        public async Task<IActionResult> AddProperty(string groupId, string propertyId)
        {

            var group = await _context.Groups.FindAsync(groupId);

            if(group == null)
            {
                return NotFound();
            }

            if (group.PropertiesId.Count >= 6)
            {
                return BadRequest("O Grupo ja tem 6 propriedades na votação!");
            }

            if (group.PropertiesId.Contains(propertyId))
            {
                return BadRequest("A propriedade ja existe no grupo!");
            }

            group.PropertiesId.Add(propertyId);
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
        /// Remove uma propriedade do grupo especificado com base nos identificadores únicos do grupo e da propriedade.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo.</param>
        /// <param name="propertyId">O identificador único da propriedade a ser removida do grupo.</param>
        /// <returns>
        /// Um objeto IActionResult contendo uma mensagem de sucesso se a propriedade for removida com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 400 (Pedido inválido) se a propriedade não existir no grupo.
        /// </returns>
        [HttpPut("removeProperty")]
        [Authorize]
        public async Task<IActionResult> RemoveProperty(string groupId, string propertyId)
        {

            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }


            if (!group.PropertiesId.Contains(propertyId))
            {
                return BadRequest("A propriedade não existe no grupo!");
            }

            group.PropertiesId.Remove(propertyId);
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
        /// Adiciona o utilizador autenticado como membro ao grupo especificado com base no identificador único do grupo.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo ao qual o utilizador será adicionado como membro.</param>
        /// <returns>
        /// Um objeto IActionResult contendo o grupo atualizado se o utilizador for adicionado com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 401 (Não autorizado) se o utilizador não estiver autenticado.
        /// Um código de estado 400 (Pedido inválido) se ocorrer um erro durante o processo de adição do membro.
        /// </returns>
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
        /// Remove o utilizador autenticado como membro do grupo especificado com base no identificador único do grupo.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo do qual o utilizador será removido como membro.</param>
        /// <returns>
        /// Um objeto IActionResult contendo o grupo atualizado após a remoção do utilizador.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 401 (Não autorizado) se o utilizador não estiver autenticado.
        /// Um código de estado 400 (Pedido inválido) se ocorrer um erro durante o processo de remoção do membro.
        /// </returns>
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
        /// Elimina o grupo especificado com base no identificador único do grupo.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo a ser eliminado.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o grupo for eliminado com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 401 (Não autorizado) se o utilizador autenticado não for o proprietário do grupo.
        /// Um código de estado 400 (Pedido inválido) se ocorrer um erro durante o processo de eliminação do grupo.
        /// </returns>
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

            var groupVotes = _context.GroupVote.Where(v => v.GroupId == groupId);

            _context.GroupMessage.RemoveRange(groupMessages);
            _context.GroupVote.RemoveRange(groupVotes);

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
        /// Cria uma nova mensagem no grupo especificado com base no identificador único do grupo.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo onde a mensagem será criada.</param>
        /// <param name="message">Os dados da nova mensagem a ser criada.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se a mensagem for criada com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 401 (Não autorizado) se o utilizador autenticado não for membro do grupo.
        /// Um código de estado 400 (Pedido inválido) se ocorrer um erro durante o processo de criação da mensagem.
        /// </returns>
        [HttpPost("{groupId}/messages")]
        [Authorize]
        public async Task<IActionResult> CreateMessage(string groupId, [FromBody] NewGroupMessage message)
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

            if (!group.MembersId.Contains(user.Id))
            {
                return Unauthorized();
            }

            var newMessage = new GroupMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserName = user.Name,
                Message = message.message,
                GroupId = groupId
            };

            _context.GroupMessage.Add(newMessage);

            group.MessagesId.Add(newMessage.MessageId);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Obtém todas as mensagens do grupo especificado com base no seu identificador único.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo do qual as mensagens serão obtidas.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma lista de mensagens do grupo se as mensagens forem obtidas com sucesso.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado com o ID fornecido.
        /// Um código de estado 401 (Não autorizado) se o utilizador autenticado não for membro do grupo.
        /// </returns>
        [HttpGet("{groupId}/messages")]
        [Authorize]
        public async Task<IActionResult> GetMessages(string groupId)
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

            var messages = await _context.GroupMessage
          .Where(m => m.GroupId == groupId)
          .Select(m => new {
              m.UserName,
              m.Message,
          }).ToListAsync();


            return Ok(messages);
        }

        /// <summary>
        /// Cria um voto para uma propriedade específica em um grupo.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo para o qual o voto será criado.</param>
        /// <param name="vote">Os dados do novo voto a ser criado.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o voto for criado com sucesso.
        /// Um código de estado 401 (Não autorizado) se o utilizador autenticado não corresponder ao utilizador associado ao voto.
        /// Um código de estado 404 (Não encontrado) se o grupo ou a propriedade associada ao voto não forem encontrados.
        /// Um código de estado 400 (Pedido Inválido) se o utilizador autenticado não for membro do grupo ou se a propriedade não estiver associada ao grupo.
        /// </returns>
        [HttpPost("{groupId}/votes")]
        [Authorize]
        public async Task<IActionResult> CreateVote(string groupId, [FromBody] NewGroupVote vote)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.Id != vote.userId)
            {
                return Unauthorized();
            }
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (!group.MembersId.Contains(user.Id))
            {
                return Unauthorized();
            }

            if (!group.PropertiesId.Contains(vote.propertyId))
            {
                return BadRequest();
            }

            var alreadyVoted = await _context.GroupVote
                .Where(v => v.GroupId == groupId && v.UserId == vote.userId)
                .FirstOrDefaultAsync();



            var newVote = new GroupVote
            {
                VoteId = Guid.NewGuid().ToString(),
                UserId = vote.userId,
                PropertyId =vote.propertyId,
                GroupId = groupId
            };
            if (alreadyVoted != null)
            {
                _context.GroupVote.Remove(alreadyVoted);
                group.VotesId.Remove(alreadyVoted.VoteId);

            }
            
            _context.GroupVote.Add(newVote);

            group.VotesId.Add(newVote.VoteId);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Obtém todos os votos registrados para um grupo específico.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo para o qual os votos serão obtidos.</param>
        /// <returns>
        /// Um código de estado 200 (OK) juntamente com uma lista de votos se os votos forem obtidos com sucesso.
        /// Um código de estado 401 (Não autorizado) se o utilizador autenticado não corresponder ao utilizador associado ao grupo.
        /// Um código de estado 404 (Não encontrado) se o grupo não for encontrado.
        /// </returns>
        [HttpGet("{groupId}/votes")]
        [Authorize]
        public async Task<IActionResult> GetVotes(string groupId)
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

            var votes = await _context.GroupVote.Where(v => v.GroupId == groupId).ToListAsync();
            return Ok(votes);
        }


        /// <summary>
        /// Manipula a comunicação WebSocket para um grupo específico.
        /// </summary>
        /// <param name="groupId">O identificador único do grupo para o qual a comunicação WebSocket será manipulada.</param>
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
    /// <param name="propertyId">O identificador único da propriedade associada ao grupo (opcional).</param>
    /// <param name="memberEmails">Uma lista de endereços de e-mail dos membros a serem adicionados ao grupo (opcional).</param>
    public record GroupInputModel(string name, string? propertyId, List<string>? memberEmails);

    /// <summary>
    /// Modelo que representa a criação de uma nova mensagem.
    /// </summary>
    /// <param name="message">Conteúdo da mensagem</param>
    public record NewGroupMessage(string message);

    /// <summary>
    /// Modelo que representa a criação de um novo voto.
    /// </summary>
    /// <param name="propertyId">O identificador único da propriedade</param>
    /// <param name="userId">O identificador único do utilizador</param>
    public record NewGroupVote(string propertyId, string userId);
}
