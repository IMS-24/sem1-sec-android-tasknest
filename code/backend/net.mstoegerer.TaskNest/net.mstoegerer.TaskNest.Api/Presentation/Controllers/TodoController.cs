using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class TodoController(TodoService todoService, ILogger<TodoController> logger) : ApiBaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        logger.LogInformation("Get todo request");
        var res = await todoService.GetTodoAsync(id);
        return Ok(res);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResultDto<TodoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        logger.LogInformation("Get all todos request");
        var res = await todoService.GetTodosAsync(pageIndex, pageSize);
        return Ok(res);
    }

    [HttpPut("{id:guid}/done")]
    [Authorize]
    public async Task<IActionResult> ToggleDoneAsync(Guid id)
    {
        logger.LogInformation("Toggle done request");
        await todoService.ToggleDoneAsync(id);
        return Ok();
    }

    [HttpPut("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelTodoAsync(Guid id)
    {
        logger.LogInformation("Cancel todo request");
        await todoService.MarkTodoAsCancelledAsync(id);
        return Ok();
    }

    [HttpGet("{id:guid}/attachment")]
    [Authorize]
    [ProducesResponseType(typeof(IList<AttachmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttachments(Guid id)
    {
        logger.LogInformation("Get attachments request");
        var res = await todoService.GetAttachmentsAsync(id);
        return Ok(res);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto todoDto)
    {
        logger.LogInformation("Create todo request");
        var res = await todoService.CreateTodoAsync(todoDto);
        return Created();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("Delete todo request");
        await todoService.DeleteTodoAsync(id);
        return Ok();
    }

    [HttpPost("share")]
    [Authorize]
    public async Task<IActionResult> Share([FromBody] CreateTodoShareDto todoShareDto)
    {
        logger.LogInformation("Share todo request");
        await todoService.ShareTodoAsync(todoShareDto);
        return Created();
    }

    [HttpGet("share")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResultDto<TodoShareDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShares([FromQuery] int page, [FromQuery] int pageSize)
    {
        logger.LogInformation("Get share todos request");
        var res = await todoService.GetShareTodoAsync(page, pageSize);
        return Ok(res);
    }
}