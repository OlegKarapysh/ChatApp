

namespace Chat.WebUI.Services;

public abstract class WebApiServiceBase
{
    private protected readonly string ApiUrl;
    private protected readonly HttpClient HttpClient;
    private protected readonly ITokenStorageService TokenService;
    private protected abstract string BaseRoute { get; init; }
    
    public WebApiServiceBase(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
    {
        HttpClient = httpClientFactory.CreateClient(JwtAuthInterceptor.HttpClientWithJwtInterceptorName);
        TokenService = tokenService;
        ApiUrl = HttpClient.BaseAddress?.ToString() ?? string.Empty;
    }

    private protected async Task<WebApiResponse<TResponse>> GetAsync<TResponse>(string route = "")
    {
        var httpResponse = await HttpClient.GetAsync(BuildFullRoute(route));
        return await ParseWebApiResponse<TResponse>(httpResponse);
    }

    private protected async Task<WebApiResponse<TResponse>> PostAsync<TResponse, TData>(TData data, string route = "")
    {
        var httpResponse = await HttpClient.PostAsJsonAsync(BuildFullRoute(route), data);
        return await ParseWebApiResponse<TResponse>(httpResponse);
    }

    private protected async Task<ErrorDetailsDto?> PutAsync<TData>(TData data, string route = "")
    {
        var httpResponse = await HttpClient.PutAsJsonAsync(BuildFullRoute(route), data);

        return httpResponse.IsSuccessStatusCode
            ? default
            : await httpResponse.Content.ReadFromJsonAsync<ErrorDetailsDto>();
    }
    
    private protected async Task<WebApiResponse<TResponse>> PutAsync<TResponse, TData>(TData data, string route = "")
    {
        var httpResponse = await HttpClient.PutAsJsonAsync(BuildFullRoute(route), data);
        return await ParseWebApiResponse<TResponse>(httpResponse);
    }

    private protected async Task<ErrorDetailsDto?> DeleteAsync(string route = "")
    {
        var httpResponse = await HttpClient.DeleteAsync(BuildFullRoute(route));
        if (httpResponse.IsSuccessStatusCode)
        {
            return default;
        }

        try
        {
            return await httpResponse.Content.ReadFromJsonAsync<ErrorDetailsDto>();
        }
        catch (Exception e)
        {
            return new ErrorDetailsDto($"Unexpected error: {e.Message}", ErrorType.Unknown);
        }
    }

    private protected string BuildFullRoute(string relativeRoute) => $"{ApiUrl}{BaseRoute}{relativeRoute}";

    private protected async Task<WebApiResponse<TResponse>> ParseWebApiResponse<TResponse>(HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return new WebApiResponse<TResponse>
            {
                IsSuccessful = true,
                Content = await httpResponse.Content.ReadFromJsonAsync<TResponse>()
            };
        }

        try
        {
            return new WebApiResponse<TResponse>
            {
                IsSuccessful = false,
                ErrorDetails = await httpResponse.Content.ReadFromJsonAsync<ErrorDetailsDto>()
            };
        }
        catch (Exception e)
        {
            return new WebApiResponse<TResponse>
            {
                IsSuccessful = false,
                ErrorDetails = new ErrorDetailsDto($"Unexpected error: {e.Message}", ErrorType.Unknown)
            };
        }
    }

    private protected Dictionary<string, string> GetQueryParamsForPagedSearch(PagedSearchDto searchData)
    {
        return new Dictionary<string, string>
        {
            { nameof(PagedSearchDto.SearchFilter), searchData.SearchFilter ?? string.Empty },
            { nameof(PagedSearchDto.Page), searchData.Page.ToString() },
            { nameof(PagedSearchDto.SortingProperty), searchData.SortingProperty },
            { nameof(PagedSearchDto.SortingOrder), ((int)searchData.SortingOrder).ToString() },
        };
    }
}