using System.Globalization;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.Entities.Conversations;

namespace Chat.Application.Mappings;

public static class ConversationMappings
{
    private const string SqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
    
    public static ConversationDto MapToDto(this Conversation conversation)
    {
        return new ConversationDto
        {
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = conversation.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }

    public static Conversation MapFrom(this Conversation conversation, ConversationDto conversationDto)
    {
        conversation.Title = conversationDto.Title;
        conversation.CreatedAt = DateTime.ParseExact(conversationDto.CreatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);
        conversation.UpdatedAt = DateTime.ParseExact(conversationDto.UpdatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);

        return conversation;
    }
}