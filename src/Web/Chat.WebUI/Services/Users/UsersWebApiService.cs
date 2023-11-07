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

    public async Task<WebApiResponse<PagedUsersDto>> GetSearchedUsersPage(UsersPagedSearchFilterDto searchData)
    {
        var queryParams = new Dictionary<string, string>
        {
            { nameof(UsersPagedSearchFilterDto.SearchFilter), searchData.SearchFilter },
            { nameof(UsersPagedSearchFilterDto.Page), searchData.Page.ToString() },
            { nameof(UsersPagedSearchFilterDto.SortingProperty), searchData.SortingProperty },
            { nameof(UsersPagedSearchFilterDto.SortingOrder), ((int)searchData.SortingOrder).ToString() },
        };
        var a = QueryHelpers.AddQueryString("/search/", queryParams);
        Console.WriteLine(a);
        return await GetAsync<PagedUsersDto>(QueryHelpers.AddQueryString("/search/", queryParams));
    }

    public async Task<ErrorDetailsDto?> UpdateUserInfoAsync(UserDto userData) => await PutAsync(userData);
}