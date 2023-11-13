using System.Globalization;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.Entities.Conversations;

namespace Chat.Application.Mappings;

public static class ConversationMappings
{
    private const string SqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
    
    public static ConversationBasicInfoDto MapToBasicDto(this Conversation conversation)
    {
        return new ConversationBasicInfoDto
        {
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = conversation.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture)
        };
    }

    public static DialogDto MapToDialogDto(this Conversation conversation)
    {
        return new DialogDto
        {
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = conversation.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            Id = conversation.Id
        };
    }
    
    public static ConversationDto MapToDto(this Conversation conversation)
    {
        return new ConversationDto
        {
            Type = conversation.Type,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            UpdatedAt = conversation.UpdatedAt.ToString(SqlDateTimeFormat, CultureInfo.InvariantCulture),
            Id = conversation.Id
        };
    }

    public static Conversation MapFrom(this Conversation conversation, ConversationBasicInfoDto conversationDto)
    {
        conversation.Title = conversationDto.Title;
        conversation.CreatedAt = DateTime.ParseExact(conversationDto.CreatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);
        conversation.UpdatedAt = DateTime.ParseExact(conversationDto.UpdatedAt, SqlDateTimeFormat, CultureInfo.InvariantCulture);

        return conversation;
    }
}