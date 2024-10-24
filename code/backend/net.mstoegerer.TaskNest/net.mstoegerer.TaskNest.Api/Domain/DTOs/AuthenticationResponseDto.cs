namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class AuthenticationResponseDto
{
    public AuthenticatedUserDto? User { get; set; } = null!;
    public string? Token { get; set; } = null!;
}

public class AuthenticatedUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}