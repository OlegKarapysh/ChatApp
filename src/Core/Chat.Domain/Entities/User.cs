using Group = Chat.Domain.Entities.Groups.Group;

namespace Chat.Domain.Entities;

public class User : IdentityUser<int>, ICreatableEntity, IEntity<int>
{
    public const int MaxNameLength = 100;
    
    [MaxLength(MaxNameLength)]
    public string? FirstName { get; set; }
    [MaxLength(MaxNameLength)]
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime TokenExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IList<Message> Messages { get; set; } = new List<Message>();
    public IList<Conversation> Conversations { get; set; } = new List<Conversation>();
    public IList<ConversationParticipant> ConversationParticipants { get; set; } =
        new List<ConversationParticipant>();
    public IList<Group> OwnGroups { get; set; } = new List<Group>();
    public IList<Group> ForeignGroups { get; set; } = new List<Group>();
    public IList<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
}