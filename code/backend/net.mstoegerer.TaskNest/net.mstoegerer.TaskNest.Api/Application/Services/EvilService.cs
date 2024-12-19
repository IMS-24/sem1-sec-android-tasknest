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
                    : new Point(cUserMetaDataDto.Location.X, cUserMetaDataDto.Location.Y) { SRID = 4326 }
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