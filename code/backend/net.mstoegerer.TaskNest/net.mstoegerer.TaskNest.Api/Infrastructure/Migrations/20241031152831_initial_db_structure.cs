using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_db_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "todo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location = table.Column<Point>(type: "geometry", nullable: true),
                    due_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_to_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_users_assigned_to_id",
                        column: x => x.assigned_to_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_todo_users_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    password = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location = table.Column<Point>(type: "geometry", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_metadata", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_metadata_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attachment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    data = table.Column<byte[]>(type: "bytea", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    todo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uploaded_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment", x => x.id);
                    table.ForeignKey(
                        name: "fk_attachment_todos_todo_id",
                        column: x => x.todo_id,
                        principalTable: "todo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attachment_users_uploaded_by_id",
                        column: x => x.uploaded_by_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "todo_share",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    todo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shared_with_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shared_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_share", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_share_todo_todo_id",
                        column: x => x.todo_id,
                        principalTable: "todo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_share_users_shared_by_id",
                        column: x => x.shared_by_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_share_users_shared_with_id",
                        column: x => x.shared_with_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meta_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_meta_data_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meta_data", x => x.id);
                    table.ForeignKey(
                        name: "fk_meta_data_user_meta_data_user_meta_data_id",
                        column: x => x.user_meta_data_id,
                        principalTable: "user_metadata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_utc", "email", "name", "password", "updated_utc" },
                values: new object[] { new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@tasknest.com", "Admin", "admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "todo",
                columns: new[] { "id", "assigned_to_id", "content", "created_utc", "due_utc", "location", "status", "title", "updated_utc", "user_id" },
                values: new object[] { new Guid("e832db47-e640-4539-825b-b3940ff882d9"), new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b"), "<ol><li>First</li><li>Second</li></ol>", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "new", "First Todo", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b") });

            migrationBuilder.InsertData(
                table: "attachment",
                columns: new[] { "id", "content_type", "created_utc", "data", "file_name", "name", "size", "todo_id", "updated_utc", "uploaded_by_id" },
                values: new object[] { new Guid("d5bfdef9-331d-4162-bf50-f3e43f699499"), "text/plain", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new byte[] { 0, 1, 2 }, "test.txt", "Pepe", 100L, new Guid("e832db47-e640-4539-825b-b3940ff882d9"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b") });

            migrationBuilder.CreateIndex(
                name: "ix_attachment_todo_id",
                table: "attachment",
                column: "todo_id");

            migrationBuilder.CreateIndex(
                name: "ix_attachment_uploaded_by_id",
                table: "attachment",
                column: "uploaded_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_meta_data_user_meta_data_id",
                table: "meta_data",
                column: "user_meta_data_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_assigned_to_id",
                table: "todo",
                column: "assigned_to_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_user_id",
                table: "todo",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_share_shared_by_id",
                table: "todo_share",
                column: "shared_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_share_shared_with_id",
                table: "todo_share",
                column: "shared_with_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_share_todo_id",
                table: "todo_share",
                column: "todo_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_metadata_location",
                table: "user_metadata",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_user_metadata_user_id",
                table: "user_metadata",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachment");

            migrationBuilder.DropTable(
                name: "meta_data");

            migrationBuilder.DropTable(
                name: "todo_share");

            migrationBuilder.DropTable(
                name: "user_metadata");

            migrationBuilder.DropTable(
                name: "todo");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
