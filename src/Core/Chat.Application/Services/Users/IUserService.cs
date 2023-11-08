using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Users;

public interface IUserService
{
    Task<IList<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task UpdateUserAsync(UserDto userData, int id);
    Task<UsersPageDto> SearchUsersPagedAsync(PagedSearchDto searchData);
}