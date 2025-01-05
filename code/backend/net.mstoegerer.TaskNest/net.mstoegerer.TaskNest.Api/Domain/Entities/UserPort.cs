namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class UserPort
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Port { get; set; }
    public bool IsActive { get; set; } = false;
}