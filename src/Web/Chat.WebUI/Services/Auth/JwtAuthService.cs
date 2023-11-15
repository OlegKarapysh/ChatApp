using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Authentication;
using Chat.WebUI.Providers;

namespace Chat.WebUI.Services.Auth;

public sealed class JwtAuthService : IJwtAuthService
{
    private readonly IAuthWebApiService _httpService;
    private readonly ITokenStorageService _tokenService;
    private readonly INotifyAuthenticationChanged _authenticationState;

    public JwtAuthService(
        IAuthWebApiService httpService,
        INotifyAuthenticationChanged authenticationState,
        ITokenStorageService tokenService)
    {
        _httpService = httpService;
        _authenticationState = authenticationState;
        _tokenService = tokenService;
    }
    
    public async Task<ErrorDetailsDto?> RegisterAsync(RegistrationDto registerData)
    {
        var response = await _httpService.RegisterAsync(registerData);
        if (!response.IsSuccessful)
        {
            return response.ErrorDetails;
        }
        
        await _tokenService.SaveTokensAsync(response.Content);
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

        await _tokenService.SaveTokensAsync(response.Content);
        
        return default;
    }

    public async Task<ErrorDetailsDto?> ChangePasswordAsync(ChangePasswordDto changePasswordData)
    {
        return await _httpService.ChangePasswordAsync(changePasswordData);
    }

    public async Task LogoutAsync()
    {
        await _tokenService.RemoveTokensAsync();
        _authenticationState.NotifyAuthenticationChanged();
    }
}