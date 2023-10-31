using Chat.WebUI.HttpHandlers;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddHttpClient(this IServiceCollection services)
    {
        services.AddScoped<DelegatingHandler, JwtAuthInterceptor>();
        services.AddScoped<HttpClient, BlazorHttpClient>();
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthWebApiService, AuthWebApiService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
    }
}