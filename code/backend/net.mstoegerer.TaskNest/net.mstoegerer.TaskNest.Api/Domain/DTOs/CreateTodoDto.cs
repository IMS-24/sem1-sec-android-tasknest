namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateTodoDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Status { get; set; } = null!;
    public Guid? AssignedToId { get; set; }
    public DateTime? DueUtc { get; set; }
    public PointDto? Location { get; set; }
    public ICollection<CreateAttachmentDto> Attachments { get; set; } = new HashSet<CreateAttachmentDto>();
}