namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class AmazonController : ControllerBase
{
    private readonly IAmazonSearchService _amazonSearchService;

    public AmazonController(IAmazonSearchService amazonSearchService)
    {
        _amazonSearchService = amazonSearchService;
    }
    
    // TODO: remove AllowAnonymous.
    [AllowAnonymous, HttpGet("search/{productName}")]
    public async Task<ActionResult<AmazonProductDto[]>> SearchProduct(string productName)
    {
        return Ok(await _amazonSearchService.SearchProductAsync(productName));
    }
}