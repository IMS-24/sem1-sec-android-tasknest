using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dummy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry(Point, 4326)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry(Point, 4326)");
        }
    }
}
