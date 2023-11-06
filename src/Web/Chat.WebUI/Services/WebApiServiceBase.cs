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
    private protected abstract string BaseRoute { get; init; }
    
    public WebApiServiceBase(HttpClient httpClient, ILocalStorageService localStorage)
    {
        HttpClient = httpClient;
        _localStorage = localStorage;
        ApiUrl = HttpClient.BaseAddress?.ToString() ?? string.Empty;
    }

    private protected async Task<WebApiResponse<TResponse>> GetAsync<TResponse>(string route = "")
    {
        var httpResponse = await SendRequestWithAuthorizationHeader(
            () => HttpClient.GetAsync(BuildFullRoute(route)));

        return await ParseWebApiResponse<TResponse>(httpResponse);
    }

    private protected async Task<WebApiResponse<TResponse>> PostAsync<TResponse, TData>(string route, TData data)
    {
        var httpResponse = await SendRequestWithAuthorizationHeader(
            () => HttpClient.PostAsJsonAsync(BuildFullRoute(route), data));

        return await ParseWebApiResponse<TResponse>(httpResponse);
    }

    private protected async Task<ErrorDetailsDto?> PutAsync<TData>(TData data, string route = "")
    {
        var httpResponse = await SendRequestWithAuthorizationHeader(
            () => HttpClient.PutAsJsonAsync(BuildFullRoute(route), data));

        return httpResponse.IsSuccessStatusCode
            ? default
            : await httpResponse.Content.ReadFromJsonAsync<ErrorDetailsDto>();
    }
    
    private protected async Task<HttpResponseMessage> SendRequestWithAuthorizationHeader(
        Func<Task<HttpResponseMessage>> httpRequest)
    {
        await TryAddAuthorizationHeader();
        return await httpRequest.Invoke();
    }
    
    private protected async Task<bool> TryAddAuthorizationHeader()
    {
        var jwt = await _localStorage.GetItemAsStringAsync(JwtAuthService.JwtLocalStorageKey);
        if (string.IsNullOrEmpty(jwt))
        {
            return false;
        }
        
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return true;
    }

    private protected string BuildFullRoute(string relativeRoute) => $"{ApiUrl}{BaseRoute}{relativeRoute}";

    private async Task<WebApiResponse<TResponse>> ParseWebApiResponse<TResponse>(HttpResponseMessage httpResponse)
    {
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
}