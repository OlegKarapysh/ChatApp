using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Messages;

public interface IMessagesWebApiService
{
    Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPage(PagedSearchDto searchData);
    Task<WebApiResponse<MessageDto>> SendMessageAsync(MessageDto messageData);
}