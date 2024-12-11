using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class AttachmentService(ApplicationDbContext dbContext)
{
    /*public async Task DeleteAttachmentAsync(Guid id)
    {
        var attachment = dbContext.Attachments.FirstOrDefaultAsync(x=>x.Id == id&&x.);
    }*/


    public async Task<AttachmentDto> CreateAttachmentAsync(CreateAttachmentDto attachmentDto)
    {
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            Name = attachmentDto.Name,
            FileName = attachmentDto.FileName,
            ContentType = attachmentDto.ContentType,
            Data = attachmentDto.Data,
            Size = attachmentDto.Data.Length,
            TodoId = attachmentDto.TodoId,
            UploadedById = CurrentUser.UserId,
            CreatedUtc = attachmentDto.CreatedUtc,
            UpdatedUtc = attachmentDto.UpdatedUtc
        };
        dbContext.Attachments.Add(attachment);
        await dbContext.SaveChangesAsync();
        return new AttachmentDto
        {
            Id = attachment.Id,
            Name = attachment.Name,
            FileName = attachment.FileName,
            ContentType = attachment.ContentType,
            Data = attachment.Data,
            Size = attachment.Size,
            TodoId = attachment.TodoId,
            UploadedById = attachment.UploadedById,
            CreatedUtc = attachment.CreatedUtc,
            UpdatedUtc = attachment.UpdatedUtc
        };
    }

    public async Task<AttachmentDto> GetAttachmentAsync(Guid id)
    {
        var attachment = await dbContext.Attachments.FirstOrDefaultAsync(x => x.Id == id);
        if (attachment == null) throw new Exception("Attachment not found");
        return new AttachmentDto
        {
            Id = attachment.Id,
            Name = attachment.Name,
            FileName = attachment.FileName,
            ContentType = attachment.ContentType,
            Data = attachment.Data,
            Size = attachment.Size,
            TodoId = attachment.TodoId,
            UploadedById = attachment.UploadedById,
            CreatedUtc = attachment.CreatedUtc,
            UpdatedUtc = attachment.UpdatedUtc
        };
    }
}