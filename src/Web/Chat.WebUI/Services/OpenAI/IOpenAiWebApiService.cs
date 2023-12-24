namespace Chat.WebUI.Services.OpenAI;

public interface IOpenAiWebApiService
{
    string FileUploadUrl { get; }
    Task<WebApiResponse<UploadedFileDto>> UploadFileAsync(IBrowserFile file);
}