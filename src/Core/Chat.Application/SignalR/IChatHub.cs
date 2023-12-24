namespace Chat.Application.SignalR;

public interface IChatHub
{
    Task SendMessage(string conversationId, MessageWithSenderDto message);
    Task UpdateMessage(string conversationId, MessageWithSenderDto message);
    Task DeleteMessage(string conversationId, MessageDto message);
    Task JoinConversations(string[] conversationIds);
    Task Join(string channel);
    Task CallUser(CallDto callData);
    Task AnswerCall(CallDto callData);
    Task SignalWebRtc(WebRtcSignalDto signal);
    Task Leave(string channel);
}