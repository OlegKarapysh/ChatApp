namespace Chat.IntegrationTests;

public abstract class IntegrationTestBase
{
    private protected readonly HttpClient HttpClient;
    private protected readonly RegistrationDto RegisteredUser = new()
    {
        UserName = "DefaultUser",
        Email = "email@gmail.com",
        Password = "somethingA1!",
        RepeatPassword = "somethingA1!"
    };

    private protected IntegrationTestBase()
    {
        var appFactory = new TestApplicationFactory();
        HttpClient = appFactory.CreateClient();
    }

    private protected async Task AuthenticateAsync()
    {
        const string registerPath = "api/auth/register";
        var registrationResponse = await HttpClient.PostAsJsonAsync(registerPath, RegisteredUser);
        if (!registrationResponse.IsSuccessStatusCode)
        {
            throw new BadRegistrationException();
        }

        var tokens = await registrationResponse.Content.ReadFromJsonAsync<TokenPairDto>();
        if (tokens is null)
        {
            throw new Exception("Couldn't get authentication tokens!");
        }
        
        var authHeader = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, tokens.AccessToken);
        HttpClient.DefaultRequestHeaders.Authorization = authHeader;
    }
}