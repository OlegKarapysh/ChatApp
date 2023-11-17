using System.Globalization;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.Entities;

namespace Chat.Application.Mappings;

public static class MessageMappings
{
    private const string SqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
    
    public static MessageBasicInfoDto MapToBasicDto(this Message message)
    {
        return new MessageBasicInfoDto
        {
            TextContent = message.TextContent,
            CreatedAt = message.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = message.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }
    
    public static MessageDto MapToDto(this Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            IsRead = message.IsRead,
            ConversationId = message.ConversationId ?? default,
            SenderId = message.SenderId ?? default,
            TextContent = message.TextContent,
            CreatedAt = message.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = message.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }
    
    public static MessageWithSenderDto MapToDtoWithSender(this Message message)
    {
        return new MessageWithSenderDto
        {
            Id = message.Id,
            IsRead = message.IsRead,
            ConversationId = message.ConversationId ?? default,
            SenderId = message.SenderId ?? default,
            UserName = message.Sender?.UserName ?? string.Empty,
            TextContent = message.TextContent,
            CreatedAt = message.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = message.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }

    public static Message MapFrom(this Message message, MessageBasicInfoDto messageDto)
    {
        message.TextContent = messageDto.TextContent;

        return message;
    }
    
    public static Message MapFrom(this Message message, MessageDto messageDto)
    {
        message.IsRead = messageDto.IsRead;
        message.Id = messageDto.Id;
        message.SenderId = messageDto.SenderId;
        message.ConversationId = messageDto.ConversationId;
        message.TextContent = messageDto.TextContent;
        
        return message;
    }

    public static MessageDto MapToDo(this MessageWithSenderDto message)
    {
        return new MessageDto
        {
            Id = message.Id,
            TextContent = message.TextContent,
            IsRead = message.IsRead,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
    }
}