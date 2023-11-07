namespace Chat.Domain.Web;

public class PagedList<T> : List<T>
{
    public const int DefaultPageSize = 5;
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalTotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize = DefaultPageSize)
    {
        TotalTotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        AddRange(items);
    }
    
    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
        
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}