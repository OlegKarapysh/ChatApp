namespace Chat.Application.RequestExceptions;

public sealed class GroupAlreadyExistsException : RequestException
{
    public GroupAlreadyExistsException() : base(
        "Group with such name already exists!",
        ErrorType.InvalidNaming,
        HttpStatusCode.BadRequest)
    {
    }
}