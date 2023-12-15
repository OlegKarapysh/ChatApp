using Chat.Domain.Abstract;

namespace Chat.Domain.Entities.Groups;

public class GroupMember : EntityBase<int>
{
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public User User { get; set; } = default!;
    public Group Group { get; set; } = default!;
}