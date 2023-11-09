using System.Globalization;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.Entities;

namespace Chat.Application.Mappings;

public static class MessageMappings
{
    private const string SqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
    
    public static MessageDto MapToDto(this Message message)
    {
        return new MessageDto
        {
            TextContent = message.TextContent,
            CreatedAt = message.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = message.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }

    public static Message MapFrom(this Message message, MessageDto messageDto)
    {
        message.TextContent = messageDto.TextContent;
        message.CreatedAt = DateTime.ParseExact(messageDto.CreatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);
        message.UpdatedAt = DateTime.ParseExact(messageDto.UpdatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);

        return message;
    }
}