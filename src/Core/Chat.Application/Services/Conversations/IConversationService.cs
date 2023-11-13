using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Conversations;

public interface IConversationService
{
    Task<ConversationsPageDto> SearchConversationsPagedAsync(PagedSearchDto searchData);
    Task<IList<int>> GetUserConversationIdsAsync(int userId);
    Task<DialogDto> CreateOrGetDialogAsync(NewDialogDto newDialogData);
    Task<IList<ConversationDto>> GetAllUserConversationsAsync(int userId);
}