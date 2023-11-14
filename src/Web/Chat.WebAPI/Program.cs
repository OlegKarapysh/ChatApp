using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Chat.Persistence.Contexts;
using Chat.Domain.Entities;
using Chat.WebAPI.Extensions;
using Chat.WebAPI.Middlewares;
using Chat.WebAPI.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = long.MaxValue;
    options.EnableDetailedErrors = true;
});
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<User, IdentityRole<int>>()
       .AddEntityFrameworkStores<ChatDbContext>()
       .AddUserManager<UserManager<User>>()
       .AddSignInManager<SignInManager<User>>();
builder.Services.AddDefaultCors(builder.Configuration);
builder.Services.AddAndConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<GenericExceptionHandlerMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>(app.Configuration["SignalR:HubRoute"]!);

app.Run();
