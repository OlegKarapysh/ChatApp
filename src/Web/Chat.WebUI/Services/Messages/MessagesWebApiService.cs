using Blazored.LocalStorage;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Chat.WebUI.Services.Messages;

public class MessagesWebApiService : WebApiServiceBase, IMessagesWebApiService
{
    private protected override string BaseRoute { get; init; }

    public MessagesWebApiService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
    {
        BaseRoute = "/messages";
    }

    
    public async Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPage(PagedSearchDto searchData)
    {
        return await GetAsync<MessagesPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }
}