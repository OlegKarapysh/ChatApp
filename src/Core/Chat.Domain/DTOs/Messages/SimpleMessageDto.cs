namespace Chat.Domain.DTOs.Messages;

public record SimpleMessageDto
{
    public string Text { get; init; } = string.Empty;
    public string Sender { get; init; } = MessageSender.You.ToString();

    public static implicit operator SimpleMessageDto(string body) => new() { Text = body };

    public static implicit operator string(SimpleMessageDto message) => message.Text;
}