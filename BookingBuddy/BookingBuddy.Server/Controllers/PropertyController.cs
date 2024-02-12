using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;

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

        /// <summary>
        /// Construtor da classe PropertyController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public PropertyController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Método que retorna uma lista com todas as propriedades.
        /// </summary>
        /// <returns>Lista de propriedades</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            return await _context.Property.ToListAsync();
        }

        /// <summary>
        /// Método que retorna uma propriedade que contenha o id passado por parâmetro.
        /// Caso não exista retorna que não foi encontrada.
        /// </summary>
        /// <param name="propertyId">Identificador da propriedade</param>
        /// <returns>A propriedade, caso exista, não encontrada, caso contrário</returns>
        [HttpGet("{propertyId}")]
        public async Task<ActionResult<Property>> GetProperty(string propertyId)
        {
            var property = await _context.Property.FindAsync(propertyId);

            if (property == null)
            {
                return NotFound();
            }

            List<Amenity> amenities = [];

            property.AmenityIds?.ForEach(amenityId =>
            {
                Amenity amenity = new Amenity
                {
                    AmenityId = amenityId,
                    Name = Enum.GetName(typeof(AmenityEnum), amenityId)
                };

                amenities.Add(amenity);
            });

            property.Amenities = amenities;

            var user = await _userManager.FindByIdAsync(property.ApplicationUserId);
            property.ApplicationUser = new ReturnUser(){
                Id = user!.Id,
                Name = user.Name
            };

            return property;
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

            propertyToEdit.AmenityIds = model.AmenityIds;
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
        public async Task<ActionResult<Property>> CreateProperty([FromBody] PropertyCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var property = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                AmenityIds = model.AmenityIds,
                Name = model.Name,
                Description = model.Description,
                PricePerNight = model.PricePerNight,
                Location = model.Location,
                ImagesUrl = model.ImagesUrl
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
                else
                {
                    throw;
                }
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

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByUserId(string userId)
        {
            var properties = await _context.Property
                .Where(p => p.ApplicationUserId == userId)
                .ToListAsync();

            if (properties == null || properties.Count == 0)
            {
                return NotFound("Nenhuma propriedade encontrada para o usuário fornecido.");
            }

            return properties;
        }

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
    public record PropertyCreateModel(List<int>? AmenityIds, string Name, string Description, decimal PricePerNight, string Location, List<string> ImagesUrl);

    /// <summary>
    /// Modelo de edição de propriedade
    /// </summary>
    /// <param name="PropertyId">Identificador da propriedade</param>
    /// <param name="AmenityIds">Identificadores da lista de comodidades</param>
    /// <param name="Name">Nome da propriedade</param>
    /// <param name="Description">Descrição da propriedade</param>
    /// <param name="PricePerNight">Preço por noite da propriedade</param>
    /// <param name="Location">Localização da propriedade</param>
    /// <param name="ImagesUrl">Lista com urls das fotografias da propriedade</param>
    public record PropertyEditModel(string PropertyId, List<int>? AmenityIds, string Name, string Description, decimal PricePerNight, string Location, List<string> ImagesUrl);
}
