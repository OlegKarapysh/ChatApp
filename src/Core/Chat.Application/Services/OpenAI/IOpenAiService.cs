using Microsoft.AspNetCore.Http;
using OpenAI.Threads;

namespace Chat.Application.Services.OpenAI;

public interface IOpenAiService
{
    static List<string> AcceptableFileExtensions { get; } = new() { ".txt", ".pdf", ".docx", ".json", ".cs" };
    Task<AssistantObjectResponse> CreateAssistantWithRetrievalAsync(
        string name, string instructions, IEnumerable<string>? fileIds = default);
    Task<AssistantObjectResponse> GetAssistantAsync(string assistantId);
    Task<AssistantObjectResponse> EditAssistantAsync(string assistantId, string instructions, string name);
    Task<bool> DeleteAssistantAsync(string assistantId);
    Task<bool> DeleteThreadAsync(string? threadId);
    Task<bool> DeleteFileAsync(string assistantId, string fileId);
    Task<AssistantFileObjectResponse> AddFileToAssistant(string assistantId, string fileId);
    Task<UploadedFileDto> UploadFileAsync(IFormFile? file);
    Task<ThreadObjectResponse> CreateThreadAsync();
    Task<MessageResponse> SendMessageAsync(string message, string assistantId, string threadId);
    Task<string?> GetFunctionCallArgsAsync(string message, string assistantId, string threadId);
}