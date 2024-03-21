using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using BookingBuddy.Server.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("https://localhost:4200", "https://localhost:7048")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var azureSignalrConnectionString = builder.Configuration.GetConnectionString("AzureSignalR") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'AzureSignalR' not found.");
builder.Services.AddSignalR().AddAzureSignalR(options => { options.ConnectionString = azureSignalrConnectionString; });

builder.Services.AddDbContext<BookingBuddyServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingBuddyServerContext") ??
                         throw new InvalidOperationException(
                             "Connection string 'BookingBuddyServerContext' not found.")));

builder.Services.AddAuthorization().ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookingBuddyServerContext>()
    .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options => { options.SignIn.RequireConfirmedAccount = true; });

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(30);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PaymentController, PaymentController>();
builder.Services.AddScoped<GroupController, GroupController>();
builder.Services.AddScoped<ChatController, ChatController>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (true) // TODO: Atualizar condição para "app.Environment.IsDevelopment()"
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ChatHubController>("/hubs/chat");

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

app.Map("/api/payments/ws", async (HttpContext httpContext, PaymentController paymentController, string paymentId) =>
{
    if (httpContext.WebSockets.IsWebSocketRequest && !string.IsNullOrEmpty(paymentId))
    {
        var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
        await paymentController.HandleWebSocketAsync(paymentId, webSocket);
    }
    else
    {
        httpContext.Response.StatusCode = 400;
    }
});

app.Map("/api/groups/ws", async (HttpContext httpContext, GroupController groupController, string groupId) =>
{
    if (httpContext.WebSockets.IsWebSocketRequest && !string.IsNullOrEmpty(groupId))
    {
        var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
        await groupController.HandleWebSocketAsync(groupId, webSocket);
    }
    else
    {
        httpContext.Response.StatusCode = 400;
    }
});

app.Map("/api/chat/ws", async (HttpContext httpContext, ChatController chatController, string chatId) =>
{
    if (httpContext.WebSockets.IsWebSocketRequest && !string.IsNullOrEmpty(chatId))
    {
        var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
        await chatController.HandleWebSocketAsync(chatId, webSocket);
    }
    else
    {
        httpContext.Response.StatusCode = 400;
    }
});


app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();