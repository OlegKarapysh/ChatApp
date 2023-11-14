using System.Net.Http.Json;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Authentication;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Auth;

public sealed class AuthWebApiService : WebApiServiceBase, IAuthWebApiService
{
    private protected override string BaseRoute { get; init; }

    public AuthWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
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

    public async Task<WebApiResponse<TokenPairDto>> RefreshTokensAsync()
    {
        var expiredTokens = await TokenService.GetTokensAsync();
        var refreshedTokensResponse = await PostAsync<TokenPairDto, TokenPairDto>("/refresh", expiredTokens);
        if (refreshedTokensResponse.IsSuccessful)
        {
            await TokenService.SaveTokensAsync(refreshedTokensResponse.Content);
        }

        return refreshedTokensResponse;
    }

    public async Task<ErrorDetailsDto?> ChangePasswordAsync(ChangePasswordDto changePasswordData)
    {
        const string changePasswordRoute = "/change-password";
        var response = await HttpClient.PostAsJsonAsync(BuildFullRoute(changePasswordRoute), changePasswordData);

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