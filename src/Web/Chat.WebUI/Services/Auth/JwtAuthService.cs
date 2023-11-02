using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Authentication;
using Chat.WebUI.Providers;

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
        var response = await _httpService.RegisterAsync(registerData);
        if (!response.IsSuccessful)
        {
            return response.ErrorDetails;
        }
        
        await SaveTokens(response.Content);
        
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

    public async Task<ErrorDetailsDto?> ChangePasswordAsync(ChangePasswordDto changePasswordData)
    {
        return await _httpService.ChangePasswordAsync(changePasswordData);
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemsAsync(new[] { JwtLocalStorageKey, RefreshTokenLocalStorageKey });
        _authenticationState.NotifyAuthenticationChanged();
    }

    private async ValueTask SaveTokens(TokenPairDto? tokens)
    {
        await _localStorage.SetItemAsStringAsync(
            JwtLocalStorageKey, tokens?.AccessToken ?? string.Empty);
        await _localStorage.SetItemAsStringAsync(
            RefreshTokenLocalStorageKey, tokens?.RefreshToken ?? string.Empty);
    }
}