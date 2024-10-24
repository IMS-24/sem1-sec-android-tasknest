using net.mstoegerer.TaskNest.Api.Domain.DTOs;
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

    /*public async Task<User?> GetUserByTokenAsync(string token)
    {
        var res = await dbContext.Users.FirstOrDefaultAsync(x => x.Token == token);
        return res;
    }*/
}