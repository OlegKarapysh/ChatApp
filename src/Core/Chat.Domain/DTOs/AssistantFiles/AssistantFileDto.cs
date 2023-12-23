using System.ComponentModel.DataAnnotations;
using Chat.Domain.Entities;

namespace Chat.Domain.DTOs.AssistantFiles;

public class AssistantFileDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "File name is required!")]
    [MaxLength(AssistantFile.MaxFileNameLength)]
    public string Name { get; set; } = string.Empty;
    public int SizeInBytes { get; set; }
    public DateTime CreatedAt { get; set; }
}