using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.Entities;

namespace Chat.Application.Mappings;

public static class AssistantFileMappings
{
    public static UploadedFileDto MapToUploadedDto(this FileObjectResponse file)
    {
        return new UploadedFileDto
        {
            FileId = file.Id,
            Name = file.FileName,
            SizeInBytes = file.Bytes
        };
    }

    public static AssistantFileDto MapToDto(this AssistantFile file)
    {
        return new AssistantFileDto
        {
            Id = file.Id,
            Name = file.Name,
            SizeInBytes = file.SizeInBytes,
            CreatedAt = file.CreatedAt
        };
    }

    public static AssistantFile MapToFile(this UploadedFileDto uploadedFileDto)
    {
        return new AssistantFile
        {
            FileId = uploadedFileDto.FileId,
            Name = uploadedFileDto.Name,
            SizeInBytes = uploadedFileDto.SizeInBytes
        };
    }
}