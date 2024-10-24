namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class MetaData
{
    public Guid Id { get; set; }
    public Guid UserMetaDataId { get; set; }
    public UserMetaData UserMetaData { get; set; } = null!;
    public int Order { get; set; }

    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}