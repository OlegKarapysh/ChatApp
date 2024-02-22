namespace Chat.WebUI.Services.AiCopilot;

public sealed class AiCopilotWebApiService : WebApiServiceBase, IAiCopilotWebApiService
{
    private protected override string BaseRoute { get; init; } = "/aiCopilot";

    public AiCopilotWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<SimpleMessageDto>> SendMessageToCopilotAsync(SimpleMessageDto message)
    {
        return await PostAsync<SimpleMessageDto, SimpleMessageDto>(message, "/messages");
    }
}