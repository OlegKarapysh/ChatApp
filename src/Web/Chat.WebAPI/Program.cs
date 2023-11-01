using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Chat.Domain.Models;
using Chat.Persistence.Contexts;
using Chat.WebAPI.Extensions;
using Chat.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ChatDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDefaultCors(builder.Configuration);
builder.Services.AddAndConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization(policy =>
{
    policy.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                           .RequireAuthenticatedUser().Build();
});
builder.Services.AddIdentity<User, IdentityRole<int>>()
       .AddEntityFrameworkStores<ChatDbContext>()
       .AddUserManager<UserManager<User>>()
       .AddSignInManager<SignInManager<User>>();
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

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
