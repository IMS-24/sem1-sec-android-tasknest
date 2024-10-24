using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedUtc { get; set; } = DateTime.Now;
    public DateTime UpdatedUtc { get; set; }
    public Point? Location { get; set; } = null!;
    public DateTime? DueUtc { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid AssignedToId { get; set; }
    public User AssignedTo { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
    public string Status { get; set; } = "new";
    public ICollection<TodoShare> Shares { get; set; } = new HashSet<TodoShare>();
}