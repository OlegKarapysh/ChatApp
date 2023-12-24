namespace Chat.WebUI.Services.SignalR;

public interface IChatSignallingService
{
    Task JoinConversationsAsync(string[] conversationIds);
    Task SendMessageAsync(string conversationId, MessageWithSenderDto message);
    Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message);
    Task DeleteMessageAsync(string conversationId, MessageDto message);
}