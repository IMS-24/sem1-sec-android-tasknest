using System.Text;
using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class EvilService(ApplicationDbContext applicationDbContext)
{
    public readonly string EvilPath = "collection/";

    public async Task<IList<UserMetaDataDto>> GetMetaData()
    {
        var userMetaDataQuery = applicationDbContext
            .UserMetaData
            .Include(x => x.User)
            .Include(x => x.MetaData);
        var res = userMetaDataQuery.ToList();
        var dtos = res.Select(x => new UserMetaDataDto
        {
            CreatedUtc = x.CreatedUtc,
            Id = x.Id,
            Location = x.Location == null ? null : new PointDto { X = x.Location.X, Y = x.Location.Y },
            MetaData = x.MetaData.Select(y => new MetaDataDto
            {
                Id = y.Id,
                Order = y.Order,
                Key = y.Key,
                Value = y.Value
            }).ToList(),
            Password = x.Password,
            PhoneNumber = x.PhoneNumber,
            UserId = x.UserId
        });
        return dtos.ToList();
    }

    public async Task WriteMetaData(CreateUserMetaDataDto createUserMetaDataDto)
    {
        var userMetaDataEntity = new UserMetaData
        {
            //MetaData =createUserMetaDataDto.MetaData.ToEntity(),
            CreatedUtc = createUserMetaDataDto.CreatedUtc,
            PhoneNumber = createUserMetaDataDto.PhoneNumber,
            Password = createUserMetaDataDto.Password,
            UserId = createUserMetaDataDto.UserId,
            Location = createUserMetaDataDto.Location == null
                ? null
                : new Point(createUserMetaDataDto.Location.X, createUserMetaDataDto.Location.Y)
        };
        userMetaDataEntity.MetaData.Add(new MetaData
        {
            Key = createUserMetaDataDto.MetaData.Key,
            Value = createUserMetaDataDto.MetaData.Value
        });
        await WriteToCsvAsync(createUserMetaDataDto);
        var res = await applicationDbContext.UserMetaData.AddAsync(userMetaDataEntity);
        await applicationDbContext.SaveChangesAsync();
    }

    private async Task WriteToCsvAsync(CreateUserMetaDataDto metaData)
    {
        await CreateEvilDirectory();
        // Write to CSV
        if (File.Exists(Path.Combine(EvilPath, metaData.UserId + ".csv")))
            await File.AppendAllTextAsync("collection/" + metaData.UserId + ".csv",
                metaData.ToCsv(false));
        else
            await File.WriteAllBytesAsync("collection/" + metaData.UserId + ".csv",
                Encoding.UTF8.GetBytes(metaData.ToCsv(true)));
    }

    private async Task CreateEvilDirectory()
    {
        if (!Directory.Exists(EvilPath))
            Directory.CreateDirectory(EvilPath);
    }
}