namespace Chat.Domain.DTOs.AmazonSearch;

public class AmazonProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Rating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}