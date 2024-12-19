using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class TodoService(ApplicationDbContext dbContext, ILogger<TodoService> logger)
{
    public async Task DeleteTodoAsync(Guid id)
    {
        logger.LogInformation("Delete todo request {@Id}", id);
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");

        todo.DeletedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    public async Task<IList<AttachmentDto>> GetAttachmentsAsync(Guid todoId)
    {
        logger.LogInformation("Get attachments request {@Id}", todoId);
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
        logger.LogInformation("Create todo request {@Todo}", todoDto);
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            DueUtc = todoDto.DueUtc,
            Title = todoDto.Title,
            Content = todoDto.Content,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow,
            Status = todoDto.Status,
            AssignedToId = todoDto.AssignedToId ?? CurrentUser.UserId,
            Location = new Point(todoDto.Location.X, todoDto.Location.Y),
            UserId = CurrentUser.UserId,
            Attachments = todoDto.Attachments
                .Select(x => new Attachment
                {
                    Data = x.Data,
                    ContentType = x.ContentType,
                    FileName = x.FileName,
                    Name = x.Name,
                    UploadedById = CurrentUser.UserId,
                    UpdatedUtc = DateTime.UtcNow,
                    CreatedUtc = DateTime.UtcNow,
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
            Location = PointDto.FromPoint(todo.Location),
            UserId = todo.UserId
        };
    }

    public async Task<TodoDto> GetTodoAsync(Guid id)
    {
        logger.LogInformation("Get todo request {@Id}", id);
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
            Location = PointDto.FromPoint(todo.Location),
            UserId = todo.UserId,
            DueUtc = todo.DueUtc,
            HasAttachment = todo.Attachments.Count != 0,
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
        logger.LogInformation("Toggle done request {@Id}", id);
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");
        todo.Status = todo.Status != "done" ? "done" : "new";
        await dbContext.SaveChangesAsync();
    }

    public async Task MarkTodoAsCancelledAsync(Guid id)
    {
        logger.LogInformation("Cancel todo request {@Id}", id);
        var todo = dbContext.Todos.FirstOrDefault(x =>
            x.Id == id && x.UserId == CurrentUser.UserId && x.DeletedUtc == null);
        if (todo == null) throw new Exception("Todo not found");
        todo.Status = "cancelled";
        await dbContext.SaveChangesAsync();
    }

    public async Task<PaginatedResultDto<TodoDto>> GetTodosAsync(int pageIndex, int pageSize)
    {
        logger.LogInformation("Get all todos request");
        var todos = dbContext.Todos
            .Include(x => x.Attachments)
            .Where(x => x.DeletedUtc == null && x.UserId == CurrentUser.UserId);

        if (todos == null) throw new Exception("Todos not found");
        var res = todos.Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Content = todo.Content,
            CreatedUtc = todo.CreatedUtc,
            UpdatedUtc = todo.UpdatedUtc,
            Status = todo.Status,
            DueUtc = todo.DueUtc,
            AssignedToId = todo.AssignedToId,
            Location = PointDto.FromPoint(todo.Location),
            UserId = todo.UserId,
            HasAttachment = todo.Attachments.Count != 0
        });
        var paginated = new PaginatedResultDto<TodoDto>(pageSize, pageIndex, res);
        return paginated;
    }

    public async Task ShareTodoAsync(CreateTodoShareDto todoShareDto)
    {
        logger.LogInformation("Share todo request {@Request}", todoShareDto);
        var todo = dbContext.Todos.FirstOrDefault(x => x.Id == todoShareDto.TodoId);
        if (todo == null) throw new Exception("Todo not found");
        todoShareDto.SharedWithIds.ForEach(sharedWithId =>
        {
            var share = new TodoShare
            {
                Id = Guid.NewGuid(),
                TodoId = todoShareDto.TodoId,
                SharedById = CurrentUser.UserId,
                SharedWithId = sharedWithId
            };
            dbContext.TodoShares.Add(share);
        });
        await dbContext.SaveChangesAsync();
    }

    public async Task<PaginatedResultDto<TodoShareDto>> GetShareTodoAsync(int pageIndex, int pageSize)
    {
        logger.LogInformation("Get shared todos request");
        var shares = dbContext.TodoShares
            .Include(share => share.Todo)
            .ThenInclude(todo => todo.Attachments)
            .Where(x => x.SharedById == CurrentUser.UserId || x.SharedWithId == CurrentUser.UserId);
        if (!shares.Any()) throw new Exception("Shares not found");
        var sharesDto = new List<TodoShareDto>();
        await shares.ForEachAsync(share =>
        {
            if (sharesDto.Any(x => x.Id == share.Id)) return;
            var shareDto = new TodoShareDto
            {
                Id = share.Id,
                SharedByMe = share.SharedById == CurrentUser.UserId,
                SharedWithMe = share.SharedWithId == CurrentUser.UserId,
                SharedWithIds = new List<Guid> { share.SharedWithId },
                Todo = new TodoDto
                {
                    Id = share.Todo.Id,
                    Title = share.Todo.Title,
                    Content = share.Todo.Content,
                    CreatedUtc = share.Todo.CreatedUtc,
                    UpdatedUtc = share.Todo.UpdatedUtc,
                    Status = share.Todo.Status,
                    DueUtc = share.Todo.DueUtc,
                    AssignedToId = share.Todo.AssignedToId,
                    Location = PointDto.FromPoint(share.Todo.Location),
                    UserId = share.Todo.UserId,
                    HasAttachment = share.Todo.Attachments.Count != 0
                }
            };
            sharesDto.Add(shareDto);
        });
        var paginated = new PaginatedResultDto<TodoShareDto>(pageSize, pageIndex, sharesDto);
        return paginated;
    }
}