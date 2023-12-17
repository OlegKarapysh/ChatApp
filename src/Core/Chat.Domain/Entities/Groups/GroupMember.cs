using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;

namespace Chat.Domain.Entities.Groups;

public class GroupMember : EntityBase<int>
{
    public const int MaxIdLength = 200;
    public int? UserId { get; set; }
    public int? GroupId { get; set; }
    [MaxLength(MaxIdLength)]
    public string? ThreadId { get; set; }
    public User? User { get; set; }
    public Group? Group { get; set; }
}