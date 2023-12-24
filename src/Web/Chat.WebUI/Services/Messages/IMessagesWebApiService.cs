namespace Chat.WebUI.Services.Messages;

public interface IMessagesWebApiService
{
    Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPageAsync(PagedSearchDto searchData);
    Task<WebApiResponse<IList<MessageWithSenderDto>>> GetAllConversationMessagesAsync(int conversationId);
    Task<WebApiResponse<MessageWithSenderDto>> SendMessageAsync(MessageDto messageData);
    Task<WebApiResponse<MessageResponse>> AssistWithMessageAsync(MessageForAssistDto messageDto);
    Task<WebApiResponse<MessageDto>> UpdateMessageAsync(MessageDto messageData);
    Task<ErrorDetailsDto?> DeleteMessageAsync(int messageId);
}