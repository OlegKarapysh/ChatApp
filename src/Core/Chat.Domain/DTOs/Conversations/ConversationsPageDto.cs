namespace Chat.Domain.DTOs.Conversations;

public class ConversationsPageDto : PageDto
{
    public ConversationBasicInfoDto[]? Conversations { get; set; }
}