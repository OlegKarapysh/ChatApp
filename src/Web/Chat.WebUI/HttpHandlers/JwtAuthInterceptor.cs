using Blazored.LocalStorage;
using Chat.WebUI.Providers;

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
            JwtAuthenticationStateProvider.JwtTokenLocalStorageKey, cancellation);
        request.Headers.Add("Authorization", $"Bearer {jwt}");

        return await base.SendAsync(request, cancellation);
    }
}