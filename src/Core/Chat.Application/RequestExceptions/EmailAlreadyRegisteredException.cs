namespace Chat.Application.RequestExceptions;

public sealed class EmailAlreadyRegisteredException : RequestException
{
    public EmailAlreadyRegisteredException() : base(
        "Email is already registered. Try another one",
        ErrorType.InvalidEmail,
        HttpStatusCode.BadRequest)
    {
    }
}