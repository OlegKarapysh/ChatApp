namespace Chat.WebUI.Services.Amazon;

public interface IAmazonSearchWebApiService
{
    Task<WebApiResponse<AmazonProductDto[]>> SearchProductAsync(string name);
}