using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class todo_share_fix_column_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "todo_share",
                newName: "updated_utc");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "todo_share",
                newName: "created_utc");

            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry(Point, 4326)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_utc",
                table: "todo_share",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created_utc",
                table: "todo_share",
                newName: "created_at");

            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "todo",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry(Point, 4326)",
                oldNullable: true);
        }
    }
}
