using ChessApi.ConstringHelpers;
using ChessApi.Hubs;
using ChessApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var origins = builder.Configuration.GetSection("AllowedOrigins").GetChildren().Select(x => x.Value).ToArray();

// services
builder.Services.AddSingleton<UserDBHelper>();
builder.Services.AddSingleton<MatchesDBHelper>();
builder.Services.AddSingleton<MessagesDBHelper>();
builder.Services.AddSingleton<FriendsListDBHelper>();
builder.Services.AddSingleton<ActiveMatchManager>();
builder.Services.AddSingleton<MainChatManager>();
builder.Services.AddControllers();
//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowChessApp",
    builder =>
    {
        builder
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Hubs
builder.Services.AddSignalR();
//builder.Services.AddSingleton<PrivateMessagesHub>();
// Authentication
builder.Services.AddAuthentication("UserAuthentication")
    .AddCookie("UserAuthentication", options =>
    {
        options.Cookie.Name = "UserAuthentication";
        options.AccessDeniedPath = "/api/User/error403";
        options.LoginPath = "/api/User/error401";
        //options.Cookie.SecurePolicy = "None";
    });
//Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeLogged", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "User");
    });
});
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Build
var app = builder.Build();

// HTTP pipeline
app.UseCors("AllowChessApp");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MatchHub>("/api/MatchHub");
    endpoints.MapHub<MessageHub>("/api/MessageHub");
    endpoints.MapHub<PrivateMessagesHub>("/api/PrivateMessagesHub");
});
app.Run();
