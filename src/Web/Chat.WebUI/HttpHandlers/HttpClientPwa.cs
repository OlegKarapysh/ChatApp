using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Chat.Domain.DTOs.Authentication;
using Chat.WebUI.Providers;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.HttpHandlers;

public class HttpClientPwa : HttpClient
{
    private readonly NavigationManager _navigationManager;
    private readonly ITokenService _tokenService;
    private readonly INotifyAuthenticationChanged _notifyAuthenticationService;

    public HttpClientPwa(
        IConfiguration configuration,
        NavigationManager navigationManager,
        ITokenService tokenService,
        INotifyAuthenticationChanged notifyAuthenticationService)
    {
        _navigationManager = navigationManager;
        _tokenService = tokenService;
        _notifyAuthenticationService = notifyAuthenticationService;
        BaseAddress = new Uri(configuration["ApiUrl"]!);
        Console.WriteLine($"Created: {BaseAddress}");
    }
    
    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Sent request...");
        var response = await base.SendAsync(request, cancellationToken);
        Console.WriteLine($"Received response with status code {response.StatusCode}");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"Received Unauthorized, performing logout...");

            await LogoutAndRedirectToLogin();
            return response;
        }
        

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            Console.WriteLine($"Received Forbidden, performing logout...");

            await LogoutAndRedirectToLogin();
            return response;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized && response.Headers.Contains("Token-Expired"))
        {
            Console.WriteLine($"Received Unauthorized, performing refresh request...");

            var tokenPair = await _tokenService.GetTokens();
            var refreshTokensResult = await this.PostAsJsonAsync("/auth/refresh", tokenPair);
            if (!refreshTokensResult.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to refresh");
                await LogoutAndRedirectToLogin();
                return refreshTokensResult;
            }

            await _tokenService.SaveTokens(await refreshTokensResult.Content.ReadFromJsonAsync<TokenPairDto>());
            _notifyAuthenticationService.NotifyAuthenticationChanged();
            Console.WriteLine($"Successfully refreshed tokens, performing repeated request.");
            return await base.SendAsync(request, cancellationToken);
        }
        
        return response;
    }
    
    private async Task LogoutAndRedirectToLogin()
    {
        await _tokenService.RemoveTokens();
        _notifyAuthenticationService.NotifyAuthenticationChanged();
        _navigationManager.NavigateTo("/login");
    }
}