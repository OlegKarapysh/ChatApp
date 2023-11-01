﻿using Chat.WebUI.HttpHandlers;
using Chat.WebUI.Services;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<JwtAuthInterceptor>();
        services.AddHttpClient<WebApiServiceBase>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["ApiUrl"]!);
        }).AddHttpMessageHandler<JwtAuthInterceptor>();
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthWebApiService, AuthWebApiService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
    }
}