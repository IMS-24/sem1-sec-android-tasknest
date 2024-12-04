using Microsoft.AspNetCore.Mvc;

namespace net.mstoegerer.TaskNest.Api.Presentation.AuthorizationFilter;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}