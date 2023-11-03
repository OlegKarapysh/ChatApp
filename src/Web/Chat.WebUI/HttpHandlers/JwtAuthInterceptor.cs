using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.HttpHandlers;

public class JwtAuthInterceptor : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtAuthInterceptor(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellation)
    {
        var jwt = await _localStorage.GetItemAsStringAsync(
            JwtAuthService.JwtLocalStorageKey, cancellation);
        Console.WriteLine(jwt);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        Console.WriteLine(request.Headers.Authorization);

        return await base.SendAsync(request, cancellation);
    }
}