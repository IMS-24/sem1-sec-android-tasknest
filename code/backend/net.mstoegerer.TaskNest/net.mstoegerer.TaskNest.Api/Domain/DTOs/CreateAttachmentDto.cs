namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateAttachmentDto
{
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public Guid TodoId { get; set; }
    public Guid UploadedById { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}