using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class AuthService(ApplicationDbContext dbContext, JwtService jwtTokenService)
{
    private Task HashPasswordAsync(User user)
    {
        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, user.Password);
        return Task.CompletedTask;
    }

    private Task<bool> VerifyPasswordAsync(User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        return Task.FromResult(passwordHasher.VerifyHashedPassword(user, user.Password, password) ==
                               PasswordVerificationResult.Success);
    }

    /*
    private async Task GenerateTokenAsync(User user)
    {
        var input = $"{user.Id}{user.Email}{DateTime.UtcNow.Ticks}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        foreach (var b in bytes) builder.Append(b.ToString("x2"));

        user.Token = builder.ToString();
        user.TokenValidUntilUtc = DateTime.UtcNow.AddHours(5);
    }

    public async Task<bool> VerifyTokenAsync(User user, string candidate,
        TimeSpan validDuration)
    {
        // Retrieve the stored token and creation time for this user
        var (storedToken, creationTime) = (user.Token,
            user.TokenValidUntilUtc?.AddHours(-5) ?? DateTime.UtcNow.AddHours(-24));

        // Check if the provided token matches the stored one
        if (storedToken != candidate) return false;

        // Check if the token is still within the valid duration
        return DateTime.UtcNow - creationTime <= validDuration;
    }*/

    public async Task<AuthenticationResponseDto?> RegisterUser(CreateUserDto userDto)
    {
        try
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password
            };
            await HashPasswordAsync(user);
            //await GenerateTokenAsync(user);
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            var token = jwtTokenService.GenerateJwtToken(user);
            return new AuthenticationResponseDto
            {
                User = new AuthenticatedUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                },
                Token = token
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<AuthenticationResponseDto?> LoginUser(LoginDto loginDto)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !await VerifyPasswordAsync(user, loginDto.Password)) return null;
        var token = jwtTokenService.GenerateJwtToken(user);
        return new AuthenticationResponseDto
        {
            User = new AuthenticatedUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            },
            Token = token
        };
    }
}