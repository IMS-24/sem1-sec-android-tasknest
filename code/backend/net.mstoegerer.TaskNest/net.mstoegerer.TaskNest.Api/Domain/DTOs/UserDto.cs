namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}