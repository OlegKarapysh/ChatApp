using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Conversations;

public interface IConversationsWebApiService
{
    Task<WebApiResponse<ConversationsPageDto>> GetSearchedConversationsPageAsync(PagedSearchDto searchData);
    Task<WebApiResponse<IList<int>>> GetAllUserConversationIdsAsync();
    Task<WebApiResponse<IList<ConversationDto>>> GetAllUserConversationsAsync();
    Task<WebApiResponse<DialogDto>> CreateDialogAsync(NewDialogDto dialogData);
    Task<WebApiResponse<ConversationDto>> CreateGroupChatAsync(NewGroupChatDto groupChatData);
    Task<WebApiResponse<ConversationDto>> AddGroupMemberAsync(NewGroupMemberDto groupMemberData);
    Task<ErrorDetailsDto?> RemoveUserFromConversationAsync(int conversationId);
}