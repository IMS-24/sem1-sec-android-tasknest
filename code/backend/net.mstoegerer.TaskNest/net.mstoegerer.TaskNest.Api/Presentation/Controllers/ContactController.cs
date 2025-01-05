using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class ContactController(ContactService contactService, ILogger<AttachmentController> logger)
    : ApiBaseController
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateContactDto contactDto)
    {
        logger.LogInformation("Create contact {@Contact}", contactDto);
        var res = await contactService.CreateContactAsync(contactDto);
        return Created(string.Empty, res);
    }
}