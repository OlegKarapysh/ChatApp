using Chat.Domain.DTOs.Authentication;

namespace Chat.WebUI.Services.Auth;

public interface ITokenService
{
    ValueTask SaveTokens(TokenPairDto? tokens);
    ValueTask RemoveTokens();
    Task<TokenPairDto> GetTokens();
}