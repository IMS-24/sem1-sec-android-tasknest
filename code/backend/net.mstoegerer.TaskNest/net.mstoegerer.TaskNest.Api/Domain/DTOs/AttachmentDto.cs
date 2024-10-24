namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public long Size { get; set; }
    public Guid TodoId { get; set; }

    public Guid UploadedById { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}