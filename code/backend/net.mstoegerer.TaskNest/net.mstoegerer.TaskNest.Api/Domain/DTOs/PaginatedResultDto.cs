namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class PaginatedResultDto<T>
{
    public PaginatedResultDto(int pageSize, int pageIndex, IEnumerable<T> source)
    {
        PageIndex = pageIndex < 0 ? 0 : pageIndex;
        PageSize = pageSize == 0 ? 10 : pageSize;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        Items.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
    }

    public List<T> Items { get; set; } = [];

    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex + 1 < TotalPages;
}