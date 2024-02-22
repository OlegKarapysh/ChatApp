using System.ComponentModel;
using Chat.Application.Services.Conversations;
using Chat.Application.Services.Messages;

namespace Chat.Application.AiPlugins;

public sealed class MessageSenderPlugin
{
    private readonly IMessageService _messageService;
    private readonly IConversationService _conversationService;
    private readonly IUserService _userService;

    public MessageSenderPlugin(
        IMessageService messageService,
        IConversationService conversationService,
        IUserService userService)
    {
        _messageService = messageService;
        _conversationService = conversationService;
        _userService = userService;
    }

    [KernelFunction]
    [Description("Sends a message to the chat")]
    [return: Description("The message that was sent")]
    public async Task<string> SendMessageAsync(
        Kernel kernel,
        [Description("The message for chat")] string message,
        [Description("The username of the user who sends the message. You can ask user's username if its not mentioned.")]
        string senderUsername,
        [Description("The title of the chat. If this title is not mentioned directly," +
                     " you can form it by using this formula: '{senderUsername} - {receiverUsername} Dialog'")]
        string chatTitle)
    {
        var senderId = (await _userService.GetUserByNameAsync(senderUsername)).Id;
        var conversationId = (await _conversationService.GetConversationByTitleAsync(chatTitle)).Id;
        var messageDto = new MessageDto
        {
            TextContent = message,
            ConversationId = conversationId,
            IsAiAssisted = true,
            SenderId = senderId
        };
        var messageResponse = await _messageService.CreateMessageAsync(messageDto);

        return messageResponse.TextContent;
    }
}