namespace Chat.Domain.Web;

public class PageInfo
{
    public const int DefaultPageSize = 5;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; } = 1;
    public int PageSize { get; set; } = DefaultPageSize;

    public PageInfo()
    {
    }
    
    public PageInfo(int totalCount, int pageNumber, int pageSize = DefaultPageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
    
    public bool HasPrevious() => CurrentPage > 1;
    public bool HasNext() => CurrentPage < TotalPages;
}