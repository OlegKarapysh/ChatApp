namespace Chat.WebUI.Services.SignalR;

public sealed class ChatSignallingService : SignallingServiceBase, IChatSignallingService
{
    public ChatSignallingService(ISignallingConnectionService connectionService) : base(connectionService)
    {
    }
    
    public async Task JoinConversationsAsync(string[] conversationIds)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(
            nameof(IChatHub.JoinConversations), conversationIds));
    }

    public async Task SendMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(
            nameof(IChatHub.SendMessage), conversationId, message));
    }

    public async Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(
            nameof(IChatHub.UpdateMessage), conversationId, message));
    }

    public async Task DeleteMessageAsync(string conversationId, MessageDto message)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(
            nameof(IChatHub.DeleteMessage), conversationId, message));
    }

    public async Task CallUserAsync(CallDto call)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(nameof(IChatHub.CallUser), call));
    }

    public async Task AnswerCallAsync(CallDto call)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(nameof(IChatHub.AnswerCall), call));
    }
}