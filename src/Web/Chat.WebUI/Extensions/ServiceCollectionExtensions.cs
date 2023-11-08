using Chat.WebUI.HttpHandlers;
using Chat.WebUI.Services;
using Chat.WebUI.Services.Auth;
using Chat.WebUI.Services.Conversations;
using Chat.WebUI.Services.Users;

namespace Chat.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configuration["ApiUrl"]!) });
        //  services.AddTransient<JwtAuthInterceptor>();
        //  services.AddHttpClient<HttpClient>(httpClient =>
        //  {
        //      httpClient.BaseAddress = new Uri(configuration["ApiUrl"]!);
        //  });
        // .AddHttpMessageHandler<JwtAuthInterceptor>();
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthWebApiService, AuthWebApiService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<IUsersWebApiService, UsersWebApiService>();
        services.AddScoped<IConversationsWebApiService, ConversationsWebApiService>();
    }
}