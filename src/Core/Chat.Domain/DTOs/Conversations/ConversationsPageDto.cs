namespace Chat.Domain.DTOs.Conversations;

public class ConversationsPageDto : PageDto
{
    public ConversationDto[]? Conversations { get; set; }
}