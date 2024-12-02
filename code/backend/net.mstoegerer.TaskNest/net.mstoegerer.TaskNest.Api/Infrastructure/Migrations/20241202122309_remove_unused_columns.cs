using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_unused_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "user_metadata");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "user_metadata");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "user_metadata",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "user_metadata",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
