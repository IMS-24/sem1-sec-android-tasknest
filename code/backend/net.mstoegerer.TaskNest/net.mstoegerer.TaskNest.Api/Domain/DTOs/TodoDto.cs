namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class TodoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string Status { get; set; } = null!;
    public Guid UserId { get; set; }
    public DateTime? DueUtc { get; set; }
    public Guid AssignedToId { get; set; }
    public PointDto? Location { get; set; }
    public ICollection<AttachmentDto> Attachments { get; set; } = new HashSet<AttachmentDto>();
}