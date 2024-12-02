using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class TodoService(ApplicationDbContext dbContext)
{
    public async Task DeleteTodoAsync(Guid id)
    {
        var todo = dbContext.Todos.FirstOrDefault(x => x.Id == id);
        if (todo == null) throw new Exception("Todo not found");

        todo.DeletedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    public async Task<TodoDto> CreateTodoAsync(TodoDto todoDto)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = todoDto.Title,
            Content = todoDto.Content,
            CreatedUtc = todoDto.CreatedUtc,
            UpdatedUtc = todoDto.UpdatedUtc,
            Status = todoDto.Status,
            AssignedToId = todoDto.AssignedToId,
            Location = todoDto.Location == null
                ? null
                : new Point(todoDto.Location.X, todoDto.Location.Y),
            UserId = todoDto.UserId,
            Attachments = todoDto.Attachments
                .Select(x => new Attachment
                {
                    Data = x.Data,
                    ContentType = x.ContentType,
                    Name = x.Name
                }).ToList()
        };
        dbContext.Todos.Add(todo);
        await dbContext.SaveChangesAsync();
        return todoDto;
    }

    public async Task<TodoDto> GetTodoAsync(Guid id)
    {
        var todo = await dbContext.Todos
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");
        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Content = todo.Content,
            CreatedUtc = todo.CreatedUtc,
            UpdatedUtc = todo.UpdatedUtc,
            Status = todo.Status,
            AssignedToId = todo.AssignedToId,
            Location = todo.Location == null
                ? null
                : PointDto.FromPoint(todo.Location),
            UserId = todo.UserId,
            Attachments = todo.Attachments
                .Select(x => new AttachmentDto
                {
                    Data = x.Data,
                    ContentType = x.ContentType,
                    Name = x.Name
                }).ToList()
        };
    }

    public async Task<IEnumerable<TodoDto>> GetTodosAsync()
    {
        var todos = dbContext.Todos
            .Include(x => x.Attachments)
            .Where(x => x.DeletedUtc == null);

        if (todos == null) throw new Exception("Todos not found");
        return await todos.Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Content = todo.Content,
            CreatedUtc = todo.CreatedUtc,
            UpdatedUtc = todo.UpdatedUtc,
            Status = todo.Status,
            AssignedToId = todo.AssignedToId,
            Location = todo.Location == null
                ? null
                : PointDto.FromPoint(todo.Location),
            UserId = todo.UserId,
            Attachments = todo.Attachments
                .Select(x => new AttachmentDto
                {
                    Data = x.Data,
                    ContentType = x.ContentType,
                    Name = x.Name
                })
                .ToList()
        }).ToListAsync();
    }
}