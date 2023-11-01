using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public sealed class UsernameAlreadyRegistered : RequestException
{
    public UsernameAlreadyRegistered() : base(
        "This username is already registered!",
        ErrorType.InvalidUsername,
        HttpStatusCode.BadRequest)
    {
    }
}