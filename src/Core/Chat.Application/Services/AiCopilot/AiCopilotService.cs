using Chat.Application.Services.Conversations;
using Chat.Application.Services.Messages;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAIClient = Azure.AI.OpenAI.OpenAIClient;

namespace Chat.Application.Services.AiCopilot;

public sealed class AiCopilotService : IAiCopilotService
{
    private const string OpenAiApiKeyName = "OPENAI_API_KEY_REENBIT";
    private const string DefaultAiModel = "gpt-3.5-turbo";
    
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly OpenAIPromptExecutionSettings _promptExecutionSettings;
    private readonly ChatHistory _chatHistory;

    public AiCopilotService(
        IConfiguration configuration,
        IMessageService messageService,
        IConversationService conversationService,
        IUserService userService)
    {
        var apiKey = configuration[OpenAiApiKeyName];
        ArgumentException.ThrowIfNullOrEmpty(apiKey);
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(DefaultAiModel, new OpenAIClient(apiKey));
        kernelBuilder.Plugins.AddFromObject(new MessageSenderPlugin(messageService, conversationService, userService));
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

    public async Task<SimpleMessageDto> SendMessageToChatAsync(SimpleMessageDto message)
    {
        _chatHistory.AddUserMessage(message);
        var response = await _chatCompletionService.GetChatMessageContentAsync(
            _chatHistory,
            executionSettings: _promptExecutionSettings,
            kernel: _kernel);
        _chatHistory.AddMessage(response.Role, response.Content ?? string.Empty);

        return new SimpleMessageDto
        {
            Text = response.Content ?? string.Empty, Sender = MessageSender.Copilot.ToString()
        };
    }
}