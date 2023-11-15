namespace Chat.Domain.DTOs.Conversations;

public class NewDialogDto
{
    public int CreatorId { get; set; }
    public string InterlocutorUserName { get; set; } = string.Empty;
}