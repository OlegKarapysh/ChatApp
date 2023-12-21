namespace Chat.Application.RequestExceptions;

public sealed class EntityNotFoundException : RequestException
{
    public EntityNotFoundException(string entityName) : base(
        $"'{entityName}' is not found",
        ErrorType.NotFound,
        HttpStatusCode.NotFound)
    {
    }

    public EntityNotFoundException(string entityName, string property) : base(
        $"'{entityName}' is not found by '{property}' ",
        ErrorType.NotFound,
        HttpStatusCode.NotFound)
    {
    }
}