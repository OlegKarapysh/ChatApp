using Chat.Domain.DTOs.Users;

namespace Chat.Domain.DTOs.Groups;

public class GroupWithMembersDto : GroupDto
{
    public List<UserDto> Members { get; set; } = new();
}