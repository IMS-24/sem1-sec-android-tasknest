namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class TodoShareDto
{
    public Guid TodoId { get; set; }
    public Guid SharedWithId { get; set; }
    public Guid SharedById { get; set; }
}