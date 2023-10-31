using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.WebUI.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.WebUI.Services.Auth;

public sealed class JwtAuthService : IJwtAuthService
{
    public const string JwtLocalStorageKey = "JwtToken";
    public const string RefreshTokenLocalStorageKey = "RefreshToken";

    private readonly IAuthWebApiService _httpService;
    private readonly ILocalStorageService _localStorage;
    private readonly INotifyAuthenticationChanged _authenticationState;

    public JwtAuthService(
        IAuthWebApiService httpService,
        ILocalStorageService localStorage,
        INotifyAuthenticationChanged authenticationState)
    {
        _httpService = httpService;
        _localStorage = localStorage;
        _authenticationState = authenticationState;
    }


    public async Task<ErrorDetailsDto?> RegisterAsync(RegistrationDto registerData)
    {
        // var response = await _httpService.RegisterAsync(registerData);
        // if (!response.IsSuccessful)
        // {
        //     return response.ErrorDetails;
        // }
        //
        // await SaveTokens(response.Content);
        
        // TODO: test
        await _localStorage.SetItemAsStringAsync(JwtLocalStorageKey,
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
        _authenticationState.NotifyAuthenticationChanged();
        
        return default;
    }
    
    public async Task<ErrorDetailsDto?> LoginAsync(LoginDto loginData)
    {
        var response = await _httpService.LoginAsync(loginData);
        if (!response.IsSuccessful)
        {
            return response.ErrorDetails;
        }

        await SaveTokens(response.Content);
        
        return default;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemsAsync(new[] { JwtLocalStorageKey, RefreshTokenLocalStorageKey });
    }

    private async ValueTask SaveTokens(TokenPairDto? tokens)
    {
        await _localStorage.SetItemAsStringAsync(
            JwtLocalStorageKey, tokens?.AccessToken ?? string.Empty);
        await _localStorage.SetItemAsStringAsync(
            RefreshTokenLocalStorageKey, tokens?.RefreshToken ?? string.Empty);
    }
}