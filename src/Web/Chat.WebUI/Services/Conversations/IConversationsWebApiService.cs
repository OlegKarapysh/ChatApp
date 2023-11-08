using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Conversations;

public interface IConversationsWebApiService
{
    Task<WebApiResponse<ConversationsPageDto>> GetSearchedConversationsPage(PagedSearchDto searchData);
}