using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class PointDto
{
    public double X { get; set; }
    public double Y { get; set; }

    public static PointDto FromPoint(Point point)
    {
        return new PointDto
        {
            X = point.Y,
            Y = point.X
        };
    }

    public static Point ToPoint(PointDto pointDto)
    {
        return new Point(pointDto.X, pointDto.Y);
    }
}