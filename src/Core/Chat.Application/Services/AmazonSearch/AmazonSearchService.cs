using Chat.Application.Services.OpenAI;

namespace Chat.Application.Services.AmazonSearch;

public sealed class AmazonSearchService : IAmazonSearchService
{
    // TODO: create assistant for each user.
    private const string AmazonAssistantId = "asst_jIeAugPnrtlH6a2EVtMU2uek";
    //private const string AmazonThreadId = "thread_bem7Xe7zzsrwJfcPZjclznNZ";
    private const string SearchResultAttribute = "data-component-type";
    private const string SearchResultAttributeValue = "s-search-result";
    private readonly HttpClient _httpClient;
    private readonly IOpenAiService _openAiService;

    public AmazonSearchService(HttpClient httpClient, IOpenAiService openAiService)
    {
        _httpClient = httpClient;
        _openAiService = openAiService;
        _httpClient.BaseAddress = new Uri(IAmazonSearchService.AmazonUrl);
    }

    public async Task<IEnumerable<AmazonProductDto>> SearchProductAsync(string name)
    {
        var response = await _httpClient.GetAsync($"s?k={name}");
        // TODO: detect encoding and compression dynamically.
        var htmlText = await ParseHtmlTextAsync(response);
        var searchResultDivs = GetSearchResultsDivs(htmlText);
        var products = new List<AmazonProductDto>();
        foreach (var searchResult in searchResultDivs)
        {
            var product = await _openAiService.GetFunctionCallArgsAsync<AmazonProductDto>(searchResult, AmazonAssistantId);
            if (product is not null)
            {
                products.Add(product);
            }
        }

        return products;
    }

    private async Task<string> ParseHtmlTextAsync(HttpResponseMessage response)
    {
        var responseStream = await response.Content.ReadAsStreamAsync();
        await using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressedStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    private IEnumerable<string> GetSearchResultsDivs(string htmlText)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlText);
        if (htmlDoc.DocumentNode is null)
        {
            throw new Exception("Couldn't parse document node from HTML");
        }
        
        return htmlDoc.DocumentNode
                      .Descendants("div")
                      .Where(div => div.GetAttributeValue(SearchResultAttribute, string.Empty) == SearchResultAttributeValue)
                      .Select(div => div.InnerHtml);
    }
}