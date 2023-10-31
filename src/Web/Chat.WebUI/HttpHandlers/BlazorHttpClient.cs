namespace Chat.WebUI.HttpHandlers;

public class BlazorHttpClient : HttpClient
{
    public BlazorHttpClient(DelegatingHandler handler, IConfiguration configuration) : base(handler)
    {
        var a = configuration["ApiUrl"];
        BaseAddress = new Uri(configuration["ApiUrl"]!);
    }
}