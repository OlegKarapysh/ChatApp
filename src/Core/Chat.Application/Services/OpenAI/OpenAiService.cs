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

    public async Task<AssistantObjectResponse> GetAssistantAsync(string assistantId)
    {
        var param = new AssistantRetrieveParameter { Assistant_Id = assistantId };
        return await _client.AssistantRetrieveAsync(param);
    }

    public async Task<AssistantFileObjectResponse> AddFileToAssistant(string assistantId, string fileId)
    {
        var param = new AssistantFileCreateParameter { Assistant_Id = assistantId, File_Id = fileId };
        return await _client.AssistantFileCreateAsync(param);
    }

    public async Task<UploadedFileDto> UploadFileAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0 || file.FileName is null)
        {
            throw new OpenAiApiRequestException("Failed to upload a file! It has bad format or zero length.");
        }

        if (IOpenAiService.AcceptableFileExtensions.All(x => !file.FileName.EndsWith(x, StringComparison.Ordinal)))
        {
            throw new OpenAiApiRequestException("Provided file format is not supported!");
        }
        
        const string purpose = "assistants";
        var fileUploadParameter = new FileUploadParameter { Purpose = purpose };
        await using var fileStream = file.OpenReadStream();
        if (fileStream is null)
        {
            throw new OpenAiApiRequestException("Failed to read the provided file!");
        }
        
        fileUploadParameter.SetFile(file.FileName, fileStream);
        var fileResponse = await _client.FileUploadAsync(fileUploadParameter);
        return fileResponse.MapToUploadedDto();
    }
}