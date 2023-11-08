using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Conversations;

public interface IConversationService
{
    Task<ConversationsPageDto> SearchUsersPagedAsync(PagedSearchDto searchData);
}