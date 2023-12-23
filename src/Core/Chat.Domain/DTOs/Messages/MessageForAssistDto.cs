namespace Chat.Domain.DTOs.Messages;

public class MessageForAssistDto
{
    public int MessageId { get; set; }
    public string ReceiverUserName { get; set; } = string.Empty;
}