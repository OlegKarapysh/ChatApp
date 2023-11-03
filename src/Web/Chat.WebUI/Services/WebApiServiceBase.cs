using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Chat.Domain.DTOs;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services;

public abstract class WebApiServiceBase
{
    private protected readonly HttpClient HttpClient;
    private readonly ILocalStorageService _localStorage;
    private protected readonly string ApiUrl;
    private protected string BaseRoute { get; init; }
    private protected string FullRoute => $"{ApiUrl}{BaseRoute}";
    
    public WebApiServiceBase(HttpClient httpClient, ILocalStorageService localStorage)
    {
        HttpClient = httpClient;
        _localStorage = localStorage;
        ApiUrl = HttpClient.BaseAddress?.ToString() ?? string.Empty;
    }

    private protected async Task<WebApiResponse<TResponse>> PostAsync<TResponse, TData>(string route, TData data)
    {
        await TryAddAuthorization();
        var httpResponse = await HttpClient.PostAsJsonAsync($"{FullRoute}{route}", data);
        
        return httpResponse.IsSuccessStatusCode
            ? new WebApiResponse<TResponse>
            {
                IsSuccessful = true,
                Content = await httpResponse.Content.ReadFromJsonAsync<TResponse>()
            }
            : new WebApiResponse<TResponse>
            {
                IsSuccessful = false,
                ErrorDetails = await httpResponse.Content.ReadFromJsonAsync<ErrorDetailsDto>()
            };
    }

    private protected async Task<bool> TryAddAuthorization()
    {
        var jwt = await _localStorage.GetItemAsStringAsync(JwtAuthService.JwtLocalStorageKey);
        if (string.IsNullOrEmpty(jwt))
        {
            return false;
        }
        Console.WriteLine(jwt);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        Console.WriteLine(HttpClient.DefaultRequestHeaders.Authorization);
        return true;
    }
}