namespace Chat.Application.RequestExceptions;

public class InvalidPasswordException : RequestException
{
    public InvalidPasswordException() : base(
        "Invalid password.",
        ErrorType.InvalidPassword,
        HttpStatusCode.BadRequest)
    {
    }
}