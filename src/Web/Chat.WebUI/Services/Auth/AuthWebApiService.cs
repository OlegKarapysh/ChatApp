namespace Chat.WebUI.Services.Auth;

public sealed class AuthWebApiService : WebApiServiceBase, IAuthWebApiService
{
    private protected override string BaseRoute { get; init; } = "/auth";

    public AuthWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<TokenPairDto>> LoginAsync(LoginDto loginData)
    {
        return await PostAsync<TokenPairDto, LoginDto>(loginData, "/login");
    }
    
    public async Task<WebApiResponse<TokenPairDto>> RegisterAsync(RegistrationDto registerData)
    {
        return await PostAsync<TokenPairDto, RegistrationDto>(registerData, "/register");
    }

    public async Task<WebApiResponse<TokenPairDto>> RefreshTokensAsync()
    {
        var expiredTokens = await TokenService.GetTokensAsync();
        var refreshedTokensResponse = await PostAsync<TokenPairDto, TokenPairDto>(expiredTokens, "/refresh");
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