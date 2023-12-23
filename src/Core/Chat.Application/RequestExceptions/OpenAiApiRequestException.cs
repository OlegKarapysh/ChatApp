namespace Chat.Application.RequestExceptions;

public sealed class OpenAiApiRequestException : RequestException
{
    public OpenAiApiRequestException(string message) : base(message, ErrorType.InvalidAction, HttpStatusCode.BadRequest)
    {
    }
}