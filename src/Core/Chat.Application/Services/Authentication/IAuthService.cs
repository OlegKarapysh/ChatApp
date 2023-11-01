using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Authentication;

namespace Chat.Application.Services.Authentication;

public interface IAuthService
{
    Task<TokenPairDto> LoginAsync(LoginDto loginData);
    Task<TokenPairDto> RegisterAsync(RegistrationDto registerData);
    Task ChangePasswordAsync(ChangePasswordDto changePasswordData);
}