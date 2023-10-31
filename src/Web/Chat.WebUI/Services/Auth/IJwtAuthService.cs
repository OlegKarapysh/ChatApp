using Chat.Domain.DTOs;

namespace Chat.WebUI.Services.Auth;

public interface IJwtAuthService
{
    Task RegisterAsync(RegistrationDto registerData);
}