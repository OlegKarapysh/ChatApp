using Chat.Domain.Web;

namespace Chat.Domain.DTOs.Users;

public class PagedUsersDto
{
    public UserDto[] Users { get; set; }
    public PageInfo PageInfo { get; set; }
}