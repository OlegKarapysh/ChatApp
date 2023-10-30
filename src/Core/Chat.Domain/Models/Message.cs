using Chat.Domain.Abstract;

namespace Chat.Domain.Models;

public class Message : AuditableEntityBase<int>
{
    public string Content { get; set; } = string.Empty;
    public Conversation Conversation { get; set; } = default!;
    public User Sender { get; set; } = default!;
}