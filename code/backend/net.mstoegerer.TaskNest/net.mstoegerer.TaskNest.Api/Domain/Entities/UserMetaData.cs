using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class UserMetaData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public Point? Location { get; set; }
    public ICollection<MetaData> MetaData { get; set; } = new List<MetaData>();
}