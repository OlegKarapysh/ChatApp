namespace Chat.Domain.DTOs;

public class RegistrationDto : LoginDto
{
    public string UserName { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}