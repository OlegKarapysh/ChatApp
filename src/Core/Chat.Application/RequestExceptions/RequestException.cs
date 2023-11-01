using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public abstract class RequestException : Exception
{
    public ErrorType ErrorType { get; }
    public HttpStatusCode StatusCode { get; }

    public RequestException(string message, ErrorType errorType, HttpStatusCode statusCode) : base(message)
    {
        ErrorType = errorType;
        StatusCode = statusCode;
    }
}