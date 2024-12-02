namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class TodoShare
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; } = null!;
    public Guid SharedWithId { get; set; }
    public User SharedWith { get; set; } = null!;
    public Guid SharedById { get; set; }
    public User SharedBy { get; set; } = null!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}