using Chat.Application.Services.OpenAI;

namespace Chat.Application.Services.AmazonSearch;

public sealed class AmazonSearchService : IAmazonSearchService
{
    // TODO: create assistant for each user.
    private const string AmazonAssistantId = "asst_2cVSz0i6XuiCVmYc8zKcJADW";
    private const string SearchResultAttributeName = "data-component-type";
    private const string SearchResultAttributeValue = "s-search-result";
    private readonly HttpClient _httpClient;
    private readonly IOpenAiService _openAiService;

    public AmazonSearchService(HttpClient httpClient, IOpenAiService openAiService)
    {
        _httpClient = httpClient;
        _openAiService = openAiService;
        _httpClient.BaseAddress = new Uri(IAmazonSearchService.AmazonUrl);
    }

    public async Task<List<AmazonProductDto>> SearchProductsAsync(string name)
    {
        var response = await _httpClient.GetAsync($"s?k={name}");
        // TODO: detect encoding and compression dynamically.
        var htmlText = await ParseHtmlTextAsync(response);
        var searchResultDivs = GetSearchResultsDivs(htmlText).ToArray();
        LogHtml(searchResultDivs.First());
        // TODO: remove Take() call.
        var functionCallTasks = searchResultDivs.Take(2).Select(searchResult =>
            _openAiService.GetFunctionCallArgsAsync<AmazonProductDto>(searchResult, AmazonAssistantId)).ToList();
        await Task.WhenAll(functionCallTasks);
        
        return functionCallTasks.Where(t => t.Result is not null).Select(t => t.Result!).ToList();
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
                      .Where(div => div.GetAttributeValue(SearchResultAttributeName, string.Empty) == SearchResultAttributeValue)
                      .Select(div => div.InnerHtml);
    }

    private void LogHtml(string html, string path = @"C:\Users\sebas\OneDrive\Desktop\productsHtml.txt")
    {
        // TODO: for testing HTML scraping.
        File.AppendAllText(path, "\n\n===============================\n\n");
        File.AppendAllText(path, html);
    }
}