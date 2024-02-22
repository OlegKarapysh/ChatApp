namespace Chat.WebUI.Services.OpenAI;

public sealed class OpenAiWebApiService : WebApiServiceBase, IOpenAiWebApiService
{
    public string FileUploadUrl => BuildFullRoute("/file");
    private protected override string BaseRoute { get; init; } = "/openai";

    public OpenAiWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<UploadedFileDto>> UploadFileAsync(IBrowserFile file)
    {
        var httpContent = new MultipartFormDataContent();
        await using var fileStream = file.OpenReadStream();
        httpContent.Add(new StreamContent(fileStream), "file", file.Name);
        var response = await HttpClient.PostAsync(FileUploadUrl, httpContent);
        return await ParseWebApiResponse<UploadedFileDto>(response);
    }
}