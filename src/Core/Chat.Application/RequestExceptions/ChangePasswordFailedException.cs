using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public sealed class ChangePasswordFailedException : RequestException
{
    public ChangePasswordFailedException() : base(
        "Failed to change user's password!",
        ErrorType.InvalidPassword,
        HttpStatusCode.BadRequest)
    {
    }
}