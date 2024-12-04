namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class UserMetaDataDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public PointDto? Location { get; set; }

    public ICollection<MetaDataDto> MetaData { get; set; } = new List<MetaDataDto>();
}