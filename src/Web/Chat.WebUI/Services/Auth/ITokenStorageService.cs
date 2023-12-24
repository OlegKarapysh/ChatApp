namespace Chat.WebUI.Services.Auth;

public interface ITokenStorageService
{
    ValueTask SaveTokensAsync(TokenPairDto? tokens);
    ValueTask RemoveTokensAsync();
    Task<TokenPairDto> GetTokensAsync();
}