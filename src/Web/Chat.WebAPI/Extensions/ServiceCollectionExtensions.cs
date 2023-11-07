using System.Text;
using Chat.Application.JWT;
using Chat.Application.Services;
using Chat.Application.Services.Authentication;
using Chat.Application.Services.JWT;
using Chat.Application.Services.Users;
using Chat.DomainServices.UnitsOfWork;
using Chat.Persistence.UnitsOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Chat.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(b => b.AddDefaultPolicy(policyBuilder =>
        {
            policyBuilder.WithOrigins(configuration["Cors:AllowedOrigin"]!)
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials()
                         .WithExposedHeaders(configuration["Cors:TokenExpiredHeader"]!);
        }));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
    }
    
    public static void AddAndConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions));
        var secretKey = jwtOptions[nameof(JwtOptions.SecretKey)]!;
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtOptions[nameof(JwtOptions.Issuer)]!;
            options.Audience = jwtOptions[nameof(JwtOptions.Audience)]!;
            options.SecretKey = secretKey;
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        });
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.ClaimsIssuer = jwtOptions[nameof(JwtOptions.Issuer)];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],
        
                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],
        
                ValidateLifetime = true,
                RequireExpirationTime = false,
        
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers.Add(configuration["Cors:TokenExpiredHeader"]!, "true");
                    }
        
                    return Task.CompletedTask;
                }
            };
        });
    }
}