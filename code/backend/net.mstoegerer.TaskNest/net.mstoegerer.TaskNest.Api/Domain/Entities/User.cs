namespace net.mstoegerer.TaskNest.Api.Domain.Entities;

public class User
{
    public string ExternalId { get; set; } = null!;
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
    public ICollection<UserMetaData> MetaDataAssociation { get; set; } = new HashSet<UserMetaData>();
    public ICollection<Todo> Todos { get; set; } = new HashSet<Todo>();
    public ICollection<TodoShare> ProvidedShares { get; set; } = new HashSet<TodoShare>();
    public ICollection<TodoShare> ReceivedShares { get; set; } = new HashSet<TodoShare>();
    public ICollection<Todo> AssignedTodos { get; set; } = new HashSet<Todo>();
    public ICollection<Attachment> UploadedAttachments { get; set; } = new HashSet<Attachment>();
    public ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    public ICollection<UserPort> PortMappings { get; set; } = new HashSet<UserPort>();
}