using Microsoft.AspNetCore.Components.Authorization;
using BlazorSpinner;
using Chat.WebUI.HttpHandlers;
using Chat.WebUI.Providers;
using Chat.WebUI.Services.Auth;
using Chat.WebUI.Services.Conversations;
using Chat.WebUI.Services.Messages;
using Chat.WebUI.Services.SignalR;
using Chat.WebUI.Services.Users;

namespace Chat.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SpinnerService>();
        services.AddScoped<ITokenStorageService, TokenStorageService>();
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
        services.AddScoped<INotifyAuthenticationChanged, JwtAuthenticationStateProvider>();
        services.AddTransient<JwtAuthInterceptor>();
        services.AddHttpClient(JwtAuthInterceptor.HttpClientWithJwtInterceptorName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["ApiUrl"]!);
        }).AddHttpMessageHandler<JwtAuthInterceptor>();
        services.AddScoped<IAuthWebApiService, AuthWebApiService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<IUsersWebApiService, UsersWebApiService>();
        services.AddScoped<IConversationsWebApiService, ConversationsWebApiService>();
        services.AddScoped<IMessagesWebApiService, MessagesWebApiService>();
        services.AddScoped<IHubConnectionService, HubConnectionService>();
    }
}