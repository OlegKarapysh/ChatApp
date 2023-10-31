namespace Chat.WebUI.HttpHandlers;

public class BlazorHttpClient : HttpClient
{
    public BlazorHttpClient(HttpMessageHandler handler, IConfiguration configuration) : base(handler)
    {
        BaseAddress = new Uri(configuration["ApiUrl"]!);
    }
}