using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class ContactService(ApplicationDbContext dbContext, ILogger<ContactService> logger)
{
    public async Task<Guid> CreateContactAsync(CreateContactDto contactDto)
    {
        logger.LogInformation("Create contact {@Contact}", contactDto);
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            Name = contactDto.Name,
            Email = contactDto.Email,
            Phone = contactDto.Phone,
            Address = contactDto.Address,
            Notes = contactDto.Notes,
            UserId = contactDto.UserId
        };
        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync();
        return contact.Id;
    }
}