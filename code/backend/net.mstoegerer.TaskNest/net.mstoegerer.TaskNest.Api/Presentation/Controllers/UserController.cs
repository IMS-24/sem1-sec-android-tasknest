using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Presentation.AuthorizationFilter;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class UserController(UserService userService) : ApiBaseController
{
    [HttpGet]
    /*[Authorize]*/
    public async Task<ActionResult> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpPost("register")]
    [ApiKey]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        await userService.AddUserAsync(createUserDto);
        return Created();
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<Guid>> GetUserByEmail(string email)
    {
        var userId = await userService.GetUserByEmailAsync(email);
        return Ok(userId);
    }
}