namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class CallRecord
{
    public string PhoneNumber { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int Duration { get; set; } // Duration in seconds
    public string CallType { get; set; } = null!; // E.g., Incoming, Outgoing, Missed
}