using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;

namespace Chat.Domain.Models.Conversations;

public class Conversation : AuditableEntityBase<int>
{
    public const int MaxTitleLength = 100;
    
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = string.Empty;

    public ConversationType Type { get; set; } = ConversationType.Dialog;
    public IList<Message> Messages { get; set; } = new List<Message>();
}