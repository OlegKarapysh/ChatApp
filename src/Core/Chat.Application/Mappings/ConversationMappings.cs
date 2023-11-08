using Chat.Domain.DTOs.Conversations;
using Chat.Domain.Entities.Conversations;

namespace Chat.Application.Mappings;

public static class ConversationMappings
{
    public static ConversationDto MapToDto(this Conversation conversation)
    {
        return new ConversationDto
        {
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt
        };
    }

    public static Conversation MapFrom(this Conversation conversation, ConversationDto conversationDto)
    {
        conversation.Title = conversationDto.Title;
        conversation.CreatedAt = conversationDto.CreatedAt;
        conversation.UpdatedAt = conversationDto.UpdatedAt;

        return conversation;
    }
}