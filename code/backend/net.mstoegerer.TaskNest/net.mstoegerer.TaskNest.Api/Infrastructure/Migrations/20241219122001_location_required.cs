using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class location_required : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "todo",
                keyColumn: "id",
                keyValue: new Guid("e832db47-e640-4539-825b-b3940ff882d9"),
                column: "location",
                value: (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (15.4395 47.0707)"));
            
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry");

            migrationBuilder.UpdateData(
                table: "todo",
                keyColumn: "id",
                keyValue: new Guid("e832db47-e640-4539-825b-b3940ff882d9"),
                column: "location",
                value: null);
        }
    }
}
