using Chat.Domain.DTOs;

namespace Chat.Domain.Web;

public class WebApiResponse<T>
{
    public bool IsSuccessful { get; init; }
    public ErrorDetailsDto? ErrorDetails { get; init; }
    public T? Content { get; init; }
}