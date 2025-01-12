namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Notes { get; set; } = null!;
}