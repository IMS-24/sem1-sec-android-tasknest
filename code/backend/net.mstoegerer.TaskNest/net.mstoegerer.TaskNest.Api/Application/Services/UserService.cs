using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class UserService(ApplicationDbContext dbContext)
{
    public Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        return Task.FromResult<IEnumerable<UserDto>>(dbContext.Users.Select(x => new UserDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email
        }));
    }

    public async Task AddUserAsync(CreateUserDto createUserDto)
    {
        await dbContext.Users.AddAsync(new User
        {
            Name = $"{createUserDto.GivenName} {createUserDto.FamilyName}",
            Email = createUserDto.Email,
            ExternalId = createUserDto.ExternalId
        });
        await dbContext.SaveChangesAsync();
    }
}