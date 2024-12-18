using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class TodoController(TodoService todoService, ILogger<TodoController> logger) : ApiBaseController
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
        var res = await todoService.GetTodosAsync();
        return Ok(res);
    }

    [HttpPut("{id:guid}/done")]
    [Authorize]
    public async Task<IActionResult> ToggleDoneAsync(Guid id)
    {
        await todoService.ToggleDoneAsync(id);
        return Ok();
    }

    [HttpPut("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelTodoAsync(Guid id)
    {
        await todoService.MarkTodoAsCancelledAsync(id);
        return Ok();
    }

    [HttpGet("{id:guid}/attachment")]
    [Authorize]
    public async Task<IActionResult> GetAttachments(Guid id)
    {
        var res = await todoService.GetAttachmentsAsync(id);
        return Ok(res);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto todoDto)
    {
        var res = await todoService.CreateTodoAsync(todoDto);
        return Created();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await todoService.DeleteTodoAsync(id);
        return Ok();
    }

    [HttpPost("share")]
    [Authorize]
    public async Task<IActionResult> Share([FromBody] CreateTodoShareDto todoShareDto)
    {
        await todoService.ShareTodoAsync(todoShareDto);
        return Created();
    }

    [HttpGet("share")]
    [Authorize]
    public async Task<IActionResult> GetShares()
    {
        var res = await todoService.GetShareTodoAsync();
        return Ok(res);
    }
}