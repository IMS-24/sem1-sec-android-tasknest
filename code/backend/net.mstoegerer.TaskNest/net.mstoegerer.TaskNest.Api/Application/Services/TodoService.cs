using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class TodoService(ApplicationDbContext dbContext)
{
    public async Task DeleteTodoAsync(Guid id)
    {
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");

        todo.DeletedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttachmentDto>> GetAttachmentsAsync(Guid todoId)
    {
        var attachments = dbContext.Attachments.Where(x => x.TodoId == todoId);
        return await attachments.Select(x => new AttachmentDto
        {
            Id = x.Id,
            Name = x.Name,
            FileName = x.FileName,
            ContentType = x.ContentType,
            Data = x.Data,
            Size = x.Size,
            TodoId = x.TodoId,
            UploadedById = x.UploadedById,
            CreatedUtc = x.CreatedUtc,
            UpdatedUtc = x.UpdatedUtc
        }).ToListAsync();
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todoDto)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            DueUtc = todoDto.DuetUtc,
            Title = todoDto.Title,
            Content = todoDto.Content,
            CreatedUtc = todoDto.CreatedUtc,
            UpdatedUtc = todoDto.UpdatedUtc,
            Status = todoDto.Status,
            AssignedToId = todoDto.AssignedToId ?? CurrentUser.UserId,
            Location = todoDto.Location == null
                ? null
                : new Point(todoDto.Location.X, todoDto.Location.Y),
            UserId = CurrentUser.UserId,
            Attachments = todoDto.Attachments
                .Select(x => new Attachment
                {
                    Data = x.Data,
                    ContentType = x.ContentType,
                    FileName = x.FileName,
                    Name = x.Name,
                    UploadedById = CurrentUser.UserId,
                    UpdatedUtc = x.UpdatedUtc,
                    CreatedUtc = x.CreatedUtc,
                    Size = x.Data.Length
                }).ToList()
        };
        dbContext.Todos.Add(todo);
        await dbContext.SaveChangesAsync();
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
            UserId = todo.UserId
        };
    }

    public async Task<TodoDto> GetTodoAsync(Guid id)
    {
        var todo = await dbContext.Todos
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedUtc == null && x.UserId == CurrentUser.UserId);
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
            DueUtc = todo.DueUtc,
            Attachments = todo.Attachments
                .Select(x => new AttachmentDto
                {
                    Id = x.Id,
                    TodoId = x.TodoId,
                    Data = x.Data,
                    ContentType = x.ContentType,
                    Name = x.Name,
                    CreatedUtc = x.CreatedUtc,
                    UpdatedUtc = x.UpdatedUtc
                }).ToList()
        };
    }

    public async Task ToggleDoneAsync(Guid id)
    {
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");
        todo.Status = todo.Status != "done" ? "done" : "new";
        await dbContext.SaveChangesAsync();
    }

    public async Task MarkTodoAsCancelledAsync(Guid id)
    {
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");
        todo.Status = "cancelled";
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TodoDto>> GetTodosAsync()
    {
        var todos = dbContext.Todos
            .Include(x => x.Attachments)
            .Where(x => x.DeletedUtc == null && x.UserId == CurrentUser.UserId);

        if (todos == null) throw new Exception("Todos not found");
        var res = await todos.Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Content = todo.Content,
            CreatedUtc = todo.CreatedUtc,
            UpdatedUtc = todo.UpdatedUtc,
            Status = todo.Status,
            DueUtc = todo.DueUtc,
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
        return res;
    }

    public async Task ShareTodoAsync(TodoShareDto todoShareDto)
    {
        var todo = dbContext.Todos.FirstOrDefault(x => x.Id == todoShareDto.TodoId);
        if (todo == null) throw new Exception("Todo not found");
        var share = new TodoShare
        {
            Id = Guid.NewGuid(),
            TodoId = todoShareDto.TodoId,
            SharedById = todoShareDto.SharedById,
            SharedWithId = todoShareDto.SharedWithId
        };
        dbContext.TodoShares.Add(share);
        await dbContext.SaveChangesAsync();
    }
}