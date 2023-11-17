using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatClient
{
    Task ReceiveMessage(MessageWithSenderDto message);
    Task UpdateMessage(MessageWithSenderDto message);
    Task DeleteMessage(MessageDto message);
    Task Join(string signalingChannel);
    Task Leave(string signalingChannel);
    Task SignalWebRtc(string channel, string type, string payload);
    Task ReceiveOffer(string offer);
    Task ReceiveAnswer(string answer);
    Task ReceiveCandidate(string candidate);
}