using Microsoft.AspNetCore.WebUtilities;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Conversations;

public class ConversationsWebApiService : WebApiServiceBase, IConversationsWebApiService
{
    private protected override string BaseRoute { get; init; }
    
    public ConversationsWebApiService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        : base(httpClientFactory, tokenService)
    {
        BaseRoute = "/conversations";
    }
    
    public async Task<WebApiResponse<ConversationsPageDto>> GetSearchedConversationsPage(PagedSearchDto searchData)
    {
        return await GetAsync<ConversationsPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }
}