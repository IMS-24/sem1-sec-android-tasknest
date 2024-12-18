namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateTodoShareDto
{
    public Guid TodoId { get; set; }
    public List<Guid> SharedWithIds { get; set; } = [];
}