using System.Globalization;
using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class MetadataGenerator
{
    private readonly List<string> _metadataKeys =
    [
        "device_mode", "sim_carrier", "device_movement", "app_version", "os_version",
        "network_type", "battery_level", "storage_available", "wifi_connected", "bluetooth_status",
        "screen_brightness", "locale", "timezone", "location_accuracy", "user_interaction",
        "background_app_usage", "active_hours", "sleep_pattern", "recent_calls", "last_message_sent"
    ];

    private readonly Random _random = new();
    private readonly DateTime _startDate = DateTime.Parse("2024-10-15T00:00:00Z"); // Starting within the last two weeks
    private readonly IList<UserMetaData> _userMetaDataList = new List<UserMetaData>();

    private readonly IList<User> _users = new List<User>
    {
        new()
        {
            Id = Guid.Parse("a13d07f3-1c91-4d2c-9296-ff8d7b7fabc3"),
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            Password = "S8k@xZ!Q9",
            CreatedUtc = new DateTime(2024, 10, 01, 14, 35, 20, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 05, 16, 22, 13, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("bf98b232-33f5-4a29-8dff-78f04b7d48c9"),
            Name = "Bob Smith",
            Email = "bob.smith@example.com",
            Password = "P3y@j!B1l",
            CreatedUtc = new DateTime(2024, 11, 10, 09, 47, 32, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 11, 12, 11, 15, 20, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("d4c95f9b-a62e-4f32-b8ab-92f832ad0f8a"),
            Name = "Charlie Brown",
            Email = "charlie.brown@example.com",
            Password = "G7a#zX5v@",
            CreatedUtc = new DateTime(2024, 09, 22, 12, 04, 56, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 09, 25, 13, 10, 45, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("b04e9a4d-f329-4a3a-8d3e-08f6b492b48c"),
            Name = "Diana Garcia",
            Email = "diana.garcia@example.com",
            Password = "X9w$L3p!Y",
            CreatedUtc = new DateTime(2024, 10, 12, 08, 15, 24, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 18, 15, 40, 32, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("2f9b6d1d-6f9a-467a-81b4-e6b67473b3e5"),
            Name = "Evan Miller",
            Email = "evan.miller@example.com",
            Password = "H5q@rT!2g",
            CreatedUtc = new DateTime(2024, 09, 28, 11, 25, 42, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 02, 13, 55, 22, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("73c64fd9-0d15-4f82-8db6-c329ba47a7b2"),
            Name = "Fiona Davis",
            Email = "fiona.davis@example.com",
            Password = "V2z#Y7j!L",
            CreatedUtc = new DateTime(2024, 11, 01, 14, 30, 17, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 11, 03, 16, 50, 45, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("c07e5d2e-916f-4a68-bb7f-5c87b326d83d"),
            Name = "George Martinez",
            Email = "george.martinez@example.com",
            Password = "W4p@X5m#J",
            CreatedUtc = new DateTime(2024, 10, 20, 12, 40, 35, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 25, 14, 20, 10, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("4a18b99d-95f3-45a8-81f7-8b47368fae32"),
            Name = "Hannah Wilson",
            Email = "hannah.wilson@example.com",
            Password = "M6s#C8k@H",
            CreatedUtc = new DateTime(2024, 09, 27, 10, 55, 48, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 01, 13, 30, 22, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("925e8b36-b2c5-41e4-8a5c-58c967e5dfd5"),
            Name = "Ian Anderson",
            Email = "ian.anderson@example.com",
            Password = "K8w!zN7#B",
            CreatedUtc = new DateTime(2024, 10, 05, 09, 15, 23, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 08, 10, 25, 42, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("e42b5f8a-0b29-4b3e-83f8-cf3f284e9a1d"),
            Name = "Jane Lee",
            Email = "jane.lee@example.com",
            Password = "A9x@L6y$W",
            CreatedUtc = new DateTime(2024, 10, 29, 08, 05, 35, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 31, 09, 10, 17, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("b7c4e39d-fc29-4783-8c5a-53f7b5f8dca1"),
            Name = "Kevin Taylor",
            Email = "kevin.taylor@example.com",
            Password = "P3v#M7x!D",
            CreatedUtc = new DateTime(2024, 09, 26, 07, 20, 45, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 09, 30, 12, 00, 50, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("c4f8a7d1-d3a7-45b5-9157-9e74b5f7c9a5"),
            Name = "Laura Harris",
            Email = "laura.harris@example.com",
            Password = "T9j@F2m#H",
            CreatedUtc = new DateTime(2024, 10, 11, 10, 45, 12, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 15, 11, 05, 32, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("ae25d7e9-4d3f-4a9e-85d7-7e8c5b4f92a3"),
            Name = "Michael Clark",
            Email = "michael.clark@example.com",
            Password = "Z2k@R7p!G",
            CreatedUtc = new DateTime(2024, 10, 09, 12, 15, 28, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 12, 14, 35, 50, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("3e7b9c58-3e2d-4534-8a9c-5b1c7f8b45e3"),
            Name = "Nina Lewis",
            Email = "nina.lewis@example.com",
            Password = "B5y#M8w!V",
            CreatedUtc = new DateTime(2024, 09, 21, 09, 55, 43, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 09, 26, 12, 10, 17, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("c3f8d2a7-5c3f-4d5e-95c7-3a9f7e5c98b7"),
            Name = "Oliver King",
            Email = "oliver.king@example.com",
            Password = "D8l!N5q@F",
            CreatedUtc = new DateTime(2024, 10, 03, 14, 20, 33, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 08, 16, 30, 18, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("de4b7f5c-8a5d-4b2a-9c4b-5d3f7a9e8b2f"),
            Name = "Paula Hall",
            Email = "paula.hall@example.com",
            Password = "J9r@X7b!L",
            CreatedUtc = new DateTime(2024, 11, 03, 08, 30, 45, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 11, 06, 09, 40, 30, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("5e8a7f9b-4c7e-4d2f-85b7-7e5b9a3c2f1d"),
            Name = "Quinn Scott",
            Email = "quinn.scott@example.com",
            Password = "F3p#V9k!S",
            CreatedUtc = new DateTime(2024, 09, 24, 07, 05, 17, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 09, 28, 09, 50, 12, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("f9b7c5e8-3d9a-4a7f-8c2b-5a7e9f8d2c3a"),
            Name = "Rachel Adams",
            Email = "rachel.adams@example.com",
            Password = "L8y!G5p@H",
            CreatedUtc = new DateTime(2024, 10, 18, 13, 15, 23, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 21, 14, 25, 45, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("8b5f3d2a-7c9a-4e7f-8c3b-2d7f5e9a4b7c"),
            Name = "Sam Walker",
            Email = "sam.walker@example.com",
            Password = "M2r@V8l!N",
            CreatedUtc = new DateTime(2024, 11, 05, 15, 35, 12, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 11, 07, 16, 40, 30, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = Guid.Parse("7e5c9d3b-2a4e-4f5d-8a3c-9f8b7d5e3c2f"),
            Name = "Tina Young",
            Email = "tina.young@example.com",
            Password = "K7x!L4p@H",
            CreatedUtc = new DateTime(2024, 10, 25, 10, 45, 38, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2024, 10, 28, 12, 55, 12, 0, DateTimeKind.Utc)
        }
    };

    public MetadataGenerator()
    {
        GenerateUserMetaData();
    }

    private void GenerateUserMetaData()
    {
        for (var j = 0; j < 500; j++)
            foreach (var user in _users)
            {
                // Generate UserMetaData entry for each User
                var userMetaData = new UserMetaData
                {
                    Id = Guid.NewGuid(),
                    Password = user.Password,
                    PhoneNumber = $"+43{_random.Next(100000000, 999999999)}", // Austrian-style random number
                    UserId = user.Id,
                    CreatedUtc = _startDate,
                    Location = new Point(15.4395, 47.0707) { SRID = 4326 }, // Starting location around Graz
                    MetaData = new List<MetaData>()
                };

                // Generate between 5 and 50 metadata entries for this user
                var metadataCount = _random.Next(5, 51);
                var currentTimestamp = _startDate;
                var currentLatitude = 47.0707;
                var currentLongitude = 15.4395;

                for (var i = 0; i < metadataCount; i++)
                {
                    // Select a random metadata key and value
                    var key = _metadataKeys[_random.Next(_metadataKeys.Count)];
                    var value = key switch
                    {
                        "device_mode" => _random.Next(2) == 0 ? "portrait" : "landscape",
                        "sim_carrier" => _random.Next(2) == 0 ? "A1" : "T-Mobile",
                        "device_movement" => _random.Next(2) == 0 ? "walking" : "driving",
                        "app_version" => $"v{_random.Next(1, 5)}.{_random.Next(0, 10)}.{_random.Next(0, 10)}",
                        "os_version" => $"Android {_random.Next(8, 13)}",
                        "network_type" => _random.Next(2) == 0 ? "WiFi" : "4G",
                        "battery_level" => $"{_random.Next(10, 100)}%",
                        "storage_available" => $"{_random.Next(100, 50000)}MB",
                        "wifi_connected" => _random.Next(2) == 0 ? "true" : "false",
                        "bluetooth_status" => _random.Next(2) == 0 ? "enabled" : "disabled",
                        "screen_brightness" => $"{_random.Next(0, 100)}%",
                        "locale" => _random.Next(2) == 0 ? "de_AT" : "en_US",
                        "timezone" => _random.Next(2) == 0 ? "CET" : "CEST",
                        "location_accuracy" => $"{_random.Next(5, 20)}m",
                        "user_interaction" => _random.Next(2) == 0 ? "active" : "inactive",
                        "background_app_usage" => $"{_random.Next(0, 300)} mins",
                        "active_hours" => $"{_random.Next(0, 24)}h",
                        "sleep_pattern" => $"{_random.Next(4, 8)}h",
                        "recent_calls" => $"{_random.Next(0, 20)} calls",
                        "last_message_sent" => DateTime.UtcNow.AddMinutes(-_random.Next(0, 1440))
                            .ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        _ => "unknown"
                    };

                    // Randomly adjust coordinates slightly for each timestamp to simulate movement within Graz
                    currentLatitude += (_random.NextDouble() - 0.5) * 0.003;
                    currentLongitude += (_random.NextDouble() - 0.5) * 0.003;
                    var location = new Point(currentLongitude, currentLatitude) { SRID = 4326 };

                    // Create a MetaData entry
                    var metaDataEntry = new MetaData
                    {
                        Id = Guid.NewGuid(),
                        Key = key,
                        Value = value
                    };

                    // Assign metadata and update timestamps
                    userMetaData.MetaData.Add(metaDataEntry);
                    userMetaData.Location = location;
                    userMetaData.CreatedUtc = currentTimestamp;

                    // Move to the next 5-minute interval
                    currentTimestamp = currentTimestamp.AddMinutes(5);
                }

                // Add UserMetaData with all metadata entries to the list
                _userMetaDataList.Add(userMetaData);
            }

        // Output
        var userMetaSqlSb = new StringBuilder();
        var metaSqlSb = new StringBuilder();
        var userMetaDataListSb = new StringBuilder();
        var metaDataListSb = new StringBuilder();
        userMetaDataListSb.Append("var userMetadataList = new List<UserMetaData>\n{\n");
        metaDataListSb.Append("var metadataList = new List<MetaData>\n{\n");
        userMetaSqlSb.Append("INSERT INTO user_metadata(id,password, phone_number, user_id, created_utc, location)\n" +
                             "VALUES \n");
        metaSqlSb.Append("INSERT INTO meta_data (id, user_meta_data_id, key,value)\n" +
                         "VALUES \n");
        foreach (var userMeta in _userMetaDataList)
        {
            /*userMetaSqlSb.Append(
                $"(\"{userMeta.Id}\", \"{userMeta.Password}\", \"{userMeta.PhoneNumber}\", {userMeta.UserId}, {userMeta.CreatedUtc}, {userMeta.Location}),\n");
                */
            userMetaSqlSb.Append($"('{userMeta.Id}', ");
            userMetaSqlSb.Append($"'{userMeta.Password}', ");
            userMetaSqlSb.Append($"'{userMeta.PhoneNumber}', ");
            userMetaSqlSb.Append($"'{userMeta.UserId}', ");
            userMetaSqlSb.Append($"'{userMeta.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}', "); // UTC timestamp with timezone format

            // Check if Location is set and add it to SQL
            if (userMeta.Location != null)
            {
                userMetaSqlSb.Append($"ST_SetSRID(ST_MakePoint({userMeta.Location.X}, {userMeta.Location.Y}), {userMeta.Location.SRID})");
            }
            else
            {
                userMetaSqlSb.Append("NULL"); // Handle null location
            }

            userMetaSqlSb.Append("),\n");
            
            userMetaDataListSb.Append("\tnew ()\n    {\n");
            userMetaDataListSb.Append($"\t\tId = Guid.Parse(\"{userMeta.Id}\"),\n");
            userMetaDataListSb.Append($"\t\tUserId = Guid.Parse(\"{userMeta.UserId}\"),\n");
            userMetaDataListSb.Append(
                $"\t\tCreatedUtc = DateTime.Parse(\"{userMeta.CreatedUtc}\", null, DateTimeStyles.AssumeUniversal).ToUniversalTime(),\n");
            userMetaDataListSb.Append($"\t\tPhoneNumber = \"{userMeta.PhoneNumber}\",\n");
            if (userMeta.Location != null)
                userMetaDataListSb.Append(
                    $"\t\tLocation = new Point({userMeta.Location.X.ToString(CultureInfo.InvariantCulture)}, {userMeta.Location.Y}) {{ SRID = 4326 }},\n");

            metaDataListSb.Append($"\t/*'{userMeta.Id}'*/\n");
            foreach (var meta in userMeta.MetaData)
            {
                metaSqlSb.Append("(");
                metaSqlSb.Append($"'{meta.Id}', ");
                metaSqlSb.Append($"'{userMeta.Id}', ");
                metaSqlSb.Append($"'{meta.Key}', ");
                metaSqlSb.Append($"'{meta.Value}'");
                metaSqlSb.Append("),\n");
                metaDataListSb.Append("\tnew ()\n\t{\n");
                metaDataListSb.Append($"\t\tId = Guid.Parse(\"{meta.Id}\"),\n");
                metaDataListSb.Append($"\t\tUserMetaDataId = Guid.Parse(\"{userMeta.Id}\"),\n");
                metaDataListSb.Append($"\t\tKey = \"{meta.Key}\",\n");
                metaDataListSb.Append($"\t\tValue = \"{meta.Value}\"\n\t}},\n");
            } //end foreach userMeta.MetaData

            userMetaDataListSb.Append("\t},\n");

//TODO: FIX ME
            //metaDataListSb.Append("},\n");
        } //end foreach userMetaDatalist

        userMetaDataListSb.Append("};");
        metaDataListSb.Append("};");
        File.WriteAllText("metadata.txt", metaDataListSb.ToString());
        File.WriteAllText("usermetadata.txt", userMetaDataListSb.ToString());
        File.WriteAllText("metadata.sql", metaSqlSb.ToString());
        File.WriteAllText("usermetadata.sql", userMetaSqlSb.ToString());
    }
}