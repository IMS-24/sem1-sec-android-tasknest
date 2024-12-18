namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class TodoShareDto
{
    public Guid Id { get; set; }
    public TodoDto Todo { get; set; } = null!;
    public List<Guid> SharedWithIds { get; set; } = [];
}