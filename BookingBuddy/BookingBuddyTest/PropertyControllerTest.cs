using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Mvc;

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

            var property = new PropertyCreateModel
            (
                LandlordId: landlord.LandlordId,
                AmenityIds: new List<int>(),
                Name: "Propriedade de teste",
                Description: "Descrição da propriedade de teste",
                PricePerNight: 20,
                Location: "penacony",
                ImagesUrl: new List<string>()
            );

            var result = await controller.CreateProperty(property);

            Assert.NotNull(result);
        }

        
        [Fact]
        public async Task Get_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property2 = new PropertyCreateModel
            (
                LandlordId: landlord.LandlordId,
                AmenityIds: new List<int>(),
                Name: "Propriedade de teste",
                Description: "Descrição da propriedade de teste",
                PricePerNight: 20,
                Location: "penacony",
                ImagesUrl: new List<string>()
            );

            var result = await controller.CreateProperty(property2);

            Assert.NotNull(result);

            var propertyFromDb = await controller.GetProperty($"{result.Value?.PropertyId}");

            Assert.NotNull(propertyFromDb);
        }

        [Fact]
        public async Task Get_PropertyFromApi_IfNotExists()
        {
            var controller = new PropertyController(_context);

            var propertyFromDb = await controller.GetProperty("tdsad");

            Assert.IsAssignableFrom<ActionResult<BookingBuddy.Server.Models.Property>>(propertyFromDb);
        }

        [Fact]
        public async Task Edit_PropertyFromApi_ReturnsError_IfProperty_DoesNotHave_TheGivenId()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property3 = new PropertyEditModel
            (
                PropertyId: "propertyTest3",
                LandlordId: landlord.LandlordId,
                AmenityIds: new List<int>(),
                Name: "Propriedade de teste",
                Description: "Descrição da propriedade de teste",
                PricePerNight: 20,
                Location: "penacony",
                ImagesUrl: new List<string>()
            );

            var property4 = new PropertyEditModel
            (
                PropertyId: "propertyTest4",
                LandlordId: landlord.LandlordId,
                AmenityIds: new List<int>(),
                Name: "Propriedade de teste",
                Description: "Descrição da propriedade de teste",
                PricePerNight: 20,
                Location: "penacony",
                ImagesUrl: new List<string>()
            );

            var result = await controller.EditProperty(property3.PropertyId, property4);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_PropertyFromApi()
        {
            var controller = new PropertyController(_context);
            ApplicationUser? appUser = _context.Users.FirstOrDefault(u => u.UserName == "landlord@bookingbuddy.com");
            Landlord? landlord = _context.Landlord.FirstOrDefault(l => l.LandlordId == "landlord");

            var property5 = new PropertyCreateModel
            (
                LandlordId: landlord.LandlordId,
                AmenityIds: new List<int>(),
                Name: "Propriedade de teste",
                Description: "Descrição da propriedade de teste",
                PricePerNight: 20,
                Location: "penacony",
                ImagesUrl: new List<string>()
            );

            var createResult = await controller.CreateProperty(property5);

            var result = await controller.DeleteProperty($"{createResult.Value?.PropertyId}");

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
