using Chat.Domain.DTOs.Groups;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Groups;

public class GroupsWebApiService : WebApiServiceBase, IGroupsWebApiService
{
    private protected override string BaseRoute { get; init; } = "/groups";

    public GroupsWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<IList<GroupInfoDto>>> GetAllGroupsInfoAsync()
    {
        return await GetAsync<IList<GroupInfoDto>>("/all");
    }
}