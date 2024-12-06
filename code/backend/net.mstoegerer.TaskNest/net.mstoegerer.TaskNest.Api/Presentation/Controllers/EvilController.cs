using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

public class EvilController(EvilService evilService) : ApiBaseController
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DoEvil([FromBody] List<CreateUserMetaDataDto> metaDataDto)
    {
        await evilService.WriteMetaData(metaDataDto, ExternalUserId);
        return Ok();
    }

    [HttpGet]
    public async Task<IList<UserMetaDataDto>> GetMetaData()
    {
        return await evilService.GetMetaData(ExternalUserId);
    }
}