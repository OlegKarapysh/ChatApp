using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Domain.DTOs.AssistantFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Chat.Application.Services.OpenAI;

public sealed class OpenAiService : IOpenAiService
{
    private const string OpenAiApiKeyName = "OPENAI_API_KEY";
    private const string DefaultAiModel = "gpt-4-1106-preview";
    private const string RetrievalToolName = "retrieval";
    
    private readonly OpenAIClient _client;

    public OpenAiService(IConfiguration configuration)
    {
        var apiKey = configuration[OpenAiApiKeyName];
        ArgumentException.ThrowIfNullOrEmpty(apiKey);
        _client = new OpenAIClient(apiKey);
    }
    
    public async Task<AssistantObjectResponse> CreateAssistantWithRetrievalAsync(
        string name, string instructions, IEnumerable<string>? fileIds = default)
    {
        var assistantCreateParameter = new AssistantCreateParameter
        {
            Name = name,
            Instructions = instructions,
            Model = DefaultAiModel,
            File_Ids = fileIds?.ToList(),
            Tools = new List<ToolObject> { new(RetrievalToolName) }
        };

        return await _client.AssistantCreateAsync(assistantCreateParameter);
    }

    public async Task<UploadedFileDto> UploadFileAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            throw new OpenAiApiRequestException("Failed to upload a file! It has bad format or zero length.");
        }
        
        const string purpose = "assistants";
        var fileUploadParameter = new FileUploadParameter { Purpose = purpose };
        await using var fileStream = file.OpenReadStream();
        if (fileStream is null)
        {
            throw new OpenAiApiRequestException("Failed to read the provided file!");
        }
        
        fileUploadParameter.SetFile(file.FileName ?? "Untitled-file", fileStream);
        var fileResponse = await _client.FileUploadAsync(fileUploadParameter);
        return fileResponse.MapToUploadedDto();
    }
}