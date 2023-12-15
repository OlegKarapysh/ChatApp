using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;

namespace Chat.Domain.Entities.Groups;

public class Group : EntityBase<int>
{
    public const int MaxNameLength = 100;
    public const int MaxIdLength = 200;
    
    [MaxLength(MaxNameLength)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(MaxIdLength)]
    public string AssistantId { get; set; } = string.Empty;
    public int CreatorId { get; set; }
    public User? Creator { get; set; }
    public IList<User> Members { get; set; } = new List<User>();
    public IList<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public IList<AssistantFile> Files { get; set; } = new List<AssistantFile>();
}