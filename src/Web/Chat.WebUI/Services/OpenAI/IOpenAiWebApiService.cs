using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.Web;
using Microsoft.AspNetCore.Components.Forms;

namespace Chat.WebUI.Services.OpenAI;

public interface IOpenAiWebApiService
{
    string FileUploadUrl { get; }
    Task<WebApiResponse<UploadedFileDto>> UploadFileAsync(IBrowserFile file);
}