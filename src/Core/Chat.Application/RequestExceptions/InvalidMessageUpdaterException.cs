using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public class InvalidMessageUpdaterException : RequestException
{
    public InvalidMessageUpdaterException() : base(
        "Only the user who sent the message can change it!",
        ErrorType.InvalidAction,
        HttpStatusCode.BadRequest)
    {
    }
}