using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public sealed class MissingClaimException : RequestException
{
    public MissingClaimException(string claimName) : base(
        $"Couldn't find user's '{claimName}' claim!",
        ErrorType.NotFound,
        HttpStatusCode.Unauthorized)
    {
    }
}