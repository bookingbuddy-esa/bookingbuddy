using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;
using BookingBuddy.Server.Services;
using Microsoft.DotNet.Scaffolding.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using NuGet.Common;
using System.Web;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de gestão de propriedades.
    /// </summary>
    [Route("api/properties")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Construtor da classe PropertyController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public PropertyController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Método que retorna uma lista com todas as propriedades.
        /// </summary>
        /// <returns>Lista de propriedades</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Models.Property>>> GetProperties()
        {
            var properties = await _context.Property.ToListAsync();
            foreach (var property in properties)
            {
                List<Amenity> amenities = [];
                property.AmenityIds?.ForEach(amenityId =>
                {
                    var amenity = _context.Amenity.FirstOrDefault(a => a.AmenityId == amenityId);
                    if (amenity != null)
                    {
                        amenities.Add(amenity);
                    }
                });
                property.Amenities = amenities;
                var user = await _userManager.FindByIdAsync(property.ApplicationUserId);
                property.ApplicationUser = new ReturnUser()
                {
                    Id = user!.Id,
                    Name = user.Name
                };
            }

            //return await _context.Property.OrderByDescending(p => p.Clicks).ToListAsync();

            var promotedPropertyIds = await _context.Order
                .Where(order => order.EndDate > DateTime.Now && order.State)
                .Select(order => order.PropertyId)
                .ToListAsync();

            var promotedProperties = await _context.Property
                .Where(property => promotedPropertyIds.Contains(property.PropertyId))
                .OrderByDescending(property => property.Clicks)
                .ToListAsync();

            var otherProperties = properties
                .Except(promotedProperties)
                .OrderByDescending(property => property.Clicks)
                .ToList();

            var orderedProperties = promotedProperties.Concat(otherProperties).ToList();

            return orderedProperties;
        }

        /// <summary>
        /// Método que retorna uma propriedade que contenha o id passado por parâmetro.
        /// Caso não exista retorna que não foi encontrada.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade</param>
        /// <returns>A propriedade, caso exista, não encontrada, caso contrário</returns>
        [HttpGet("{propertyId}")]
        public async Task<ActionResult<Models.Property>> GetProperty(string propertyId)
        {
            try
            {
                var property = await _context.Property.FindAsync(propertyId);

                if (property == null)
                {
                    return NotFound();
                }

                property.Clicks += 1;
                await _context.SaveChangesAsync();

                List<Amenity> amenities = [];

                property.AmenityIds?.ForEach(amenityId =>
                {
                    var amenity = _context.Amenity.FirstOrDefault(a => a.AmenityId == amenityId);
                    if (amenity != null)
                    {
                        amenities.Add(amenity);
                    }
                });

                property.Amenities = amenities;

                var user = await _userManager.FindByIdAsync(property.ApplicationUserId);
                property.ApplicationUser = new ReturnUser()
                {
                    Id = user!.Id,
                    Name = user.Name
                };

                return property;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Método que retorna as métricas de uma propriedade.
        /// </summary>
        /// <param name="propertyId">Id da propriedade</param>
        /// <returns>As métricas da propriedade</returns>
        [HttpGet]
        [Authorize]
        [Route("metrics/{propertyId}")]
        public async Task<IActionResult> GetMetrics(string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound();
            }

            if (user.Id != property.ApplicationUserId)
            {
                return Forbid();
            }


            var propertyRatings = await _context.Rating
                .Where(r => r.PropertyId == propertyId)
                .Include(r => r.ApplicationUser)
                .Select(r => new
                {
                    r.RatingId,
                    applicationUser = new ReturnUser()
                    {
                        Id = r.ApplicationUserId,
                        Name = r.ApplicationUser!.Name
                    },
                    r.Value,
                })
                .ToListAsync();

            var bookingOrders = await _context.BookingOrder
                .Include(bo => bo.Order)
                .Include(bo => bo.Order!.Payment)
                .Include(bo => bo.Order!.ApplicationUser)
                .Where(bo => bo.Order!.PropertyId == property.PropertyId)
                .Select(bo => new
                {
                    bo.BookingOrderId,
                    applicationUser = new ReturnUser()
                    {
                        Id = bo.Order!.ApplicationUserId,
                        Name = bo.Order.ApplicationUser!.Name
                    },
                    bo.Order!.StartDate,
                    bo.Order!.EndDate,
                    bo.Order.Payment!.Amount
                })
                .ToListAsync();


            var metrics = new
            {
                propertyId = property.PropertyId,
                clicks = property.Clicks,
                ratings = propertyRatings,
                orders = bookingOrders
            };

            return Ok(metrics);
        }

        /// <summary>
        /// Método que atualiza uma propriedade existente na base de dados da aplicação.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade a atualizar</param>
        /// <param name="model">Modelo de edição de uma propriedade</param>
        /// <returns>Não encontrada, caso a propriedade não exista na base dados, ou uma exceção, ou sem conteúdo, caso contrário</returns>
        [HttpPut("edit/{propertyId}")]
        [Authorize]
        public async Task<IActionResult> EditProperty(string propertyId, [FromBody] PropertyEditModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var propertyToEdit = await _context.Property.FirstOrDefaultAsync(p => p.PropertyId == propertyId);

            if (propertyToEdit == null)
            {
                return NotFound();
            }

            if (user.Id != propertyToEdit.ApplicationUserId)
            {
                return Forbid();
            }

            if (propertyId != model.PropertyId)
            {
                return BadRequest();
            }

            List<Amenity> amenities = [];

            model.Amenities?.ForEach(amenityName =>
            {
                var amenity =
                    _context.Amenity.FirstOrDefault(a => string.Equals(a.Name.ToUpper(), amenityName.ToUpper()));
                if (amenity != null)
                {
                    amenities.Add(amenity);
                }
            });

            propertyToEdit.AmenityIds = amenities.Select(a => a.AmenityId).ToList();
            propertyToEdit.Name = model.Name;
            propertyToEdit.Description = model.Description;
            propertyToEdit.PricePerNight = model.PricePerNight;
            propertyToEdit.Location = model.Location;
            propertyToEdit.ImagesUrl = model.ImagesUrl;

            _context.Entry(propertyToEdit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(propertyId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Método que adiciona uma nova propriedade na base de dados da aplicação.
        /// </summary>
        /// <param name="model">Modelo de criação de uma propriedade</param>
        /// <returns>A propriedade criada, um conflito, caso já exista uma propriedade com o mesmo identificador, ou uma exceção caso contrário</returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            List<Amenity> amenities = [];

            model.Amenities?.ForEach(amenityName =>
            {
                var amenity =
                    _context.Amenity.FirstOrDefault(a => string.Equals(a.Name.ToUpper(), amenityName.ToUpper()));
                if (amenity != null)
                {
                    amenities.Add(amenity);
                }
            });

            var property = new Models.Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                Name = model.Name,
                Description = model.Description,
                PricePerNight = model.PricePerNight,
                Location = model.Location,
                AmenityIds = amenities.Select(a => a.AmenityId).ToList(),
                ImagesUrl = model.ImagesUrl,
                Clicks = 0
            };

            _context.Property.Add(property);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PropertyExists(property.PropertyId))
                {
                    return Conflict();
                }

                throw;
            }

            if (!await _userManager.IsInRoleAsync(user, "landlord"))
            {
                await _userManager.AddToRoleAsync(user, "landlord");
            }

            return CreatedAtAction("GetProperty", new { propertyId = property.PropertyId }, property);
        }

        /// <summary>
        /// Método que remove uma propriedade da base de dados da aplicação.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade a remover</param>
        /// <returns>Propriedade não encontrada, caso não exista nenhuma propriedade com o identificador fornecido, ou sem conteúdo, caso contrário</returns>
        [HttpDelete("delete/{propertyId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound();
            }

            if (user.Id != property.ApplicationUserId)
            {
                return Forbid();
            }

            _context.Property.Remove(property);
            await _context.SaveChangesAsync();

            if(!_context.Property.Any(p => p.ApplicationUserId == user.Id))
            {
                await _userManager.RemoveFromRoleAsync(user, "landlord");
            }
            
            return NoContent();
        }

        /// <summary>
        /// Método que obtém as propriedades de um utilizador.
        /// </summary>
        /// <param name="userId">Identificador do utilizador</param>
        /// <returns>Lista com as propriedades do utilizador, caso exista, ou não encontrada, caso contrário</returns>
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertiesByUserId(string userId)
        {
            var properties = await _context.Property
                .Where(p => p.ApplicationUserId == userId)
                .ToListAsync();

            if (properties == null || properties.Count == 0)
            {
                return NotFound("Nenhuma propriedade encontrada para o utilizador fornecido.");
            }

            return Ok(properties);
        }


        /// <summary>
        /// Método que obtém as datas bloqueadas de uma propriedade.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade</param>
        /// <returns>Lista com as datas bloqueadas de uma propriedade, caso exista, ou não encontrada, caso contrário</returns>
        [HttpGet("blockedDates/{propertyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyBlockedDates(string propertyId)
        {
            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            var blockedDates = await _context.BlockedDate
                .Where(b => b.PropertyId == propertyId)
                .ToListAsync();

            return Ok(blockedDates);
        }

        /// <summary>
        /// Método que bloqueia as datas no calendario de uma propriedade.
        /// </summary>
        /// <param name="inputModel">Modelo de criação de uma BlockedDate</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 200 OK: Operação bem-sucedida, as datas foram bloqueadas com sucesso.
        /// - 400 Bad Request: O modelo de entrada é inválido.
        /// </returns>
        [Authorize]
        [HttpPost("blockDates")]
        public async Task<IActionResult> BlockDates([FromBody] BlockDateInputModel inputModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var property = await _context.Property.FindAsync(inputModel.PropertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            if (user.Id != property.ApplicationUserId)
            {
                return Forbid();
            }

            var blockedDate = new BlockedDate
            {
                Start = inputModel.StartDate,
                End = inputModel.EndDate,
                PropertyId = inputModel.PropertyId
            };

            try
            {
                _context.BlockedDate.Add(blockedDate);
                await _context.SaveChangesAsync();

                return Ok("Datas bloqueadas com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que desbloqueia uma data de uma propriedade.
        /// </summary>
        /// <param name="id">Identificador da data a desbloquear</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 200 OK: Operação bem-sucedida, a data foi desbloqueada com sucesso.
        /// - 404 Not Found: A data com o identificador fornecido não foi encontrada. 
        /// </returns>
        [Authorize]
        [HttpDelete("unblock/{id}")]
        public async Task<IActionResult> UnblockDates(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var blockedDate = await _context.BlockedDate.FindAsync(id);

            if (blockedDate == null)
            {
                return NotFound();
            }

            var property = await _context.Property.FindAsync(blockedDate.PropertyId);

            if (user.Id != property.ApplicationUserId)
            {
                return Forbid();
            }

            _context.BlockedDate.Remove(blockedDate);
            await _context.SaveChangesAsync();

            return Ok("Dates unblocked successfully");
        }

        /// <summary>
        /// Obtém as reservas associadas do utilizador com login efetuado.
        /// </summary>
        /// <returns>Ação HTTP que representa o resultado da operação.</returns>
        [HttpGet("bookings")]
        [Authorize]
        public async Task<IActionResult> GetAssociatedBookings()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var properties = await _context.Property
                .Where(p => p.ApplicationUserId == user.Id)
                .ToListAsync();

            var associatedBookingOrders = await _context.BookingOrder
                .Include(bo => bo.Order)
                .Include(bo => bo.Order!.Payment)
                .Include(bo => bo.Order!.ApplicationUser)
                .Where(bo => properties.Select(p => p.PropertyId).Contains(bo.Order!.PropertyId))
                .Select(bo => new
                {
                    bo.BookingOrderId,
                    bo.OrderId,
                    applicationUser = new ReturnUser()
                    {
                        Id = bo.Order!.ApplicationUserId,
                        Name = bo.Order.ApplicationUser!.Name
                    },
                    bo.Order!.Property!.Name,
                    guest = bo.Order!.ApplicationUser.Name,
                    checkIn = bo.Order!.StartDate,
                    checkOut = bo.Order!.EndDate,
                    bo.Order!.State,
                    bo.Order.Payment!.Amount,
                    bo.NumberOfGuests,
                })
                .ToListAsync();


            return Ok(associatedBookingOrders);
        }

        /// <summary>
        /// Método que obtém os descontos de uma propriedade.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade</param>
        /// <returns>Lista com os descontos de uma propriedade, caso exista, ou não encontrada, caso contrário</returns>
        [AllowAnonymous]
        [HttpGet("discounts/{propertyId}")]
        public async Task<IActionResult> GetPropertyDiscounts(string propertyId)
        {
            var discounts = await _context.Discount
                .Where(b => b.PropertyId == propertyId)
                .ToListAsync();

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            return Ok(discounts);
        }


        /// <summary>
        /// Método que retorna todos os users que tiverem favoritos
        /// </summary>
        /// <param name="favoriteUserIds"></param>
        /// <returns></returns>
        ///[HttpGet('properties/fav')]
        private async Task<IEnumerable<ApplicationUser>> GetUserListIdFavorites(IEnumerable<string> favoriteUserIds)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var userId in favoriteUserIds)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// Método que adiciona um desconto no calendario de uma propriedade.
        /// </summary>
        /// <param name="inputModel">Modelo de criação de um Discount</param>
        /// <param name="sendEmail">Indica se deve enviar um email para os utilizadores que têm a propriedade nos favoritos</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 200 OK: Operação bem-sucedida, o desconto foi adicionado com sucesso.
        /// - 400 Bad Request: O modelo de entrada é inválido.
        /// </returns>
        [Authorize]
        [HttpPost("createDiscount")]
        public async Task<IActionResult> ApplyDiscount([FromBody] DiscountInputModel inputModel, bool sendEmail = true)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var property = await _context.Property.FindAsync(inputModel.PropertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            if (currentUser.Id != property.ApplicationUserId)
            {
                return Forbid();
            }

            try
            {
                var discount = new Discount
                {
                    DiscountId = Guid.NewGuid().ToString(),
                    DiscountAmount = inputModel.Amount,
                    StartDate = inputModel.StartDate,
                    EndDate = inputModel.EndDate,
                    PropertyId = inputModel.PropertyId
                };

                _context.Discount.Add(discount);
                await _context.SaveChangesAsync();

                if (sendEmail)
                {
                    //TODO: select dos applicationUserId na tabela favoritos ao propertyid = inputModel.property id

                    var userIdsFavorites = await _context.Favorites
                        .Where(f => f.PropertyId == inputModel.PropertyId)
                        .Select(f => f.ApplicationUserId)
                        .ToListAsync();

                    IEnumerable<ApplicationUser> userListIdsFavorite = await GetUserListIdFavorites(userIdsFavorites);


                    foreach (var user in userListIdsFavorite)
                    {
                        var propertyLink =
                            $"{_configuration.GetSection("Front-End-Url").Value!}/property/" + inputModel.PropertyId;

                        await EmailSender.SendTemplateEmail(_configuration.GetSection("MailAPIKey").Value!,
                            "d-14f3e58637f14d9d9cfb8da43a1dad7f", user.Email!, user.Name,
                            new { propertyLink });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Disconto aplicado com sucesso.");
        }

        /// <summary>
        /// Método que remove um desconto de uma propriedade.
        /// </summary>
        /// <param name="id">Identificador do desconto</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 200 OK: Operação bem-sucedida, o desconto foi removido com sucesso.
        /// - 404 Not Found: O desconto com o identificador fornecido não foi encontrada. 
        /// </returns>
        [Authorize]
        [HttpDelete("removeDiscount/{id}")]
        public async Task<IActionResult> RemoveDiscount(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var discount = await _context.Discount.FindAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            var property = await _context.Property.FindAsync(discount.PropertyId);

            if (property == null)
            {
                return BadRequest("Propriedade não encontrada.");
            }

            if (user.Id != property.ApplicationUserId)
            {
                return Forbid();
            }

            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();

            return Ok("Discount removed successfully");
        }

        /// <summary>
        /// Adiciona um propriedade aos favoritos
        /// </summary>
        /// <param name="propertyId"> Parametro que contém o id da propriedade a ser adicionada aos favoritos</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [Authorize]
        [HttpPost("favorites/add/{propertyId}")]
        public async Task<IActionResult> AddToFavorite(string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            if (_context.Favorites.Any(f => f.ApplicationUserId == user.Id && f.PropertyId == propertyId))
                return Conflict("A propriedade já está na lista de favoritos.");

            var favorite = new Favorite
            {
                ApplicationUserId = user.Id,
                PropertyId = propertyId
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok("Propriedade adicionada aos favoritos com sucesso.");
        }

        /// <summary>
        /// Remove uma propriedade dos favoritos
        /// </summary>
        /// <param name="propertyId">Parametro que contém o id da propriedade a ser removida dos favoritos</param>
        /// <returns>Mensagem de feedback, notFound, BadRequest ou Ok</returns>
        [HttpDelete]
        [Route("favorites/remove/{propertyId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromFavorite(string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound("Nenhuma propriedade encontrada para o identificador fornecido.");
            }

            var favorite =
                _context.Favorites.FirstOrDefault(f => f.ApplicationUserId == user.Id && f.PropertyId == propertyId);

            if (favorite == null)
            {
                return BadRequest("A propriedade não está na lista de favoritos.");
            }

            _context.Favorites.Remove(favorite);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Erro ao remover a propriedade dos favoritos.");
            }

            return Ok("Propriedade removida dos favoritos com sucesso.");
        }

        /// <summary>
        /// Método que obtem as propriedades que o utilizador marcou como favoritas
        /// </summary>
        /// <param name="userId"> Parametro que fornece o id do utilizador</param>
        /// <returns>Ação HTTP que representa o resultado da operação, neste caso, os favoritos</returns>
        [HttpGet("favorites/user/{userId}")]
        public async Task<IActionResult> GetUserFavorites(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context.Favorites
                .Include(f => f.Property)
                .Where(f => f.ApplicationUserId == userId)
                .Select(f => f.Property)
                .ToListAsync();

            return Ok(favorites);
        }

        /// <summary>
        /// Método que verifica se a propriedade escolhida está nos favoritos do utilizador
        /// </summary>
        /// <param name="propertyId">Parametro que contém o id da propriedade a ser analisada</param>
        /// <returns>ação HTTP que representa o resultado da operação.</returns>
        [HttpGet("favorites/check/{propertyId}")]
        public async Task<IActionResult> IsPropertyInFavorites(string propertyId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var isInFavorites = await _context.Favorites
                .AnyAsync(f => f.ApplicationUserId == user.Id && f.PropertyId == propertyId);

            return Ok(isInFavorites);
        }


        /// <summary>
        /// Método que verifica se uma propriedade existe.
        /// </summary>
        /// <param name="id">Identificador da propriedade.</param>
        /// <returns>
        /// Retorna verdadeiro se uma propriedade com o identificador fornecido existir; caso contrário, retorna falso.
        /// </returns>
        private bool PropertyExists(string id)
        {
            return _context.Property.Any(e => e.PropertyId == id);
        }
    }

    /// <summary>
    /// Modelo de criação de propriedade
    /// </summary>
    /// <param name="AmenityIds">Identificadores da lista de comodidades</param>
    /// <param name="Name">Nome da propriedade</param>
    /// <param name="Description">Descrição da propriedade</param>
    /// <param name="PricePerNight">Preço por noite da propriedade</param>
    /// <param name="Location">Localização da propriedade</param>
    /// <param name="ImagesUrl">Lista com urls das fotografias da propriedade</param>
    public record PropertyCreateModel(
        string Name,
        string Description,
        decimal PricePerNight,
        string Location,
        List<string>? Amenities,
        List<string> ImagesUrl);

    /// <summary>
    /// Modelo de edição de propriedade
    /// </summary>
    /// <param name="PropertyId">Identificador da propriedade</param>
    /// <param name="Amenities">Lista de comodidades</param>
    /// <param name="Name">Nome da propriedade</param>
    /// <param name="Description">Descrição da propriedade</param>
    /// <param name="PricePerNight">Preço por noite da propriedade</param>
    /// <param name="Location">Localização da propriedade</param>
    /// <param name="Amenities">Lista de comodidades</param>
    /// <param name="ImagesUrl">Lista com urls das fotografias da propriedade</param>
    public record PropertyEditModel(
        string PropertyId,
        string Name,
        string Description,
        decimal PricePerNight,
        string Location,
        List<string>? Amenities,
        List<string> ImagesUrl);

    /// <summary>
    /// Modelo de criação de uma BlockedDate
    /// </summary>
    /// <param name="StartDate">Data Inicial do Bloqueio</param>
    /// <param name="EndDate">Data Final do bloqueio</param>
    /// <param name="PropertyId">Identificador da Propriedade</param>-
    public record BlockDateInputModel(string StartDate, string EndDate, string PropertyId);

    /// <summary>
    /// Modelo de criação de um desconto
    /// </summary>
    /// <param name="Amount">Quantia do Desconto</param>
    /// <param name="StartDate">Data Inicial do Bloqueio</param>
    /// <param name="EndDate">Data Final do bloqueio</param>
    /// <param name="PropertyId">Identificador da Propriedade</param>-
    public record DiscountInputModel(int Amount, DateTime StartDate, DateTime EndDate, string PropertyId);
}