namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class GetShellCodeDto
{
    public Guid Id { get; set; }
    public byte[] Binary { get; set; }
}