using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class ContactService(ApplicationDbContext dbContext, ILogger<ContactService> logger)
{
    public async Task SyncContactsAsync(IList<CreateContactDto> contacts)
    {
        foreach (var createContactDto in contacts)
        {
            logger.LogInformation("Create contact {@Contact}", createContactDto);

            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = createContactDto.Name,
                Email = createContactDto.Email,
                Phone = createContactDto.Phone,
                Address = createContactDto.Address,
                Notes = createContactDto.Notes,
                UserId = CurrentUser.UserId
            };
            dbContext.Contacts.Add(contact);
        }


        await dbContext.SaveChangesAsync();
    }
}