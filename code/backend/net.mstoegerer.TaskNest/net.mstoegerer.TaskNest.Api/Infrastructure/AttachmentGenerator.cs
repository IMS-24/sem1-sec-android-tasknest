using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class AttachmentGenerator
{
    public AttachmentGenerator()
    {
        Generate();
    }

    private void Generate()
    {
        var attachments = new List<Attachment>();

        var random = new Random();
        var today = DateTime.Parse("2024-10-29T00:00:00Z");

        var fileNames = new List<string>
        {
            "document.pdf", "photo.jpg", "report.docx", "presentation.pptx",
            "spreadsheet.xlsx", "diagram.svg", "contract.pdf", "receipt.jpg",
            "manual.pdf", "blueprint.png"
        };

        var contentTypes = new List<string>
        {
            "application/pdf", "image/jpeg", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "image/svg+xml", "image/png"
        };
        var todos = new List<Todo>
        {
            new()
            {
                Id = Guid.Parse("b2cfe1d2-3f4c-40f1-a64e-7f8a2f4b9c53"),
                Title = "Buy groceries",
                Content = "Need to buy milk, eggs, and bread.",
                CreatedUtc = new DateTime(2024, 10, 02, 12, 45, 30, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 05, 13, 15, 42, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 10, 18, 30, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("a13d07f3-1c91-4d2c-9296-ff8d7b7fabc3"), // Alice Johnson
                AssignedToId = Guid.Parse("d4c95f9b-a62e-4f32-b8ab-92f832ad0f8a"), // Charlie Brown
                Status = "new",
                Location = new Point(-73.935242, 40.730610) { SRID = 4326 } // New York, NY
            },
            new()
            {
                Id = Guid.Parse("c3a1e7f5-7b8d-489c-9d3a-6b7d4e9b4a2d"),
                Title = "Finish project report",
                Content = "Complete the financial analysis for Q3.",
                CreatedUtc = new DateTime(2024, 10, 04, 10, 20, 25, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 06, 11, 25, 40, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 15, 14, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("bf98b232-33f5-4a29-8dff-78f04b7d48c9"), // Bob Smith
                AssignedToId = Guid.Parse("e42b5f8a-0b29-4b3e-83f8-cf3f284e9a1d"), // Jane Lee
                Status = "done",
                Location = new Point(-0.127758, 51.507351) { SRID = 4326 } // London, UK
            },
            new()
            {
                Id = Guid.Parse("d8c5e2a1-7d2e-45a1-8f2a-3e5b7c9f3d4e"),
                Title = "Plan vacation",
                Content = "Research destinations and create itinerary.",
                CreatedUtc = new DateTime(2024, 09, 20, 09, 15, 00, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 09, 25, 10, 35, 22, 0, DateTimeKind.Utc),
                DueUtc = null,
                UserId = Guid.Parse("d4c95f9b-a62e-4f32-b8ab-92f832ad0f8a"), // Charlie Brown
                AssignedToId = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"), // Hannah Wilson
                Status = "postponed",
                Location = new Point(2.352222, 48.856613) { SRID = 4326 } // Paris, France
            },
            new()
            {
                Id = Guid.Parse("f3a8c1e4-9b2d-4f5b-8c9a-2f7b5d3c7e9a"),
                Title = "Organize team meeting",
                Content = "Coordinate schedules and prepare agenda.",
                CreatedUtc = new DateTime(2024, 10, 18, 13, 10, 15, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 20, 14, 25, 35, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 25, 10, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("b04e9a4d-f329-4a3a-8d3e-08f6b492b48c"), // Diana Garcia
                AssignedToId = Guid.Parse("b7c4e39d-fc29-4783-8c5a-53f7b5f8dca1"), // Kevin Taylor
                Status = "new"
                // No location specified for this todo
            },
            new()
            {
                Id = Guid.Parse("a9e2f3c8-7f5b-4c3d-8a7d-5c9e1a4b7d8c"),
                Title = "Submit expense report",
                Content = "Upload receipts and finalize report.",
                CreatedUtc = new DateTime(2024, 10, 12, 08, 30, 20, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 15, 09, 45, 50, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 20, 18, 30, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("2f9b6d1d-6f9a-467a-81b4-e6b67473b3e5"), // Evan Miller
                AssignedToId = Guid.Parse("f9b7c5e8-3d9a-4a7f-8c2b-5a7e9f8d2c3a"), // Rachel Adams
                Status = "overdue",
                Location = new Point(-118.243683, 34.052235) { SRID = 4326 } // Los Angeles, CA
            },
            new()
            {
                Id = Guid.Parse("4e9b7c5f-7d3a-48b1-8c2f-9b5d7e3f4c1a"),
                Title = "Prepare client presentation",
                Content = "Review slides and practice presentation.",
                CreatedUtc = new DateTime(2024, 10, 19, 11, 15, 40, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 20, 14, 35, 22, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 30, 09, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("73c64fd9-0d15-4f82-8db6-c329ba47a7b2"), // Fiona Davis
                AssignedToId = Guid.Parse("7e5c9d3b-2a4e-4f5d-8a3c-9f8b7d5e3c2f"), // Tina Young
                Status = "new",
                Location = new Point(139.691711, 35.689487) { SRID = 4326 } // Tokyo, Japan
            },
            new()
            {
                Id = Guid.Parse("b1c8f5d3-4a2e-4b5c-8f7d-2e9c5d7a3f9b"),
                Title = "Update project roadmap",
                Content = "Refine timeline and milestones for Q4.",
                CreatedUtc = new DateTime(2024, 10, 10, 14, 05, 00, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 12, 15, 20, 25, 0, DateTimeKind.Utc),
                DueUtc = null,
                UserId = Guid.Parse("c07e5d2e-916f-4a68-bb7f-5c87b326d83d"), // George Martinez
                AssignedToId = Guid.Parse("925e8b36-b2c5-41e4-8a5c-58c967e5dfd5"), // Ian Anderson
                Status = "done"
                // No location specified for this todo
            },
            new()
            {
                Id = Guid.Parse("d4c7b1e5-8a7f-45f1-9d3c-3a9f7b8e2f1d"),
                Title = "Conduct employee review",
                Content = "Prepare feedback for Q4 review session.",
                CreatedUtc = new DateTime(2024, 09, 29, 09, 45, 30, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 02, 10, 55, 45, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 15, 15, 30, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"), // Hannah Wilson
                AssignedToId = Guid.Parse("a13d07f3-1c91-4d2c-9296-ff8d7b7fabc3"), // Alice Johnson
                Status = "postponed",
                Location = new Point(151.209290, -33.868820) { SRID = 4326 } // Sydney, Australia
            },
            new()
            {
                Id = Guid.Parse("d1b16a7c-d2e7-4d99-9c71-c5792836535b"),
                Title = "Check inventory levels",
                Content = "Verify stock counts for incoming orders.",
                CreatedUtc = new DateTime(2024, 10, 15, 09, 25, 15, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 16, 10, 45, 30, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 11, 02, 13, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("b7c4e39d-fc29-4783-8c5a-53f7b5f8dca1"), // Kevin Taylor
                AssignedToId = Guid.Parse("f9b7c5e8-3d9a-4a7f-8c2b-5a7e9f8d2c3a"), // Rachel Adams
                Status = "new",
                Location = new Point(15.4280, 47.0709) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("0129a8db-c17e-479c-b1da-d63321954bbe"),
                Title = "Organize monthly meeting",
                Content = "Prepare agenda and coordinate with participants.",
                CreatedUtc = new DateTime(2024, 10, 20, 10, 20, 25, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 22, 11, 30, 45, 0, DateTimeKind.Utc),
                DueUtc = null,
                UserId = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"), // Hannah Wilson
                AssignedToId = Guid.Parse("c07e5d2e-916f-4a68-bb7f-5c87b326d83d"), // George Martinez
                Status = "postponed",
                Location = new Point(15.4501, 47.0812) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("30642b82-18f2-4685-b7fb-e8f68934edfb"),
                Title = "Inspect machinery",
                Content = "Regular maintenance check on equipment.",
                CreatedUtc = new DateTime(2024, 10, 10, 08, 45, 35, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 15, 09, 35, 55, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 11, 01, 15, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("925e8b36-b2c5-41e4-8a5c-58c967e5dfd5"), // Ian Anderson
                AssignedToId = Guid.Parse("a13d07f3-1c91-4d2c-9296-ff8d7b7fabc3"), // Alice Johnson
                Status = "done",
                Location = new Point(15.4613, 47.0678) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("c1352b31-c47c-4a71-b94f-bcff37f2051f"),
                Title = "Finalize project documentation",
                Content = "Compile project details for closing report.",
                CreatedUtc = new DateTime(2024, 10, 12, 14, 30, 22, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 15, 15, 25, 42, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 25, 12, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("f9b7c5e8-3d9a-4a7f-8c2b-5a7e9f8d2c3a"), // Rachel Adams
                AssignedToId = Guid.Parse("bf98b232-33f5-4a29-8dff-78f04b7d48c9"), // Bob Smith
                Status = "overdue",
                Location = new Point(15.4255, 47.0659) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("c18841e3-ef2e-4381-804e-3506835d205f"),
                Title = "Conduct safety audit",
                Content = "Review safety protocols and hazard analysis.",
                CreatedUtc = new DateTime(2024, 10, 18, 12, 15, 45, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 19, 13, 25, 50, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 11, 05, 10, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("73c64fd9-0d15-4f82-8db6-c329ba47a7b2"), // Fiona Davis
                AssignedToId = Guid.Parse("7e5c9d3b-2a4e-4f5d-8a3c-9f8b7d5e3c2f"), // Tina Young
                Status = "new",
                Location = new Point(15.4397, 47.0602) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("4b33e255-3c0c-40b0-837b-0b0c7673cebc"),
                Title = "Create budget proposal",
                Content = "Draft budget for upcoming fiscal year.",
                CreatedUtc = new DateTime(2024, 10, 07, 09, 40, 17, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 09, 10, 25, 35, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 28, 17, 30, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("d4c95f9b-a62e-4f32-b8ab-92f832ad0f8a"), // Charlie Brown
                AssignedToId = Guid.Parse("c3f8d2a7-5c3f-4d5e-95c7-3a9f7e5c98b7"), // Oliver King
                Status = "new",
                Location = new Point(15.4299, 47.0804) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("c865f939-f3c3-4304-b68f-221d549cc12f"),
                Title = "Review product quality",
                Content = "Ensure products meet quality standards.",
                CreatedUtc = new DateTime(2024, 10, 17, 15, 20, 50, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 19, 16, 35, 27, 0, DateTimeKind.Utc),
                DueUtc = null,
                UserId = Guid.Parse("f9b7c5e8-3d9a-4a7f-8c2b-5a7e9f8d2c3a"), // Rachel Adams
                AssignedToId = Guid.Parse("b04e9a4d-f329-4a3a-8d3e-08f6b492b48c"), // Diana Garcia
                Status = "new",
                Location = new Point(15.4441, 47.0718) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("811022c1-2a10-4f3f-8a96-f83e88eb3947"),
                Title = "Update training materials",
                Content = "Revise materials for new employee orientation.",
                CreatedUtc = new DateTime(2024, 10, 13, 10, 45, 30, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 14, 12, 30, 50, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 10, 30, 09, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"), // Hannah Wilson
                AssignedToId = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"), // self
                Status = "done",
                Location = new Point(15.4472, 47.0732) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("072aa631-9024-48fe-ad7b-152ea2aae5da"),
                Title = "Host annual conference",
                Content = "Organize and execute the company conference.",
                CreatedUtc = new DateTime(2024, 10, 11, 08, 15, 55, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 13, 09, 25, 50, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 11, 20, 18, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("3e7b9c58-3e2d-4534-8a9c-5b1c7f8b45e3"), // Nina Lewis
                AssignedToId = Guid.Parse("3e7b9c58-3e2d-4534-8a9c-5b1c7f8b45e3"), // Self
                Status = "postponed",
                Location = new Point(15.4195, 47.0761) { SRID = 4326 }
            },
            new()
            {
                Id = Guid.Parse("6f82a68b-4c86-444e-908c-906f99f25663"),
                Title = "Implement new software",
                Content = "Deploy and test the updated software.",
                CreatedUtc = new DateTime(2024, 10, 21, 14, 30, 17, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 10, 22, 15, 35, 45, 0, DateTimeKind.Utc),
                DueUtc = new DateTime(2024, 11, 10, 09, 00, 00, 0, DateTimeKind.Utc),
                UserId = Guid.Parse("e42b5f8a-0b29-4b3e-83f8-cf3f284e9a1d"), // Jane Lee
                AssignedToId = Guid.Parse("b7c4e39d-fc29-4783-8c5a-53f7b5f8dca1"), // Kevin Taylor
                Status = "overdue",
                Location = new Point(15.4321, 47.0683) { SRID = 4326 }
            }
        };
        foreach (var todo in todos)
        {
            // Generate between 1 and 5 attachments for each Todo
            var attachmentCount = random.Next(1, 6);
            for (var i = 0; i < attachmentCount; i++)
            {
                // Select a random file name and content type
                var fileName = fileNames[random.Next(fileNames.Count)];
                var contentType = contentTypes[random.Next(contentTypes.Count)];

                // Generate random data to simulate file content
                var data = new byte[random.Next(10, 50)]; // between 1 KB and 50 KB
                random.NextBytes(data);

                // Calculate file size based on data length
                long size = data.Length;

                // Create an attachment
                var attachment = new Attachment
                {
                    Id = Guid.NewGuid(),
                    Name = $"{fileName.Split(".")[0]}_{i}",
                    FileName = fileName,
                    ContentType = contentType,
                    Data = data,
                    Size = size,
                    TodoId = todo.Id,
                    UploadedById = todo.AssignedToId,
                    CreatedUtc = today.AddMinutes(-random.Next(0, 10080)), // Randomly set within the last week
                    UpdatedUtc = today
                };

                // Add the attachment to the list
                attachments.Add(attachment);
            }
        }

// Output for each Todo with Attachments
        var sb = new StringBuilder();
        sb.Append("var attachments = new List<Attachment>()\n{\n");
        foreach (var attachment in attachments)
        {
            sb.Append("\tnew ()\n\t{\n");
            sb.Append($"\t\tName =\"{attachment.Name}\",\n");
            sb.Append($"\t\tId = Guid.Parse(\"{attachment.Id}\"),\n");
            sb.Append($"\t\tFileName = \"{attachment.FileName}\",\n");
            sb.Append($"\t\tContentType = \"{attachment.ContentType}\",\n");
            sb.Append($"\t\tData = [ {string.Join(", ", attachment.Data)}],\n");
            sb.Append($"\t\tSize = {attachment.Size},\n");
            sb.Append($"\t\tTodoId = Guid.Parse(\"{attachment.TodoId}\"),\n");
            sb.Append($"\t\tUploadedById = Guid.Parse(\"{attachment.UploadedById}\"),\n");
            sb.Append(
                $"\t\tCreatedUtc = DateTime.SpecifyKind(DateTime.Parse(\"{attachment.CreatedUtc}\", null, DateTimeStyles.AssumeUniversal), DateTimeKind.Utc),\n");

            sb.Append(
                $"\t\tUpdatedUtc = DateTime.SpecifyKind(DateTime.Parse(\"{attachment.UpdatedUtc}\", null, DateTimeStyles.AssumeUniversal), DateTimeKind.Utc),\n");

            sb.Append("\t},\n");
        }

        sb.Append("};");
        File.WriteAllText("attachments.txt", sb.ToString());
    }
}