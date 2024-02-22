namespace Chat.WebUI.Services.Auth;

public interface IAuthWebApiService
{
    Task<WebApiResponse<TokenPairDto>> LoginAsync(LoginDto loginData);
    Task<WebApiResponse<TokenPairDto>> RegisterAsync(RegistrationDto registerData);
    Task<WebApiResponse<TokenPairDto>> RefreshTokensAsync();
    Task<ErrorDetailsDto?> ChangePasswordAsync(ChangePasswordDto changePasswordData);
}