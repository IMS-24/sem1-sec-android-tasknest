namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateAttachmentDto
{
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public Guid TodoId { get; set; }
}