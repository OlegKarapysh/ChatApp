using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OpenAI.Assistants;
using OpenAI.Threads;

namespace Chat.Application.Services.OpenAI;

public sealed class OpenAiService : IOpenAiService
{
    private const string OpenAiApiKeyName = "OPENAI_API_KEY";
    private const string DefaultAiModel = "gpt-4-1106-preview";
    private const string RetrievalToolName = "retrieval";
    private const int PollingIntervalMs = 400;

    private readonly OpenAIClient _clientHigLab;
    private readonly global::OpenAI.OpenAIClient _clientDotNet;

    public OpenAiService(IConfiguration configuration)
    {
        var apiKey = configuration[OpenAiApiKeyName];
        ArgumentException.ThrowIfNullOrEmpty(apiKey);
        _clientHigLab = new OpenAIClient(apiKey);
        _clientDotNet = new global::OpenAI.OpenAIClient(apiKey);
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

        return await _clientHigLab.AssistantCreateAsync(assistantCreateParameter);
    }

    public async Task<AssistantObjectResponse> GetAssistantAsync(string assistantId)
    {
        var parameter = new AssistantRetrieveParameter { Assistant_Id = assistantId };
        return await _clientHigLab.AssistantRetrieveAsync(parameter);
    }

    public async Task<AssistantObjectResponse> EditAssistantAsync(string assistantId, string instructions, string name)
    {
        var assistant = await GetAssistantAsync(assistantId);
        var parameter = new AssistantModifyParameter
        {
            Assistant_Id = assistantId,
            Description = assistant.Description,
            File_Ids = assistant.File_Ids,
            Instructions = instructions,
            Metadata = assistant.MetaData,
            Model = assistant.Model,
            Name = name,
            Tools = assistant.Tools
        };
        return await _clientHigLab.AssistantModifyAsync(parameter);
    }

    public async Task<bool> DeleteAssistantAsync(string assistantId)
    {
        return await _clientDotNet.AssistantsEndpoint!.DeleteAssistantAsync(assistantId)!;
    }

    public async Task<bool> DeleteThreadAsync(string? threadId)
    {
        return !string.IsNullOrWhiteSpace(threadId) && await _clientDotNet.ThreadsEndpoint!.DeleteThreadAsync(threadId)!;
    }

    public async Task<bool> DeleteFileAsync(string assistantId, string fileId)
    {
        var isFileRemovedFromAssistant = await _clientDotNet.AssistantsEndpoint!.RemoveFileAsync(assistantId, fileId)!;
        return isFileRemovedFromAssistant && await _clientDotNet.FilesEndpoint!.DeleteFileAsync(fileId)!;
    }

    public async Task<AssistantFileObjectResponse> AddFileToAssistant(string assistantId, string fileId)
    {
        var parameter = new AssistantFileCreateParameter { Assistant_Id = assistantId, File_Id = fileId };
        return await _clientHigLab.AssistantFileCreateAsync(parameter);
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
        var fileResponse = await _clientHigLab.FileUploadAsync(fileUploadParameter);
        return fileResponse.MapToUploadedDto();
    }

    public async Task<ThreadObjectResponse> CreateThreadAsync()
    {
        return await _clientHigLab.ThreadCreateAsync();
    }

    public async Task<MessageResponse> SendMessageAsync(string message, string assistantId, string threadId)
    {
        var runResponse = await CreateRunAsync(message, assistantId, threadId);
        RunStatus runStatus;
        do
        {
            runStatus = (await _clientDotNet.ThreadsEndpoint!.RetrieveRunAsync(threadId, runResponse.Id)).Status;
            await Task.Delay(TimeSpan.FromMilliseconds(PollingIntervalMs));
        } while (runStatus != RunStatus.Completed);
        
        var responseMessages = await _clientDotNet.ThreadsEndpoint!.ListMessagesAsync(threadId)!;
        return responseMessages.Items[0];
    }

    public async Task<TArgs?> GetFunctionCallArgsAsync<TArgs>(string message, string assistantId)
    {
        var assistant = await _clientDotNet.AssistantsEndpoint!.RetrieveAssistantAsync(assistantId)!;
        var runResponse = await assistant.CreateThreadAndRunAsync(message);
        RunStatus runStatus;
        do
        {
            var run = await _clientDotNet.ThreadsEndpoint!.RetrieveRunAsync(runResponse.ThreadId, runResponse.Id);
            runStatus = run.Status;
            if (runStatus == RunStatus.RequiresAction)
            {
                // var toolCall = run.RequiredAction.SubmitToolOutputs.ToolCalls[0];
                // var args = JsonConvert.DeserializeObject<TArgs>(toolCall.FunctionCall.Arguments);
                // var toolOutput = new ToolOutput(toolCall.Id, string.Empty);
                // var toolOutputRun = await run.SubmitToolOutputsAsync(toolOutput);
                // await toolOutputRun.WaitForStatusChangeAsync();
                // return args;
                await _clientDotNet.ThreadsEndpoint!.DeleteThreadAsync(runResponse.ThreadId);
                var args = run.RequiredAction?.SubmitToolOutputs?.ToolCalls?[0]?.FunctionCall?.Arguments;
                return args is null ? default : JsonConvert.DeserializeObject<TArgs>(args);
            }
            await Task.Delay(TimeSpan.FromMilliseconds(PollingIntervalMs));
        } while (runStatus is not (RunStatus.Completed or RunStatus.Cancelled or RunStatus.Failed));

        return default;
    }

    private async Task<RunResponse> CreateRunAsync(string message, string assistantId, string threadId)
    {
        var createMessageParameter = new CreateMessageRequest(message);
        var messageResponse = await _clientDotNet.ThreadsEndpoint!.CreateMessageAsync(threadId, createMessageParameter)!;
        var createRunParameter = new CreateRunRequest(assistantId);
        var runResponse = await _clientDotNet.ThreadsEndpoint!.CreateRunAsync(threadId, createRunParameter)!;
        if (runResponse is null || messageResponse is null)
        {
            throw new OpenAiApiRequestException("Failed to initialize a run for this message");
        }

        return runResponse;
    }
}