using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace net.mstoegerer.TaskNest.Api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase
{
    protected HttpContext Context => HttpContext;
    protected string? ExternalUserId => Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
}