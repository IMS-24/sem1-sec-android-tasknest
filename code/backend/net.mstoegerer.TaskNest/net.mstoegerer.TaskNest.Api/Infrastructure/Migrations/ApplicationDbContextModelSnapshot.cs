﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

#nullable disable

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("data");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("file_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<Guid>("TodoId")
                        .HasColumnType("uuid")
                        .HasColumnName("todo_id");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_utc");

                    b.Property<Guid>("UploadedById")
                        .HasColumnType("uuid")
                        .HasColumnName("uploaded_by_id");

                    b.HasKey("Id")
                        .HasName("pk_attachment");

                    b.HasIndex("TodoId")
                        .HasDatabaseName("ix_attachment_todo_id");

                    b.HasIndex("UploadedById")
                        .HasDatabaseName("ix_attachment_uploaded_by_id");

                    b.ToTable("attachment", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("d5bfdef9-331d-4162-bf50-f3e43f699499"),
                            ContentType = "text/plain",
                            CreatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Data = new byte[] { 0, 1, 2 },
                            FileName = "test.txt",
                            Name = "Pepe",
                            Size = 100L,
                            TodoId = new Guid("e832db47-e640-4539-825b-b3940ff882d9"),
                            UpdatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            UploadedById = new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b")
                        });
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.MetaData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("key");

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Order"));

                    b.Property<Guid>("UserMetaDataId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_meta_data_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_meta_data");

                    b.HasIndex("UserMetaDataId")
                        .HasDatabaseName("ix_meta_data_user_meta_data_id");

                    b.ToTable("meta_data", (string)null);
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AssignedToId")
                        .HasColumnType("uuid")
                        .HasColumnName("assigned_to_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .IsUnicode(true)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<DateTime?>("DeletedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_utc");

                    b.Property<DateTime?>("DueUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("due_utc");

                    b.Property<Point>("Location")
                        .HasColumnType("geometry")
                        .HasColumnName("location");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasDefaultValue("new")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_utc");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_todo");

                    b.HasIndex("AssignedToId")
                        .HasDatabaseName("ix_todo_assigned_to_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_todo_user_id");

                    b.ToTable("todo", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e832db47-e640-4539-825b-b3940ff882d9"),
                            AssignedToId = new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b"),
                            Content = "<ol><li>First</li><li>Second</li></ol>",
                            CreatedUtc = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Status = "new",
                            Title = "First Todo",
                            UpdatedUtc = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            UserId = new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b")
                        });
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.TodoShare", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<Guid>("SharedById")
                        .HasColumnType("uuid")
                        .HasColumnName("shared_by_id");

                    b.Property<Guid>("SharedWithId")
                        .HasColumnType("uuid")
                        .HasColumnName("shared_with_id");

                    b.Property<Guid>("TodoId")
                        .HasColumnType("uuid")
                        .HasColumnName("todo_id");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_utc");

                    b.HasKey("Id")
                        .HasName("pk_todo_share");

                    b.HasIndex("SharedById")
                        .HasDatabaseName("ix_todo_share_shared_by_id");

                    b.HasIndex("SharedWithId")
                        .HasDatabaseName("ix_todo_share_shared_with_id");

                    b.HasIndex("TodoId")
                        .HasDatabaseName("ix_todo_share_todo_id");

                    b.ToTable("todo_share", (string)null);
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("email");

                    b.Property<Guid?>("ExternalId")
                        .HasColumnType("uuid")
                        .HasColumnName("external_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_utc");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("23d8d722-4037-466c-a68f-98e90e9ba66b"),
                            CreatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@tasknest.com",
                            Name = "Admin",
                            UpdatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.UserMetaData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<Point>("Location")
                        .HasColumnType("geometry")
                        .HasColumnName("location");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_metadata");

                    b.HasIndex("Location")
                        .HasDatabaseName("ix_user_metadata_location");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Location"), "GIST");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_metadata_user_id");

                    b.ToTable("user_metadata", (string)null);
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.Attachment", b =>
                {
                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.Todo", "Todo")
                        .WithMany("Attachments")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_attachment_todos_todo_id");

                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "UploadedBy")
                        .WithMany("UploadedAttachments")
                        .HasForeignKey("UploadedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_attachment_users_uploaded_by_id");

                    b.Navigation("Todo");

                    b.Navigation("UploadedBy");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.MetaData", b =>
                {
                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.UserMetaData", "UserMetaData")
                        .WithMany("MetaData")
                        .HasForeignKey("UserMetaDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_meta_data_user_meta_data_user_meta_data_id");

                    b.Navigation("UserMetaData");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.Todo", b =>
                {
                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "AssignedTo")
                        .WithMany("AssignedTodos")
                        .HasForeignKey("AssignedToId")
                        .HasConstraintName("fk_todo_users_assigned_to_id");

                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "User")
                        .WithMany("Todos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_todo_users_user_id");

                    b.Navigation("AssignedTo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.TodoShare", b =>
                {
                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "SharedBy")
                        .WithMany("ProvidedShares")
                        .HasForeignKey("SharedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_todo_share_users_shared_by_id");

                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "SharedWith")
                        .WithMany("ReceivedShares")
                        .HasForeignKey("SharedWithId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_todo_share_users_shared_with_id");

                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.Todo", "Todo")
                        .WithMany("Shares")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_todo_share_todo_todo_id");

                    b.Navigation("SharedBy");

                    b.Navigation("SharedWith");

                    b.Navigation("Todo");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.UserMetaData", b =>
                {
                    b.HasOne("net.mstoegerer.TaskNest.Api.Domain.Entities.User", "User")
                        .WithMany("MetaDataAssociation")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_metadata_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.Todo", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Shares");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.User", b =>
                {
                    b.Navigation("AssignedTodos");

                    b.Navigation("MetaDataAssociation");

                    b.Navigation("ProvidedShares");

                    b.Navigation("ReceivedShares");

                    b.Navigation("Todos");

                    b.Navigation("UploadedAttachments");
                });

            modelBuilder.Entity("net.mstoegerer.TaskNest.Api.Domain.Entities.UserMetaData", b =>
                {
                    b.Navigation("MetaData");
                });
#pragma warning restore 612, 618
        }
    }
}
