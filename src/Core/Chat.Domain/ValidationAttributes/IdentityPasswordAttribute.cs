using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Chat.Domain.ValidationAttributes;

public class IdentityPasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_\W]).+$";
        return Regex.IsMatch(value.ToString(), passwordRegex);
    }
}