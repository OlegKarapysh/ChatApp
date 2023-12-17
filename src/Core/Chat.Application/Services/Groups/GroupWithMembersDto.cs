using Chat.Domain.DTOs.Groups;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Groups;

public class GroupWithMembersDto : GroupDto
{
    public List<UserDto> Members { get; set; } = new();
}