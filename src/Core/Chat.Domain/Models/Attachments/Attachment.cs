using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;

namespace Chat.Domain.Models.Attachments;

public class Attachment : EntityBase<int>
{
    public const int MaxSourceUrlLength = 4000;
    
    [MaxLength(MaxSourceUrlLength)]
    public string SourceUrl { get; set; } = string.Empty;

    public AttachmentType Type { get; set; } = AttachmentType.File;
    public int MessageId { get; set; }
    public Message Message { get; set; } = default!;
}