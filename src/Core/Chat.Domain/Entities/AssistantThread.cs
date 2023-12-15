using System.ComponentModel.DataAnnotations;
using Chat.Domain.Abstract;
using Chat.Domain.Entities.Conversations;

namespace Chat.Domain.Entities;

public class AssistantThread : EntityBase<int>
{
    public const int MaxIdLength = 200;
    
    [MaxLength(MaxIdLength)]
    public string ThreadId { get; set; } = string.Empty;
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
}