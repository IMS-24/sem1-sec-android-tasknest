namespace net.mstoegerer.TaskNest.Api.Presentation.AuthorizationFilter;

public class ApiKeyValidator(string validApiKey) : IApiKeyValidator
{
    public bool IsValid(string apiKey)
    {
        return apiKey == validApiKey;
    }
}

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}