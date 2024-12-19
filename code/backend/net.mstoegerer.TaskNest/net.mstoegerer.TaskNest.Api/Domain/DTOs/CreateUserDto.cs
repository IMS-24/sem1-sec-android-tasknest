using System.Text.Json.Serialization;

namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateUserDto
{
    public string Email { get; set; }

    [JsonPropertyName("external_id")] public string ExternalId { get; set; } = null!;

    [JsonPropertyName("family_name")] public string FamilyName { get; set; } = null!;

    [JsonPropertyName("given_name")] public string GivenName { get; set; } = null!;
}