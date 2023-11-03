using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;
using Chat.Domain.Models.Attachments;
using Chat.Domain.Models.Conversations;

namespace Chat.Domain.Models;

public class Message : AuditableEntityBase<int>
{
    public const int MaxTextLength = 1000;
    
    [MaxLength(MaxTextLength)]
    public string TextContent { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public int? SenderId { get; set; }
    public int? ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
    public User? Sender { get; set; }
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}