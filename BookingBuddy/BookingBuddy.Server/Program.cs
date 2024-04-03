using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using BookingBuddy.Server.Controllers;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("https://localhost:4200", "https://localhost:7048")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddDbContext<BookingBuddyServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingBuddyServerContext") ??
                         throw new InvalidOperationException(
                             "Connection string 'BookingBuddyServerContext' not found.")));

builder.Services.AddAuthorization().ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = true;
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

app.UseWebSockets(new WebSocketOptions
{
    //AllowedOrigins = { builder.Configuration.GetSection("Front-End-Url").Value ?? "" }
});

app.Map("/api/payments/ws", async (HttpContext httpContext, PaymentController paymentController, string? paymentId) =>
{
    try
    {
        if (httpContext.WebSockets.IsWebSocketRequest && !string.IsNullOrEmpty(paymentId))
        {
            var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            await paymentController.HandleWebSocketAsync(paymentId, webSocket);
        }
        else
        {
            throw new Exception("Invalid request.");
        }
    }
    catch
    {
        if (!httpContext.Response.HasStarted)
        {
            httpContext.Response.StatusCode = 400;
        }
    }
});

// TODO: Deixar de usar o userId como parâmetro e passar a usar um token
app.Map("/api/groups/ws",
    async (HttpContext httpContext, GroupController groupController, string? socketId) =>
    {
        try
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                groupController.ControllerContext.HttpContext = httpContext;
                await groupController.HandleWebSocketAsync(socketId, webSocket);
            }
            else
            {
                throw new Exception("Invalid request.");
            }
        }
        catch
        {
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = 400;
            }
        }
    });

// TODO: Deixar de usar o userId como parâmetro e passar a usar um token
app.Map("/api/chat/ws",
    async (HttpContext httpContext, ChatController chatController, string? chatId) =>
    {
        try
        {
            if (httpContext.WebSockets.IsWebSocketRequest && !string.IsNullOrEmpty(chatId))
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                chatController.ControllerContext.HttpContext = httpContext;
                await chatController.HandleWebSocketAsync(chatId, webSocket);
            }
            else
            {
                throw new Exception("Invalid request.");
            }
        }
        catch
        {
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = 400;
            }
        }
    });


app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();