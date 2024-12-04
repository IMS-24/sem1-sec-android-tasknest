using System.Text.Json.Serialization;

namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class CreateUserDto
{
    public string Email { get; set; }

    [JsonPropertyName("external_id")] public string ExternalId { get; set; }

    [JsonPropertyName("family_name")] public string FamilyName { get; set; }

    [JsonPropertyName("given_name")] public string GivenName { get; set; }
}