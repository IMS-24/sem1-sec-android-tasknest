using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class TodoController(TodoService todoService) : ApiBaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await todoService.GetTodoAsync(id);
        return Ok(res);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var res = await todoService.GetTodosAsync(ExternalUserId);
        return Ok(res);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto todoDto)
    {
        var res = await todoService.CreateTodoAsync(ExternalUserId, todoDto);
        return Ok(res);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await todoService.DeleteTodoAsync(id);
        return Ok();
    }

    [HttpPost("share")]
    public async Task<IActionResult> Shared([FromBody] TodoShareDto todoShareDto)
    {
        await todoService.ShareTodoAsync(todoShareDto);
        return Ok();
    }
}