namespace Chat.Domain.DTOs.Authentication;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 100 characters long!")]
    [EmailAddress(ErrorMessage = "Email must be valid!")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Password is required!")]
    [IdentityPassword(ErrorMessage =
        "Password must contain an uppercase character, a lowercase character, a digit, and a non-alphanumeric character!")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters long!")]
    public string Password { get; set; } = default!;
}