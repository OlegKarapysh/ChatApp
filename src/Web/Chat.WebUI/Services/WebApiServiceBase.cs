using System.Net.Http.Json;
using Chat.Domain.DTOs;
using Chat.Domain.Web;

namespace Chat.WebUI.Services;

public abstract class WebApiServiceBase
{
    private protected readonly HttpClient HttpClient;
    private protected readonly string ApiUrl;
    private protected string BaseRoute { get; init; }
    private protected string FullRoute => $"{ApiUrl}{BaseRoute}";
    
    public WebApiServiceBase(HttpClient httpClient, IConfiguration configuration)
    {
        HttpClient = httpClient;
        ApiUrl = configuration["ApiUrl"]!;
    }

    private protected async Task<WebApiResponse<TResponse>> PostAsync<TResponse, TData>(string route, TData data)
    {
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
}