using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Users;

public sealed class UsersWebApiService : WebApiServiceBase, IUsersWebApiService
{
    private protected override string BaseRoute { get; init; }

    public UsersWebApiService(HttpClient httpClient, ILocalStorageService localStorage)
        : base(httpClient, localStorage)
    {
        BaseRoute = "/users";
    }

    public async Task<WebApiResponse<UserDto>> GetCurrentUserInfoAsync() => await GetAsync<UserDto>();

    public async Task<ErrorDetailsDto?> UpdateUserInfoAsync(UserDto userData) => await PutAsync(userData);
}