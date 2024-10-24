using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class TodoController(TodoService todoService) : ApiBaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await todoService.GetTodoAsync(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var res = await todoService.GetTodosAsync();
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoDto todoDto)
    {
        var res = await todoService.CreateTodoAsync(todoDto);
        return Ok(res);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch([FromBody] TodoDto todoDto)
    {
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromBody] TodoDto todoDto)
    {
        return Ok();
    }
}