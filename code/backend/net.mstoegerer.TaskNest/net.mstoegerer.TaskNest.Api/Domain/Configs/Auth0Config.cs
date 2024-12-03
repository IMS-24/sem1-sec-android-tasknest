namespace net.mstoegerer.TaskNest.Api.Domain.Configs;

public class Auth0Config
{
    public string Audience { get; set; } = null!;
    public string Domain { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}