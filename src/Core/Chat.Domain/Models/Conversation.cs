using Chat.Domain.Abstract;

namespace Chat.Domain.Models;

public class Conversation : AuditableEntityBase<int>
{
    public string Title { get; set; } = string.Empty;
    public IList<Message> Messages { get; set; } = new List<Message>();
}