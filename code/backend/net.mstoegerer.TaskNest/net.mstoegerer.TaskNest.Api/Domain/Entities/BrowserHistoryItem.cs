namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class BrowserHistoryItem
{
    public string Url { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}