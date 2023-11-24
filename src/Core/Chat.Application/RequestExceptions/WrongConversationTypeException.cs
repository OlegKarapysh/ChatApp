using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public sealed class WrongConversationTypeException : RequestException
{
    public WrongConversationTypeException() : base(
        "Requested operation with this conversation type is not supported!",
        ErrorType.InvalidAction,
        HttpStatusCode.BadRequest)
    {
    }
}