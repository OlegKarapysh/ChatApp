namespace Chat.Domain.DTOs.Users;

public class UserMessageSenderDto
{
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}