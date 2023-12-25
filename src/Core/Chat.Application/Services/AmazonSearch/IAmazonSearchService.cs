namespace Chat.Application.Services.AmazonSearch;

public interface IAmazonSearchService
{
    const string HttpClientName = "AmazonClient";
    const string AmazonUrl = "https://www.amazon.com/";
    Task<List<AmazonProductDto>> SearchProductAsync(string name);
}