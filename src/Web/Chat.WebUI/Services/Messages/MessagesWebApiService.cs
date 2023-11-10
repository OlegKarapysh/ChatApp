﻿using Microsoft.AspNetCore.WebUtilities;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Messages;

public class MessagesWebApiService : WebApiServiceBase, IMessagesWebApiService
{
    private protected override string BaseRoute { get; init; }

    public MessagesWebApiService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        : base(httpClientFactory, tokenService)
    {
        BaseRoute = "/messages";
        Console.WriteLine("Messages web api service with httpClient type: " + httpClientFactory.GetType().FullName);
    }

    
    public async Task<WebApiResponse<MessagesPageDto>> GetSearchedMessagesPage(PagedSearchDto searchData)
    {
        return await GetAsync<MessagesPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }
}