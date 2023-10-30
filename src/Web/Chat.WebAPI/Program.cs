using System.Text;
using Chat.Domain.Models;
using Chat.Persistence.Contexts;
using Chat.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ChatDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(b => b.AddDefaultPolicy(policyBuilder =>
{
    policyBuilder.AllowAnyHeader()
                 .AllowAnyMethod()
    // .AllowCredentials()
    // TODO: Allow only web UI origin.
                 .AllowAnyOrigin();
}));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.ClaimsIssuer = builder.Configuration["Jwt:Issuer"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        RequireExpirationTime = false,
        
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var tokenExpiredHeader = "Token-Expired";
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Add(tokenExpiredHeader, "true");
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(policy =>
{
    policy.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                           .RequireAuthenticatedUser().Build();
});
builder.Services.AddIdentity<User, IdentityRole<int>>()
       .AddEntityFrameworkStores<ChatDbContext>()
       .AddUserManager<UserManager<User>>() // TODO: Add custom UserManager.
       .AddSignInManager();

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
