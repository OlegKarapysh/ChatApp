namespace Chat.Domain.Entities.Conversations;

public class Conversation : AuditableEntityBase<int>
{
    public const int MaxTitleLength = 100;
    
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = string.Empty;
    public ConversationType Type { get; set; } = ConversationType.Dialog;
    public IList<Message> Messages { get; set; } = new List<Message>();
    public IList<User> Members { get; set; } = new List<User>();
    public IList<ConversationParticipant> ConversationParticipants { get; set; } =
        new List<ConversationParticipant>();
}