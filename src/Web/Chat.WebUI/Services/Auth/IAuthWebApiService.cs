using Chat.Domain.DTOs;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Auth;

public interface IAuthWebApiService
{
    Task<WebApiResponse<TokenPairDto>> LoginAsync(LoginDto loginData);
    Task<WebApiResponse<TokenPairDto>> RegisterAsync(RegistrationDto registerData);
}