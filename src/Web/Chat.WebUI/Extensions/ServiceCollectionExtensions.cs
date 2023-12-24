namespace Chat.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DialogService>();
        services.AddScoped<SpinnerService>();
        services.AddScoped<ITokenStorageService, TokenStorageService>();
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
        services.AddScoped<INotifyAuthenticationChanged, JwtAuthenticationStateProvider>();
        services.AddTransient<JwtAuthInterceptor>();
        services.AddHttpClient(JwtAuthInterceptor.HttpClientWithJwtInterceptorName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["ApiUrl"]!);
        }).AddHttpMessageHandler<JwtAuthInterceptor>();
    }

    public static void AddWebApiServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthWebApiService, AuthWebApiService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<IUsersWebApiService, UsersWebApiService>();
        services.AddScoped<IConversationsWebApiService, ConversationsWebApiService>();
        services.AddScoped<IMessagesWebApiService, MessagesWebApiService>();
        services.AddScoped<IOpenAiWebApiService, OpenAiWebApiService>();
        services.AddScoped<IGroupsWebApiService, GroupsWebApiService>();
    }

    public static void AddSignallingServices(this IServiceCollection services)
    {
        services.AddScoped<ISignallingConnectionService, SignallingConnectionService>();
        services.AddScoped<IChatSignallingService, ChatSignallingService>();
        services.AddTransient<WebRtcService>();
    }
}