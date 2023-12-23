namespace Chat.Application.RequestExceptions;

public class UserAlreadyAddedToGroupException : RequestException
{
    public UserAlreadyAddedToGroupException() : base(
        "This user is already added to one of your groups!",
        ErrorType.InvalidAction,
        HttpStatusCode.BadRequest)
    {
    }
}