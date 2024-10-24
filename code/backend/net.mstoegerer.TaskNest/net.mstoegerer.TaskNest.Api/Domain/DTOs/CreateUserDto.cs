namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateUserDto : UserDto
{
    public string Password { get; set; }
}