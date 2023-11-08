namespace Chat.Domain.DTOs.Users;

public class UsersPageDto : PageDto
{
    public UserDto[]? Users { get; set; }
}