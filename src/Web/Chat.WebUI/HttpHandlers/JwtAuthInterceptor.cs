using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Chat.Domain.DTOs.Authentication;
using Chat.WebUI.Pages;
using Chat.WebUI.Providers;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.HttpHandlers;

public sealed class JwtAuthInterceptor : DelegatingHandler
{
    public const string HttpClientWithJwtInterceptorName = "HttpClientJwt";
    public const string BearerAuthScheme = "Bearer";
    private readonly string _apiUrl;
    private readonly NavigationManager _navigationManager;
    private readonly ITokenStorageService _tokenService;
    private readonly INotifyAuthenticationChanged _notifyAuthenticationService;

    public JwtAuthInterceptor(
        IConfiguration configuration,
        ITokenStorageService localStorage,
        NavigationManager navigationManager,
        INotifyAuthenticationChanged notifyAuthenticationService)
    {
        _tokenService = localStorage;
        _navigationManager = navigationManager;
        _notifyAuthenticationService = notifyAuthenticationService;
        _apiUrl = configuration["ApiUrl"]!;
    }
    
     protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwt = (await _tokenService.GetTokensAsync()).AccessToken;
        TryAddBearerHeader(request, jwt);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            await LogoutAndRedirectToLogin();
            return response;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized && response.Headers.Contains("Token-Expired"))
        {
            var tokenPair = await _tokenService.GetTokensAsync();
            var tokenPairJson = JsonSerializer.Serialize(tokenPair);
            var jsonContent = new StringContent(tokenPairJson, System.Text.Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl}/auth/refresh") { Content = jsonContent };
            var refreshTokensResult = await base.SendAsync(httpRequestMessage, cancellationToken);
            if (!refreshTokensResult.IsSuccessStatusCode)
            {
                await LogoutAndRedirectToLogin();
                return refreshTokensResult;
            }

            var tokens = await refreshTokensResult.Content.ReadFromJsonAsync<TokenPairDto>(cancellationToken: cancellationToken);
            await _tokenService.SaveTokensAsync(tokens);
            _notifyAuthenticationService.NotifyAuthenticationChanged();
            TryAddBearerHeader(request, tokens?.AccessToken ?? string.Empty);
            return await base.SendAsync(request, cancellationToken);
        }
        
        return response;
    }

    private void TryAddBearerHeader(HttpRequestMessage httpRequestMessage, string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }
        
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(BearerAuthScheme, token);
    }

    private async Task LogoutAndRedirectToLogin()
    {
        await _tokenService.RemoveTokensAsync();
        _notifyAuthenticationService.NotifyAuthenticationChanged();
        _navigationManager.NavigateTo(LoginPage.Path, true);
    }
}