namespace Chat.Application.RequestExceptions;

public sealed class BadRegistrationException : RequestException
{
    public BadRegistrationException() : base(
        "Registration failed!",
        ErrorType.InvalidEmailOrPassword,
        HttpStatusCode.BadRequest)
    {
    }
}