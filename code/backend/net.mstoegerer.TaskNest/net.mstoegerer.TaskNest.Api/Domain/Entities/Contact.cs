namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}