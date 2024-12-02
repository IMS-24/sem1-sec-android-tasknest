using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class todo_delete_flag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_utc",
                table: "todo",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "todo",
                keyColumn: "id",
                keyValue: new Guid("e832db47-e640-4539-825b-b3940ff882d9"),
                column: "deleted_utc",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_utc",
                table: "todo");
        }
    }
}
