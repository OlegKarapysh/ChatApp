using System.Net;
using Chat.Domain.Errors;

namespace Chat.Application.RequestExceptions;

public sealed class EntityNotFoundException : RequestException
{
    public EntityNotFoundException(string entityName) : base(
        $"Entity '{entityName}' is not found",
        ErrorType.NotFound,
        HttpStatusCode.NotFound)
    {
    }

    public EntityNotFoundException(string entityName, int id) : base(
        $"Entity '{entityName}' with id '{id}' is not found",
        ErrorType.NotFound,
        HttpStatusCode.NotFound)
    {
    }

    public EntityNotFoundException(string entityName, string property) : base(
        $"Entity '{entityName}' is not found by '{property}' ",
        ErrorType.NotFound,
        HttpStatusCode.NotFound)
    {
    }
}