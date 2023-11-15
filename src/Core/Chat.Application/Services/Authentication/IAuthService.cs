using Chat.Domain.DTOs.Authentication;

namespace Chat.Application.Services.Authentication;

public interface IAuthService
{
    Task<TokenPairDto> LoginAsync(LoginDto loginData);
    Task<TokenPairDto> RegisterAsync(RegistrationDto registerData);
    Task ChangePasswordAsync(ChangePasswordDto changePasswordData, int id);
    Task<TokenPairDto> RefreshTokenPairAsync(TokenPairDto tokens);
}