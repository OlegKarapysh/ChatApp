using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Messages;

public interface IMessageService
{
    Task<MessagesPageDto> SearchMessagesPagedAsync(PagedSearchDto searchData);
    Task<MessageDto> CreateMessageAsync(MessageDto messageData);
    Task<IList<MessageDto>> GetAllConversationMessagesAsync(int conversationId);
}