namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class SMSMessage
{
    public string Sender { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}