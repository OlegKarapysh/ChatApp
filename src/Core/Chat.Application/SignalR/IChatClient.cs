using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatClient
{
    Task ReceiveMessage(MessageWithSenderDto message);
    Task UpdateMessage(MessageWithSenderDto message);
    Task DeleteMessage(MessageDto message);
    Task ReceiveCallRequest(ConversationDto conversation);
    Task ReceiveCallAnswer(ConversationDto conversation);
    Task Join(string signalingChannel);
    Task Leave(string signalingChannel);
    Task SignalWebRtc(string channel, string type, string payload);
    Task ReceiveOffer(string offer);
    Task ReceiveAnswer(string answer);
    Task ReceiveCandidate(string candidate);
}