using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Messages;

public interface IMessagesWebApiService
{
    Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPageAsync(PagedSearchDto searchData);
    Task<WebApiResponse<IList<MessageWithSenderDto>>> GetAllConversationMessagesAsync(int conversationId);
    Task<WebApiResponse<MessageWithSenderDto>> SendMessageAsync(MessageDto messageData);
    Task<WebApiResponse<MessageDto>> UpdateMessageAsync(MessageDto messageData);
    Task<ErrorDetailsDto?> DeleteMessageAsync(int messageId);
}