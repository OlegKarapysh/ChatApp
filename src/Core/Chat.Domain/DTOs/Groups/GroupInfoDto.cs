using Group = Chat.Domain.Entities.Groups.Group;

namespace Chat.Domain.DTOs.Groups;

public class GroupInfoDto
{
    public int Id { get; set; }
    [MaxLength(Group.MaxNameLength)]
    public string Name { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;
    public int MembersCount { get; set; }
    public int FilesCount { get; set; }
}