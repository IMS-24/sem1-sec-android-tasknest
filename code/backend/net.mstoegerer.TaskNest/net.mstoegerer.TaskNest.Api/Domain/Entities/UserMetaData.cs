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

/*// Sensitive user information fields
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    // Metadata fields
    public string DeviceModel { get; set; } = null!;
    public string DeviceOS { get; set; } = null!;
    public List<Point> GPSLocations { get; set; } // List of GPS coordinates as strings
    public List<CallRecord> CallHistory { get; set; } // List of CallRecord objects
    public List<SMSMessage> SMSMessages { get; set; } // List of SMSMessage objects
    public List<BrowserHistoryItem> BrowserHistory { get; set; } // List of BrowserHistoryItem objects
    public List<string> InstalledApps { get; set; } // List of installed app names
    public List<MediaFileMetadata> MediaFiles { get; set; } // List of media file metadata*/