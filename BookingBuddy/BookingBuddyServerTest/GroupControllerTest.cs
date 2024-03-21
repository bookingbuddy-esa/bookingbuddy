using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddyServerTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyServerTest;

public class GroupControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;

    public GroupControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
    }
    
    public GroupController CreateController(string? userId = null)
    {
        return new GroupController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userId != null
                        ? new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                        {
                            new(ClaimTypes.NameIdentifier, userId),
                        }, "TestAuthentication"))
                        : new ClaimsPrincipal(),
                }
            }
        };
    }

    private async Task<Group?> CreateRandomGroup(string email = "bookingbuddy.landlord@bookingbuddy.com")
    {
        var user = await _userManager.UserManager.FindByEmailAsync(email);
        var group = new Group()
        {
            GroupId = Guid.NewGuid().ToString(),
            GroupOwnerId = user!.Id,
            Name = "Casa Teste",
            MembersId = [user.Id],
            PropertiesId = [],
            MessagesId = [],
            ChoosenProperty = null
        };

        try
        {
            _context.DbContext.Groups.Add(group);
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            return null;
        }

        return group;
    }


    [Fact]
    public async void GetGroup_Returns_Group()
    {
        var group = await CreateRandomGroup();
        Assert.NotNull(group);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetGroup(group!.GroupId);
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetProperty_Returns_NotFound_When_Property_NotFound()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetGroup(Guid.NewGuid().ToString());
        Assert.NotNull(result);
    }

    [Fact]
    public async void CreateGroup_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.CreateGroup(
            new GroupInputModel(
                "Teste",
                null,
                []
            ));
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void CreateProperty_Returns_CreatedAtAction_When_User_IsAuthenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.CreateGroup(
            new GroupInputModel(
                "Teste",
                null,
                []
            ));
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async void DeleteGroup_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var group = await CreateRandomGroup();
        Assert.NotNull(group);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.DeleteGroup(group!.GroupId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void DeleteGroup_Returns_Unauthorized_When_User_NotOwner()
    {
        var group = await CreateRandomGroup();
        Assert.NotNull(group);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.admin@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);

        var result = await controller.DeleteGroup(group!.GroupId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void DeleteGroup_Returns_Ok_When_User_IsOwner()
    {
        var group = await CreateRandomGroup();
        Assert.NotNull(group);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);

        var result = await controller.DeleteGroup(group!.GroupId);
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async void GetGroupsByUserId_Returns_NotFound_When_NoGroupsFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.admin@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetGroupsByUserId(user.Id);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetPropertiesByUserId_Returns_Ok_When_PropertiesFound()
    {
        var group1 = await CreateRandomGroup();
        Assert.NotNull(group1);

        var group2 = await CreateRandomGroup();
        Assert.NotNull(group2);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetGroupsByUserId(user.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void AddMember_Returns_CreatedAtAction_When_User_IsAuthenticated()
    {
        
        var group = await CreateRandomGroup();
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        var controller = CreateController(user.Id);

       
        var result = await controller.AddMember(group!.GroupId);

        
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async void CreateMessage_Returns_Unauthorized_When_User_IsntAuthenticated()
    {
        
        var group = await CreateRandomGroup();
        var controller = CreateController();

       
        var result = await controller.CreateMessage(group!.GroupId, new NewGroupMessage("Test message"));

        
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void CreateMessage_Returns_Ok_When_User_IsAuthenticated()
    {

        var group = await CreateRandomGroup();
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        var controller = CreateController(user!.Id);


        var result = await controller.CreateMessage(group!.GroupId, new NewGroupMessage("Test message"));


        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetMessages_Returns_Ok_When_User_IsAuthenticated()
    {
        var group = await CreateRandomGroup();
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        var controller = CreateController(user!.Id);
        var msg = new NewGroupMessage("Nova Mensagem");

        await controller.CreateMessage(group.GroupId, msg);
        var result = await controller.GetMessages(group!.GroupId) as OkObjectResult;

        Assert.NotNull(result);
        var messages = Assert.IsAssignableFrom<IEnumerable<object>>(result.Value);
        Assert.NotEmpty(messages);
    }

}