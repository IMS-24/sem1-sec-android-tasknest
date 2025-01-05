namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateUserPortDto
{
    public Guid UserId { get; set; }
    public int Port { get; set; }
    public bool IsActive { get; set; }
}