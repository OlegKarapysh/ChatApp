using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Microsoft.AspNetCore.Components;

namespace Chat.WebUI.Services.Auth;

public sealed class JwtAuthService : IJwtAuthService
{
    private readonly IAuthWebApiService _httpService;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public JwtAuthService(IAuthWebApiService httpService, ILocalStorageService localStorage, NavigationManager navigation)
    {
        _httpService = httpService;
        _localStorage = localStorage;
        _navigation = navigation;
    }


    public async Task RegisterAsync(RegistrationDto registerData)
    {
        _navigation.NavigateTo("/");
    }
}