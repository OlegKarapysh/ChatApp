using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.WebUI.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    public const string JwtTokenLocalStorageKey = "JwtToken";
    private readonly ILocalStorageService _localStorageService;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwtTokenFromLocalStorage = await _localStorageService.GetItemAsync<string>(JwtTokenLocalStorageKey);
        return string.IsNullOrEmpty(jwtTokenFromLocalStorage)
            ? new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
            : new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimsFromJwt(jwtTokenFromLocalStorage), "JwtAuth")));
    }

    public void NotifyAuthenticationChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = ParseJwtPayload(jwt);
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private string ParseJwtPayload(string jwt)
    {
        const int payloadSectionIndex = 1;
        const string sectionsSeparator = ".";

        return jwt.Split(sectionsSeparator)[payloadSectionIndex];
    }
}