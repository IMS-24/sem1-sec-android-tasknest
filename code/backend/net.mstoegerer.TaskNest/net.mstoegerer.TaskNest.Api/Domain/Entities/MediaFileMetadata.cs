namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class MediaFileMetadata
{
    public string FilePath { get; set; } = null!;
    public string FileType { get; set; } = null!; // E.g., photo, video
    public DateTime Timestamp { get; set; }
}