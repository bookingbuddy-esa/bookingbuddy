﻿using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Property = BookingBuddy.Server.Models.Property;

namespace BookingBuddy.Server.Controllers
{
    
    [Route("api/groups")]
    [ApiController]
    public class GroupController : Controller
    {
        
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly WebSocketWrapper<Group> WebSocketWrapper = new();

        /// <summary>
        /// Construtor da classe PropertyController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public GroupController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
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
                GroupId = Guid.NewGuid().ToString().Split("-").Last(),
                GroupOwnerId = user.Id,
                Name = model.name,
                MembersId = new List<string> { user.Id },
                PropertiesId = new List<string>()
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
                        // TODO: send invite link para o endpoint /invite
                        Console.WriteLine("TODO");
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
        /// Método que retorna um grupo que contenha o id passado por parâmetro.
        /// Caso não exista retorna que não foi encontrado.
        /// </summary>
        /// <param name="groupId">Identificador do grupo</param>
        /// <returns>O grupo, caso exista, não encontrado, caso contrário</returns>
        [HttpGet("{groupId}")]
        public async Task<ActionResult<Models.Group>> GetGroup(string groupId)
        {
            try
            {
                var group = await _context.Groups.FindAsync(groupId);
                if (group == null)
                {
                    return NotFound();
                }

                List<Property> properties = new List<Property>();
                group.PropertiesId?.ForEach(propertyId => {
                    var property = _context.Property.FirstOrDefault(p => p.PropertyId == propertyId);
                    if (property != null)
                    {
                        properties.Add(property); // TODO: não acho que seja necessário adicionar tudo o que retorna da property
                    }
                });

                group.Properties = properties;


                List<ReturnUser> users = new List<ReturnUser>();

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
                return group;
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
        /// <returns>Lista com os grupos do utilizador, caso exista, ou não encontrada, caso contrário</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGroupsByUserId(string userId)
        {
            try {
                var groups = await _context.Groups.Where(g => g.MembersId.Contains(userId)).ToListAsync();
                if (groups == null)
                {
                    return NotFound();
                }

                List<Group> groupsList = new List<Group>();
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


        [HttpPut("addProperty")]
        public async Task<IActionResult> AddProperty(string groupId, string propertyId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if(group == null)
            {
                return NotFound();
            }

            if (group.Properties.Count >= 5)
            {

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

        [HttpPut("setChoosenProperty")]
        public async Task<IActionResult> SetChoosenProperty(string groupId, string propertyId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (group.ChoosenProperty != null)
            {

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

        [HttpPut("addMember")]
        public async Task<IActionResult> AddMember(string groupId, string userId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            group.MembersId.Add(userId);
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


        [NonAction]
        public async Task HandleWebSocketAsync(string groupId, WebSocket webSocket)
        {
            var group = await _context.Groups.FindAsync(groupId);
            await WebSocketWrapper.HandleAsync(group, webSocket, async groupReceived => {
                Console.WriteLine("Group received: " + JsonSerializer.Serialize(groupReceived));
                await WebSocketWrapper.NotifyAllAsync(groupReceived);
            });
        }

    }

    public record GroupInputModel(string name, string? propertyId, List<string>? memberEmails);
}
