using Microsoft.AspNetCore.Http;
using Chat.Domain.DTOs.AssistantFiles;

namespace Chat.Application.Services.OpenAI;

public interface IOpenAiService
{
    static List<string> AcceptableFileExtensions { get; } = new() { ".txt", ".pdf", ".docx", ".json", ".cs" };
    Task<AssistantObjectResponse> CreateAssistantWithRetrievalAsync(
        string name, string instructions, IEnumerable<string>? fileIds = default);
    Task<UploadedFileDto> UploadFileAsync(IFormFile? file);
}