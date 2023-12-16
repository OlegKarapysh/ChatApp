using System.ComponentModel.DataAnnotations;
using Chat.Domain.Entities.Groups;

namespace Chat.Domain.DTOs.Groups;

public class NewGroupDto
{
    [MaxLength(Group.MaxNameLength)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(Group.MaxInstructionsLength)]
    public string Instructions { get; set; } = string.Empty;
    public int CreatorId { get; set; }
}