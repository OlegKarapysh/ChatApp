using OpenAI.Threads;

namespace Chat.Application.Services.Messages;

public interface IMessageService
{
    Task<MessagesPageDto> SearchMessagesPagedAsync(PagedSearchDto searchData);
    Task<MessageWithSenderDto> CreateMessageAsync(MessageDto messageData);
    Task<IList<MessageWithSenderDto>> GetAllConversationMessagesAsync(int conversationId);
    Task<bool> DeleteMessageAsync(int messageId);
    Task<MessageDto> UpdateMessageAsync(MessageDto messageData, int updaterId);
    Task<MessageResponse> AssistWithMessageAsync(MessageForAssistDto messageDto);
}