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
        services.AddScoped<IOpenAiWebApiService, OpenAiWebApiService>();
        services.AddScoped<IHubConnectionService, HubConnectionService>();
        services.AddScoped<IGroupsWebApiService, GroupsWebApiService>();
        services.AddTransient<WebRtcService>();
    }
}