using Chat.Domain.Abstract;

namespace Chat.Domain.Entities.Conversations;

public sealed class ConversationParticipant : EntityBase<int>
{
    public int? ConversationId { get; set; }
    public int? UserId { get; set; }
    public ConversationMembershipType MembershipType { get; set; }

    public User? User { get; set; }
    public Conversation? Conversation { get; set; }
}