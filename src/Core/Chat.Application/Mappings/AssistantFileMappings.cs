using Chat.Domain.DTOs.AssistantFiles;

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
}