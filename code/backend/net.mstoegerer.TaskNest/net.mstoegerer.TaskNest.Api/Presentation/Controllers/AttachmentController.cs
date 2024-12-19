using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class AttachmentController(AttachmentService attachmentService, ILogger<AttachmentController> logger)
    : ApiBaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        logger.LogInformation("Get attachment {Id}", id);
        var res = await attachmentService.GetAttachmentAsync(id);
        return Ok(res);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateAttachmentDto attachmentDto)
    {
        logger.LogInformation("Create attachment {@Attachment}", attachmentDto);
        var res = await attachmentService.CreateAttachmentAsync(attachmentDto);
        return Created();
    }
}