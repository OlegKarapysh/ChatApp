using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Chat.WebUI.Services.Users;

public sealed class UsersWebApiService : WebApiServiceBase, IUsersWebApiService
{
    private protected override string BaseRoute { get; init; }

    public UsersWebApiService(HttpClient httpClient, ILocalStorageService localStorage)
        : base(httpClient, localStorage)
    {
        BaseRoute = "/users";
    }

    public async Task<WebApiResponse<IList<UserDto>>> GetAllUsers() => await GetAsync<IList<UserDto>>("/all");

    public async Task<WebApiResponse<UserDto>> GetCurrentUserInfoAsync() => await GetAsync<UserDto>();

    public async Task<WebApiResponse<UsersPageDto>> GetSearchedUsersPage(PagedSearchDto searchData)
    {
        return await GetAsync<UsersPageDto>(
            QueryHelpers.AddQueryString("/search/", GetQueryParamsForPagedSearch(searchData)));
    }

    public async Task<ErrorDetailsDto?> UpdateUserInfoAsync(UserDto userData) => await PutAsync(userData);
}