using System.ComponentModel.DataAnnotations;
using Chat.Domain.ValidationAttributes;

namespace Chat.Domain.DTOs.Authentication;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Password is required!")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters long!")]
    public string CurrentPassword { get; set; } = default!;
    [Required(ErrorMessage = "Password is required!")]
    [IdentityPassword(ErrorMessage =
        "Password must contain an uppercase character, a lowercase character, a digit, and a non-alphanumeric character!")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters long!")]
    public string NewPassword { get; set; } = default!;
    [Compare(nameof(NewPassword), ErrorMessage = "New passwords must match!")]
    public string RepeatNewPassword { get; set; } = default!;
}