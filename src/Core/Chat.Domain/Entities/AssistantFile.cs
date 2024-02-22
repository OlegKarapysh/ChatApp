using Group = Chat.Domain.Entities.Groups.Group;

namespace Chat.Domain.Entities;

public class AssistantFile : AuditableEntityBase<int>
{
    public const int MaxFileNameLength = 100;
    public const int MaxIdLength = 200;
    
    [MaxLength(MaxFileNameLength)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(MaxIdLength)]
    public string FileId { get; set; } = string.Empty;
    public int SizeInBytes { get; set; }
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
}