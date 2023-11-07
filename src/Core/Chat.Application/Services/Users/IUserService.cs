using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.Application.Services.Users;

public interface IUserService
{
    Task<IList<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task UpdateUserAsync(UserDto userData, int id);
    Task<PagedUsersDto> SearchUsersPagedAsync(UsersPagedSearchFilterDto searchData);
}