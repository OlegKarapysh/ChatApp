using Chat.Domain.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Conversations;

public class ConversationsWebApiService : WebApiServiceBase, IConversationsWebApiService
{
    private protected override string BaseRoute { get; init; } = "/conversations";
    
    public ConversationsWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }
    
    public async Task<WebApiResponse<ConversationsPageDto>> GetSearchedConversationsPageAsync(PagedSearchDto searchData)
    {
        return await GetAsync<ConversationsPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }

    public async Task<WebApiResponse<IList<int>>> GetAllUserConversationIdsAsync()
    {
        return await GetAsync<IList<int>>("/all-ids");
    }

    public async Task<WebApiResponse<IList<ConversationDto>>> GetAllUserConversationsAsync()
    {
        return await GetAsync<IList<ConversationDto>>("/all");
    }

    public async Task<WebApiResponse<DialogDto>> CreateDialogAsync(NewDialogDto dialogData)
    {
        return await PostAsync<DialogDto, NewDialogDto>(dialogData, "/dialogs");
    }
    
    public async Task<WebApiResponse<ConversationDto>> CreateGroupChatAsync(NewGroupChatDto groupChatData)
    {
        return await PostAsync<ConversationDto, NewGroupChatDto>(groupChatData, "/groups");
    }

    public async Task<WebApiResponse<ConversationDto>> AddGroupMemberAsync(NewGroupMemberDto groupMemberData)
    {
        return await PostAsync<ConversationDto, NewGroupMemberDto>(groupMemberData, "/members");
    }

    public async Task<ErrorDetailsDto?> RemoveUserFromConversationAsync(int conversationId)
    {
        return await DeleteAsync($"/{conversationId}");
    }
}