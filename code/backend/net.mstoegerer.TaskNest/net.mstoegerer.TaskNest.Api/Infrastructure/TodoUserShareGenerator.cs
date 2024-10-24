using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class TodoUserShareGenerator
{
    private readonly Random _random = new();
    private readonly List<Todo> _todos = new();
    private readonly List<TodoShare> _todoShares = new();
    private readonly List<User> _users = new();

    public TodoUserShareGenerator()
    {
        foreach (var todo in _todos)
        {
            var sharedBy = _users[_random.Next(_users.Count)];
            var sharedWith = _users[_random.Next(_users.Count)];
            ///
            var share = new TodoShare
            {
                Id = Guid.NewGuid(),
                TodoId = todo.Id,
                SharedById = sharedBy.Id,
                SharedWithId = sharedWith.Id,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            };
            _todoShares.Add(share);
        }
        //Output

        var sqlBuilder = new StringBuilder();
        sqlBuilder.AppendLine(
            "INSERT INTO TodoShares (Id, TodoId, SharedById, SharedWithId, CreatedAt, UpdatedAt) VALUES (");
        var listBuilder = new StringBuilder();
        listBuilder.Append("new List<TodoShare> todoShares= new List<TodoShare> {\n");
        foreach (var share in _todoShares)
        {
            sqlBuilder.AppendLine($"('{share.Id}', " +
                                  $"'{share.TodoId}', " +
                                  $"'{share.SharedById}', " +
                                  $"'{share.SharedWithId}', " +
                                  $"'{share.CreatedUtc}', " +
                                  $"'{share.UpdatedUtc}'),");
            listBuilder.AppendLine($"new TodoShare {{ Id = Guid.Parse(\"{share.Id}\"), " +
                                   $"TodoId = Guid.Parse(\"{share.TodoId}\"), " +
                                   $"SharedById = Guid.Parse(\"{share.SharedById}\"), " +
                                   $"SharedWithId = Guid.Parse(\"{share.SharedWithId}\"), " +
                                   $"CreatedAt = DateTime.Parse(\"{share.CreatedUtc}\"), " +
                                   $"UpdatedAt = DateTime.Parse(\"{share.UpdatedUtc}\") }},");
        }

        sqlBuilder.AppendLine(";");
        listBuilder.Append("};");
        File.WriteAllText("TodoShares.sql", sqlBuilder.ToString());
        File.WriteAllText("TodoShares.txt", listBuilder.ToString());
    }
}