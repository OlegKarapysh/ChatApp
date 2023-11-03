using System.Net.Http.Json;
using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Authentication;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Auth;

public sealed class AuthWebApiService : WebApiServiceBase, IAuthWebApiService
{
    public AuthWebApiService(HttpClient httpClient, ILocalStorageService localStorage)
        : base(httpClient, localStorage)
    {
        BaseRoute = "/auth";
    }

    public async Task<WebApiResponse<TokenPairDto>> LoginAsync(LoginDto loginData)
    {
        const string loginRoute = "/login";

        return await PostAsync<TokenPairDto, LoginDto>(loginRoute, loginData);
    }
    
    public async Task<WebApiResponse<TokenPairDto>> RegisterAsync(RegistrationDto registerData)
    {
        const string registerRoute = "/register";

        return await PostAsync<TokenPairDto, RegistrationDto>(registerRoute, registerData);
    }

    public async Task<ErrorDetailsDto?> ChangePasswordAsync(ChangePasswordDto changePasswordData)
    {
        await TryAddAuthorization();
        const string changePasswordRoute = "/change-password";
        var response = await HttpClient.PostAsJsonAsync($"{FullRoute}{changePasswordRoute}", changePasswordData);

        try
        {
            return await response.Content.ReadFromJsonAsync<ErrorDetailsDto>();
        }
        catch
        {
            return null;
        }
    }
}