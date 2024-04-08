using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookingBuddy.Server.Services;

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
        /// <param name="configuration">Configuração da aplicação</param>
        public PropertyController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // TODO: apagar depois
        // create a test dev endpoint to create 50 properties random
        [HttpGet("createTestProperties")]
        [Authorize]
        public async Task<IActionResult> CreateTestProperties()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Unauthorized();
            }

            var amenities = await _context.Amenity.ToListAsync();
            var random = new Random();
            for(int i=0; i<50; i++){
                var property = new Property
                {
                    PropertyId = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    Name = "Property " + i,
                    Description = "Description " + i,
                    PricePerNight = random.Next(50, 200),
                    MaxGuestsNumber = random.Next(1, 10),
                    RoomsNumber = random.Next(1, 5),
                    Location = "Location " + i,
                    AmenityIds = amenities.Select(a => a.AmenityId).ToList(),
                    ImagesUrl = new List<string> { "https://via.placeholder.com/150" },
                    Clicks = 0
                };

                _context.Property.Add(property);
            }

            await _context.SaveChangesAsync();
            return Ok("Properties created successfully");
        }

        /// <summary>
        /// Método que retorna o número total de propriedades existentes na base de dados.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertiesCount()
        {
            var properties = await _context.Property.ToListAsync();
            return Ok(properties.Count);
        }

        /// <summary>
        /// Método que retorna uma lista das propriedades existentes na base de dados, com paginação aplicada.
        /// A paginação começa na página fornecida (startIndex) e retorna um número específico de propriedades (itemsPerPage).
        /// </summary>
        /// <param name="itemsPerPage">Número de propriedades a pesquisar</param>
        /// <param name="startIndex">Número da página</param>
        /// <returns>Uma lista das propriedades na página especificada. Se não houver propriedades, retorna uma lista vazia.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProperties([FromQuery] int itemsPerPage = 50,[FromQuery] int startIndex = 0)
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
                property.ApplicationUser = new ReturnUser
                {
                    Id = user!.Id,
                    Name = user.Name
                };
            }

            var promotedPropertyIds = await _context.PromoteOrder
                .Where(order => order.EndDate > DateTime.Now && order.State == OrderState.Paid)
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

            var orderedProperties = promotedProperties.Concat(otherProperties).Skip(startIndex).Take(itemsPerPage).ToList();

            return Ok(orderedProperties);
        }


        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertiesBySearch([FromQuery] string search, [FromQuery] int itemsPerPage = 50, [FromQuery] int startIndex = 0)
        {

            var properties = await _context.Property
                .Where(p => p.Location.Contains(search) || p.Name.Contains(search)) 
                .ToListAsync();
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
                property.ApplicationUser = new ReturnUser
                {
                    Id = user!.Id,
                    Name = user.Name
                };
            }

            var propertyIds = properties.Select(p => p.PropertyId).ToList();

            var promotedPropertyIds = await _context.PromoteOrder
                .Where(order => order.EndDate > DateTime.Now && order.State == OrderState.Paid && propertyIds.Contains(order.PropertyId))
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

            var orderedProperties = promotedProperties.Concat(otherProperties).Skip(startIndex).Take(itemsPerPage).ToList();

            return Ok(properties);
        }

        /// <summary>
        /// Método que retorna os detalhes de uma propriedade com base no identificador fornecido.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade para a qual os detalhes são solicitados.</param>
        /// <returns>
        /// Os detalhes da propriedade especificada, incluindo informações como clicks, comodidades e utilizador associado.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada na base de dados.
        /// </returns>
        [HttpGet("{propertyId}")]
        public async Task<IActionResult> GetProperty(string propertyId)
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

                return Ok(property);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Método que retorna as métricas de uma propriedade com base no identificador fornecido.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade para a qual as métricas são solicitadas.</param>
        /// <returns>
        /// As métricas da propriedade especificada, incluindo o número de cliques, classificações e pedidos de reserva.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para visualizar as métricas da propriedade.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada na base de dados.
        /// </returns>
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

            // TODO: em falta - contabilizar as reservas de grupo

            var bookingOrders = await _context.BookingOrder
                .Include(bo => bo.Payment)
                .Include(bo => bo.ApplicationUser)
                .Where(bo => bo.PropertyId == property.PropertyId)
                .Select(bo => new
                {
                    applicationUser = new ReturnUser()
                    {
                        Id = bo.ApplicationUserId,
                        Name = bo.ApplicationUser!.Name
                    },
                    bo.StartDate,
                    bo.EndDate,
                    bo.Payment!.Amount
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
        /// Método que permite editar uma propriedade existente.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade a ser editada.</param>
        /// <param name="model">O modelo contendo as informações atualizadas da propriedade.</param>
        /// <returns>
        /// Um código de estado 204 (No Content) se a edição for bem-sucedida.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para editar a propriedade.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada na base de dados.
        /// Retorna 400 (Bad Request) se o identificador da propriedade no modelo não corresponder ao identificador fornecido.
        /// </returns>
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
        /// Método que cria uma nova propriedade na base de dados.
        /// </summary>
        /// <param name="model">O modelo contendo as informações da propriedade a ser criada.</param>
        /// <returns>
        /// Um código de estado 201 (Created) e os detalhes da propriedade criada se a operação for bem-sucedida.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 409 (Conflict) se ocorrer um conflito durante a criação da propriedade.
        /// </returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (model.RoomsNumber > 100 || model.MaxGuestsNumber > 50)
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

            var property = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                Name = model.Name,
                Description = model.Description,
                PricePerNight = model.PricePerNight,
                MaxGuestsNumber = model.MaxGuestsNumber,
                RoomsNumber = model.RoomsNumber,
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
        /// Método que elimina uma propriedade da base de dados com base no identificador fornecido.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade a ser eliminada.</param>
        /// <returns>
        /// Um código de estado 204 (No Content) se a eliminação for bem-sucedida.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para eliminar a propriedade.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada na base de dados.
        /// </returns>
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
        /// Método que retorna todas as propriedades associadas a um utilizador com base no identificador fornecido.
        /// </summary>
        /// <param name="userId">O identificador único do utilizador para o qual as propriedades são solicitadas.</param>
        /// <returns>
        /// Uma lista das propriedades associadas ao utilizador especificado.
        /// Retorna 404 (Not Found) se não forem encontradas propriedades para o utilizador fornecido.
        /// </returns>
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertiesByUserId(string userId)
        {
            var properties = await _context.Property
                .Where(p => p.ApplicationUserId == userId)
                .ToListAsync();

            if (properties.Count == 0)
            {
                return NotFound("Nenhuma propriedade encontrada para o utilizador fornecido.");
            }

            return Ok(properties);
        }

        /// <summary>
        /// Método que retorna as datas bloqueadas de uma propriedade com base no identificador fornecido.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade para a qual as datas bloqueadas são solicitadas.</param>
        /// <returns>
        /// Uma lista das datas bloqueadas associadas à propriedade especificada.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// </returns>
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
        /// Método que bloqueia um intervalo de datas para uma propriedade específica.
        /// </summary>
        /// <param name="inputModel">O modelo contendo as informações das datas a serem bloqueadas.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se as datas forem bloqueadas com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para bloquear datas para a propriedade.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// Retorna 400 (Bad Request) se ocorrer um erro ao bloquear as datas.
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
        /// Método que desbloqueia uma data previamente bloqueada para uma propriedade.
        /// </summary>
        /// <param name="id">O identificador único da data bloqueada a ser desbloqueada.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se a data for desbloqueada com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para desbloquear a data.
        /// Retorna 404 (Not Found) se a data bloqueada não for encontrada para o identificador fornecido.
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

            if (user.Id != property!.ApplicationUserId)
            {
                return Forbid();
            }

            _context.BlockedDate.Remove(blockedDate);
            await _context.SaveChangesAsync();

            return Ok("Dates unblocked successfully");
        }

        /// <summary>
        /// Método que retorna as reservas associadas às propriedades do utilizador autenticado.
        /// </summary>
        /// <returns>
        /// Uma lista das reservas associadas às propriedades do utilizador autenticado.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// </returns>
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
                .Include(bo => bo.Payment)
                .Include(bo => bo.ApplicationUser)
                .Where(bo => properties.Select(p => p.PropertyId).Contains(bo.PropertyId))
                .Select(bo => new
                {
                    bo.OrderId,
                    applicationUser = new ReturnUser()
                    {
                        Id = bo.ApplicationUserId,
                        Name = bo.ApplicationUser!.Name
                    },
                    bo.Property!.Name,
                    guest = bo.ApplicationUser.Name,
                    checkIn = bo.StartDate,
                    checkOut = bo.EndDate,
                    bo.State,
                    bo.Payment!.Amount,
                    bo.NumberOfGuests,
                })
                .ToListAsync();


            return Ok(associatedBookingOrders);
        }

        /// <summary>
        /// Método que retorna os descontos associados a uma propriedade com base no identificador fornecido.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade para a qual os descontos são solicitados.</param>
        /// <returns>
        /// Uma lista dos descontos associados à propriedade especificada.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// </returns>
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
        /// Método que aplica um desconto a uma propriedade e, opcionalmente, notifica os utilizadores favoritos sobre o desconto.
        /// </summary>
        /// <param name="inputModel">O modelo contendo as informações do desconto a ser aplicado.</param>
        /// <param name="sendEmail">Indica se deve ser enviada uma notificação por email aos utilizadores favoritos, por padrão é enviado email.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se o desconto for aplicado com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para aplicar o desconto à propriedade.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// Retorna 400 (Bad Request) se ocorrer um erro ao aplicar o desconto.
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

                    var userListIdsFavorite = await GetUserListIdFavorites(userIdsFavorites);


                    foreach (var user in userListIdsFavorite)
                    {
                        var propertyLink =
                            $"{_configuration["ClientUrl"]}/property/" + inputModel.PropertyId;

                        await EmailSender.SendTemplateEmail(_configuration["MailAPIKey"] ?? "",
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
        /// Método que remove um desconto de uma propriedade com base no identificador do desconto.
        /// </summary>
        /// <param name="id">O identificador único do desconto a ser removido.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se o desconto for removido com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 403 (Forbid) se o utilizador autenticado não tiver permissão para remover o desconto da propriedade.
        /// Retorna 404 (Not Found) se o desconto não for encontrado para o identificador fornecido.
        /// Retorna 400 (Bad Request) se a propriedade associada ao desconto não for encontrada.
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
        /// Método que adiciona uma propriedade à lista de favoritos do utilizador autenticado.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade a ser adicionada aos favoritos.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se a propriedade for adicionada aos favoritos com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// Retorna 409 (Conflict) se a propriedade já estiver na lista de favoritos do utilizador.
        /// </returns>
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
        /// Método que remove uma propriedade da lista de favoritos do utilizador autenticado.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade a ser removida dos favoritos.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e uma mensagem de sucesso se a propriedade for removida dos favoritos com êxito.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// Retorna 404 (Not Found) se a propriedade não for encontrada para o identificador fornecido.
        /// Retorna 400 (Bad Request) se a propriedade não estiver na lista de favoritos do utilizador.
        /// </returns>
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
        /// Método que retorna as propriedades favoritas de um utilizador com base no identificador do utilizador fornecido.
        /// </summary>
        /// <param name="userId">O identificador único do utilizador para o qual as propriedades favoritas são solicitadas.</param>
        /// <returns>
        /// Uma lista das propriedades favoritas do utilizador especificado.
        /// Retorna 404 (Not Found) se o utilizador não for encontrado para o identificador fornecido.
        /// </returns>
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
        /// Método que verifica se uma propriedade está na lista de favoritos do utilizador autenticado.
        /// </summary>
        /// <param name="propertyId">O identificador único da propriedade a ser verificada.</param>
        /// <returns>
        /// Um código de estado 200 (OK) e verdadeiro se a propriedade estiver na lista de favoritos do utilizador.
        /// Um código de estado 200 (OK) e falso se a propriedade não estiver na lista de favoritos do utilizador.
        /// Retorna 401 (Unauthorized) se o utilizador não estiver autenticado.
        /// </returns>
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
        /// Verifica se uma propriedade existe na base de dados com base no seu identificador.
        /// </summary>
        /// <param name="id">O identificador único da propriedade a ser verificada.</param>
        /// <returns>
        /// Verdadeiro se a propriedade existir na base de dados; falso caso contrário.
        /// </returns>
        private bool PropertyExists(string id)
        {
            return _context.Property.Any(e => e.PropertyId == id);
        }
    }

    /// <summary>
    /// Modelo utilizado para criar uma nova propriedade.
    /// </summary>
    /// <param name="Name">Nome da propriedade</param>
    /// <param name="Description">Descrição da propriedade</param>
    /// <param name="PricePerNight">Preço por noite da propriedade</param>
    /// <param name="MaxGuestsNumber">Numero máximo de hóspedes da propriedade</param>
    /// <param name="RoomsNumber">Numero de quartos da propriedade</param>
    /// <param name="Location">Localização da propriedade</param>
    /// <param name="ImagesUrl">Lista com URLs das imagens da propriedade</param>
    public record PropertyCreateModel(
        string Name,
        string Description,
        decimal PricePerNight,
        int MaxGuestsNumber,
        int RoomsNumber,
        string Location,
        List<string>? Amenities,
        List<string> ImagesUrl);

    /// <summary>
    /// Modelo utilizado para editar uma propriedade existente.
    /// </summary>
    /// <param name="PropertyId">Identificador da propriedade</param>
    /// <param name="Amenities">Lista de comodidades</param>
    /// <param name="Name">Nome da propriedade</param>
    /// <param name="Description">Descrição da propriedade</param>
    /// <param name="PricePerNight">Preço por noite da propriedade</param>
    /// <param name="Location">Localização da propriedade</param>
    /// <param name="ImagesUrl">Lista com URLs das imagens da propriedade</param>
    public record PropertyEditModel(
        string PropertyId,
        string Name,
        string Description,
        decimal PricePerNight,
        string Location,
        List<string>? Amenities,
        List<string> ImagesUrl);

    /// <summary>
    /// Modelo utilizado para criar uma data bloqueada (BlockedDate)
    /// </summary>
    /// <param name="StartDate">Data Inicial do bloqueio</param>
    /// <param name="EndDate">Data Final do bloqueio</param>
    /// <param name="PropertyId">Identificador único da Propriedade</param>
    public record BlockDateInputModel(string StartDate, string EndDate, string PropertyId);

    /// <summary>
    /// Modelo utilizado para criar um desconto (Discount)
    /// </summary>
    /// <param name="Amount">Quantia do Desconto</param>
    /// <param name="StartDate">Data Inicial do Bloqueio</param>
    /// <param name="EndDate">Data Final do bloqueio</param>
    /// <param name="PropertyId">Identificador da Propriedade</param>
    public record DiscountInputModel(int Amount, DateTime StartDate, DateTime EndDate, string PropertyId);
}