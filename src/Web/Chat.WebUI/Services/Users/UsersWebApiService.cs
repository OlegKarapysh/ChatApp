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
        var queryParams = new Dictionary<string, string>
        {
            { nameof(PagedSearchDto.SearchFilter), searchData.SearchFilter },
            { nameof(PagedSearchDto.Page), searchData.Page.ToString() },
            { nameof(PagedSearchDto.SortingProperty), searchData.SortingProperty },
            { nameof(PagedSearchDto.SortingOrder), ((int)searchData.SortingOrder).ToString() },
        };
        
        return await GetAsync<UsersPageDto>(QueryHelpers.AddQueryString("/search/", queryParams));
    }

    public async Task<ErrorDetailsDto?> UpdateUserInfoAsync(UserDto userData) => await PutAsync(userData);
}