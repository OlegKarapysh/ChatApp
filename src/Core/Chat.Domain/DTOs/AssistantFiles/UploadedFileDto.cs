namespace Chat.Domain.DTOs.AssistantFiles;

public class UploadedFileDto
{
    public string FileId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SizeInBytes { get; set; }
}