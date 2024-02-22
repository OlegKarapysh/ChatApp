using Chat.Application.Services.OpenAI;

namespace Chat.Application.Services.AmazonSearch;

public sealed class AmazonSearchService : IAmazonSearchService
{
    // TODO: create assistant for each user.
    private const string AmazonAssistantId3 = "asst_P4WTCXC6aAPGcwYgJLI6iUu2";
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
        var searchResultDivs = ParseSearchResults(htmlText);
        searchResultDivs.Take(2).ForEach(x => LogHtml(x));
        var functionCallTasks = searchResultDivs.Take(5).Select(searchResult =>
            _openAiService.GetFunctionCallArgsAsync<AmazonProductDto>(searchResult, AmazonAssistantId3)).ToList();
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
    
    private string[] ParseSearchResults(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);
        var searchResultDivs = doc.DocumentNode.SelectNodes("//div[@data-component-type='s-search-result']");
        if (searchResultDivs is null)
        {
            return Array.Empty<string>();
        }

        var results = new List<string>();
        foreach (var div in searchResultDivs)
        {
            var filteredContents = new List<string>();
            var imgTags = div.SelectNodes(".//img");
            if (imgTags is not null)
            {
                foreach (var img in imgTags)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        filteredContents.Add($"<img src=\"{src}\">");
                    }
                }
            }
            var textContainingElements = div.SelectNodes(".//*[not(self::script) and not(self::style) and normalize-space(text())]");
            if (textContainingElements is not null)
            {
                foreach (var element in textContainingElements)
                {
                    var classAttr = element.GetAttributeValue("class", null!);
                    var classAttribute = classAttr is not null ? $"class=\"{classAttr}\"" : "";
                    var cleanElement = HtmlNode.CreateNode($"<{element.Name} {classAttribute}>{element.InnerText}</{element.Name}>");
                    filteredContents.Add(cleanElement.OuterHtml);
                }
            }
            
            var content = string.Join(' ', filteredContents);
            content = content.Replace("<span", "<p").Replace("</span>", "</p>");
            content = content.Replace("<div", "<p").Replace("</div>", "</p>");
            results.Add(content.Trim());
        }

        return results.ToArray();
    }

    private void LogHtml(string html, string path = @"C:\Users\sebas\OneDrive\Desktop\productsHtml.txt")
    {
        // TODO: for testing HTML scraping.
        File.AppendAllText(path, "\n\n===============================\n\n");
        File.AppendAllText(path, html);
    }
}