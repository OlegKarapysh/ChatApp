namespace Chat.Application.Mappings;

public static class UserMappings
{
    public static UserDto MapToDto(this User user)
    {
        return new UserDto
        {
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty
        };
    }

    public static User MapFrom(this User user, UserDto userDto)
    {
        user.UserName = userDto.UserName;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.PhoneNumber = userDto.PhoneNumber;
        
        return user;
    }
}