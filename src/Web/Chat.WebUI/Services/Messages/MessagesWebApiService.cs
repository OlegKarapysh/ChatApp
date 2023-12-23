using Chat.Domain.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;
using OpenAI.Threads;

namespace Chat.WebUI.Services.Messages;

public class MessagesWebApiService : WebApiServiceBase, IMessagesWebApiService
{
    private protected override string BaseRoute { get; init; } = "/messages";

    public MessagesWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }
    
    public async Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPageAsync(PagedSearchDto searchData)
    {
        return await GetAsync<MessagesPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }

    public async Task<WebApiResponse<IList<MessageWithSenderDto>>> GetAllConversationMessagesAsync(int conversationId)
    {
        return await GetAsync<IList<MessageWithSenderDto>>($"/all/{conversationId}");
    }

    public async Task<WebApiResponse<MessageWithSenderDto>> SendMessageAsync(MessageDto messageData)
    {
        return await PostAsync<MessageWithSenderDto, MessageDto>(messageData);
    }

    public async Task<WebApiResponse<MessageResponse>> AssistWithMessageAsync(MessageForAssistDto messageDto)
    {
        return await PostAsync<MessageResponse, MessageForAssistDto>(messageDto, "/assist");
    }

    public async Task<WebApiResponse<MessageDto>> UpdateMessageAsync(MessageDto messageData)
    {
        return await PutAsync<MessageDto, MessageDto>(messageData);
    }

    public async Task<ErrorDetailsDto?> DeleteMessageAsync(int messageId)
    {
        return await DeleteAsync($"/{messageId}");
    }
}