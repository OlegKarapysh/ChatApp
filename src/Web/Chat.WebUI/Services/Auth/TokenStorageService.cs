using Blazored.LocalStorage;
using Chat.Domain.DTOs.Authentication;

namespace Chat.WebUI.Services.Auth;

public sealed class TokenStorageService : ITokenStorageService
{
    public const string JwtLocalStorageKey = "JwtToken";
    public const string RefreshTokenLocalStorageKey = "RefreshToken";
    private readonly ILocalStorageService _localStorage;

    public TokenStorageService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async ValueTask SaveTokensAsync(TokenPairDto? tokens)
    {
        await _localStorage.SetItemAsStringAsync(
            JwtLocalStorageKey, tokens?.AccessToken ?? string.Empty);
        await _localStorage.SetItemAsStringAsync(
            RefreshTokenLocalStorageKey, tokens?.RefreshToken ?? string.Empty);
    }

    public async ValueTask RemoveTokensAsync()
    {
        await _localStorage.RemoveItemsAsync(new[] { JwtLocalStorageKey, RefreshTokenLocalStorageKey });
    }

    public async Task<TokenPairDto> GetTokensAsync()
    {
        return new TokenPairDto
        {
            AccessToken = await _localStorage.GetItemAsStringAsync(JwtLocalStorageKey),
            RefreshToken = await _localStorage.GetItemAsStringAsync(RefreshTokenLocalStorageKey)
        };
    }
}