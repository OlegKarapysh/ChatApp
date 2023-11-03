﻿using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Chat.WebUI.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.WebUI.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider, INotifyAuthenticationChanged
{
    private readonly ILocalStorageService _localStorageService;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Console.WriteLine("Gets auth state!!");
        var jwtTokenFromLocalStorage = await _localStorageService.GetItemAsync<string>(JwtAuthService.JwtLocalStorageKey);
        
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
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private string ParseJwtPayload(string jwt)
    {
        const int payloadSectionIndex = 1;
        const string sectionsSeparator = ".";

        return jwt.Split(sectionsSeparator)[payloadSectionIndex];
    }
    
    // Padding is required for proper JWT parsing.
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        
        return Convert.FromBase64String(base64);
    }
}