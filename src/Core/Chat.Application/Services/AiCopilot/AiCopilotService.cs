using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAIClient = Azure.AI.OpenAI.OpenAIClient;

namespace Chat.Application.Services.AiCopilot;

public class AiCopilotService : IAiCopilot
{
    private const string OpenAiApiKeyName = "OPENAI_API_KEY_REENBIT";
    private const string DefaultAiModel = "gpt-4";
    
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly OpenAIPromptExecutionSettings _promptExecutionSettings;
    private readonly ChatHistory _chatHistory;

    public AiCopilotService(IConfiguration configuration)
    {
        var apiKey = configuration[OpenAiApiKeyName];
        ArgumentException.ThrowIfNullOrEmpty(apiKey);
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(DefaultAiModel, new OpenAIClient(apiKey));
        kernelBuilder.Plugins.AddFromType<MessageSenderPlugin>();
        _kernel = kernelBuilder.Build();
        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        _promptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };
        _chatHistory = new ChatHistory(
            """
               You are a friendly assistant who likes to follow the rules. You will complete required steps
               and request approval before taking any consequential actions. If the user doesn't provide
               enough information for you to complete a task, you will keep asking questions until you have
               enough information to complete the task.
            """);
    }

    public async Task<string> SendMessageToChatAsync(string message)
    {
        _chatHistory.AddUserMessage(message);
        var response = await _chatCompletionService.GetChatMessageContentsAsync(
            _chatHistory,
            executionSettings: _promptExecutionSettings,
            kernel: _kernel);

        return response.FirstOrDefault()?.Content ?? string.Empty;
    }
}