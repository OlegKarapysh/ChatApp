using Microsoft.AspNetCore.WebUtilities;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Users;

public sealed class UsersWebApiService : WebApiServiceBase, IUsersWebApiService
{
    private protected override string BaseRoute { get; init; } = "/users";

    public UsersWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
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