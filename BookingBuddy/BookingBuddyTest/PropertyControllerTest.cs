using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingBuddyTest
{
    public class PropertyControllerTest : IClassFixture<ApplicationDbContextFixture>
    {
        private BookingBuddyServerContext _context;

        public PropertyControllerTest(ApplicationDbContextFixture context)
        {
            _context = context.DbContext;
        }
        
        [Fact]
        public async Task Create_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property = new Property
            {
                PropertyId = "propertyTest",
                Name = "Propriedade de teste",
                Description = "Descrição da propriedade de teste",
                PricePerNight = 20,
                Location = "penacony",
                ImagesUrl = new List<string>(),
                LandlordId = landlord.LandlordId
            };

            var result = await controller.PostProperty(property);

            Assert.NotNull(result);
        }

        
        [Fact]
        public async Task Get_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property2 = new Property
            {
                PropertyId = "propertyTest2",
                Name = "Propriedade de teste",
                Description = "Descrição da propriedade de teste",
                PricePerNight = 20,
                Location = "penacony",
                ImagesUrl = new List<string>(),
                LandlordId = landlord.LandlordId
            };

            var result = await controller.PostProperty(property2);

            Assert.NotNull(result);

            var propertyFromDb = await controller.GetProperty("propertyTest2");

            Assert.NotNull(propertyFromDb);
        }

        [Fact]
        public async Task Get_PropertyFromApi_IfNotExists()
        {
            var controller = new PropertyController(_context);

            var propertyFromDb = await controller.GetProperty("tdsad");

            Assert.IsAssignableFrom<ActionResult<Property>>(propertyFromDb);
        }

        [Fact]
        public async Task Edit_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property3 = new Property
            {
                PropertyId = "propertyTest3",
                Name = "Propriedade de teste para editar",
                Description = "Descrição da propriedade de teste",
                PricePerNight = 20,
                Location = "penacony",
                ImagesUrl = new List<string>(),
                LandlordId = landlord.LandlordId
            };

            var property4 = new Property
            {
                PropertyId = "propertyTest4",
                Name = "Propriedade de teste para editar",
                Description = "Descrição da propriedade de teste",
                PricePerNight = 20,
                Location = "penacony",
                ImagesUrl = new List<string>(),
                LandlordId = landlord.LandlordId
            };

            var result = await controller.PutProperty(property3.PropertyId, property4);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property5 = new Property
            {
                PropertyId = "propertyTest5",
                Name = "Propriedade de teste para remover",
                Description = "Descrição da propriedade de teste",
                PricePerNight = 20,
                Location = "penacony",
                ImagesUrl = new List<string>(),
                LandlordId = landlord.LandlordId
            };

            var result = await controller.DeleteProperty(property5.PropertyId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
