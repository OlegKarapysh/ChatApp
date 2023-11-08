using Blazored.LocalStorage;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Chat.WebUI.Services.Conversations;

public class ConversationsWebApiService : WebApiServiceBase, IConversationsWebApiService
{
    private protected override string BaseRoute { get; init; }

    
    public ConversationsWebApiService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
    {
        BaseRoute = "/conversations";
    }
    
    public async Task<WebApiResponse<ConversationsPageDto>> GetSearchedConversationsPage(PagedSearchDto searchData)
    {
        return await GetAsync<ConversationsPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }
}