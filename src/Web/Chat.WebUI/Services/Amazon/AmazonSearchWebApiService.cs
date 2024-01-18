namespace Chat.WebUI.Services.Amazon;

public sealed class AmazonSearchWebApiService : WebApiServiceBase, IAmazonSearchWebApiService
{
    private protected override string BaseRoute { get; init; } = "/amazon";

    public AmazonSearchWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService) : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<AmazonProductDto[]>> SearchProductAsync(string name)
    {
        return await GetAsync<AmazonProductDto[]>($"/search/{name}");
    }
}