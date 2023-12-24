namespace Chat.WebAPI.Middlewares;

public sealed class GenericExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GenericExceptionHandlerMiddleware> _logger;

    public GenericExceptionHandlerMiddleware(RequestDelegate next, ILogger<GenericExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception exception)
    {
        const string jsonMimeType = "application/json";
        var (errorDetails, statusCode) = exception.GetErrorDetailsAndStatusCode();

        context.Response.ContentType = jsonMimeType;
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}