namespace Chat.Application.RequestExceptions;

public sealed class InvalidTokenException : RequestException
{
    public InvalidTokenException(string tokenType) : base(
        $"{tokenType} token is not valid!",
        ErrorType.InvalidToken,
        HttpStatusCode.Forbidden)
    {
    }
}