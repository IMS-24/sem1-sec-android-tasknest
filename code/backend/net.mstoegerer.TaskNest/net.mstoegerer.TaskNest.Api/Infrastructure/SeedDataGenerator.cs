using System.Globalization;
using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class SeedDataGenerator
{
    //Attachment
    private readonly List<Attachment> _attachments = new();

    //User specific
    private readonly List<string> _cities =
    [
        "Graz", "Seiersberg", "Raaba", "Feldkirchen bei Graz", "Hart bei Graz", "Grambach", "Gössendorf",
        "Kalsdorf bei Graz", "Stattegg", "Thal", "Hausmannstätten", "Gratwein-Straßengel", "Pirka"
    ];


    //attachments
    private readonly Dictionary<string, string> _contentTypes = new()
    {
        { "pdf", "application/pdf" },
        { "jpg", "image/jpeg" },
        { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { "svg", "image/svg+xml" },
        { "png", "image/png" }
    };

    private readonly Dictionary<string, string> _files = new()
    {
        { "document_01", "pdf" },
        { "photo_01", "jpg" },
        { "report_01", "docx" },
        { "presentation_01", "pptx" },
        { "spreadsheet_01", "xlsx" },
        { "diagram_01", "svg" },
        { "contract_01", "pdf" },
        { "receipt_01", "jpg" },
        { "manual_01", "pdf" },
        { "blueprint_01", "png" },
        { "invoice_01", "pdf" },
        { "resume_01", "docx" },
        { "slide_01", "pptx" },
        { "ledger_01", "xlsx" },
        { "illustration_01", "svg" },
        { "brochure_01", "pdf" },
        { "portfolio_01", "jpg" },
        { "estimate_01", "pdf" },
        { "blueprint_02", "png" },
        { "design_01", "svg" },
        { "contract_02", "pdf" },
        { "photo_02", "jpg" },
        { "document_02", "pdf" },
        { "chart_01", "png" },
        { "handbook_01", "pdf" },
        { "summary_01", "docx" },
        { "plan_01", "pptx" },
        { "projection_01", "xlsx" },
        { "template_01", "svg" },
        { "guide_01", "pdf" },
        { "evidence_01", "jpg" },
        { "presentation_02", "pptx" },
        { "article_01", "docx" },
        { "receipt_02", "jpg" },
        { "agenda_01", "pdf" },
        { "estimate_02", "xlsx" },
        { "wireframe_01", "svg" },
        { "layout_01", "pdf" },
        { "receipt_03", "jpg" },
        { "workflow_01", "pptx" },
        { "diagram_02", "svg" },
        { "invoice_02", "pdf" },
        { "blueprint_03", "png" },
        { "contract_03", "pdf" },
        { "slide_02", "pptx" },
        { "spreadsheet_02", "xlsx" },
        { "project_01", "docx" },
        { "mockup_01", "svg" },
        { "manual_02", "pdf" },
        { "sketch_01", "jpg" },
        { "catalog_01", "pdf" },
        { "blueprint_04", "png" },
        { "financials_01", "xlsx" },
        { "project_02", "pptx" },
        { "portfolio_02", "jpg" },
        { "summary_02", "docx" },
        { "plan_02", "pptx" },
        { "proposal_01", "pdf" },
        { "analysis_01", "xlsx" },
        { "logo_01", "svg" },
        { "handout_01", "pdf" },
        { "certificate_01", "jpg" },
        { "contract_04", "pdf" },
        { "chart_02", "png" },
        { "presentation_03", "pptx" },
        { "budget_01", "xlsx" },
        { "blueprint_05", "png" },
        { "document_03", "pdf" },
        { "report_02", "docx" },
        { "diagram_03", "svg" },
        { "summary_03", "docx" },
        { "contract_05", "pdf" },
        { "profile_01", "jpg" },
        { "presentation_04", "pptx" },
        { "financials_02", "xlsx" },
        { "plan_03", "pptx" },
        { "portfolio_03", "jpg" },
        { "diagram_04", "svg" },
        { "policy_01", "pdf" },
        { "agenda_02", "docx" },
        { "slide_03", "pptx" },
        { "schedule_01", "xlsx" },
        { "photo_03", "jpg" },
        { "training_01", "pdf" },
        { "blueprint_06", "png" },
        { "document_04", "pdf" },
        { "budget_02", "xlsx" },
        { "plan_04", "pptx" },
        { "instruction_01", "svg" },
        { "report_03", "docx" },
        { "diagram_05", "svg" },
        { "manual_03", "pdf" },
        { "certificate_02", "jpg" },
        { "outline_01", "pdf" },
        { "map_01", "png" },
        { "financials_03", "xlsx" },
        { "summary_04", "docx" },
        { "blueprint_07", "png" },
        { "handbook_02", "pdf" },
        { "photo_04", "jpg" },
        { "presentation_05", "pptx" },
        { "financials_04", "xlsx" },
        { "portfolio_04", "jpg" },
        { "guide_02", "pdf" },
        { "design_02", "svg" },
        { "policy_02", "pdf" },
        { "blueprint_08", "png" },
        { "report_04", "docx" },
        { "training_02", "pdf" },
        { "chart_03", "png" },
        { "project_03", "pptx" },
        { "photo_05", "jpg" },
        { "contract_06", "pdf" },
        { "presentation_06", "pptx" },
        { "outline_02", "pdf" },
        { "summary_05", "docx" },
        { "financials_05", "xlsx" },
        { "blueprint_09", "png" },
        { "article_02", "docx" },
        { "slide_04", "pptx" },
        { "handbook_03", "pdf" },
        { "portfolio_05", "jpg" },
        { "schedule_02", "xlsx" },
        { "diagram_06", "svg" },
        { "policy_03", "pdf" },
        { "guide_03", "pdf" },
        { "map_02", "png" }
    };


    private readonly List<string> _firstNames =
    [
        "Alexander", "Sophie", "Maximilian", "Marie", "Paul", "Hannah", "Lukas", "Lea", "Felix", "Anna",
        "Leon", "Julia", "Jonas", "Laura", "David", "Emma", "Finn", "Lina", "Luca", "Lisa",
        "Tim", "Sarah", "Julian", "Mia", "Tom", "Leonie", "Nico", "Amelie", "Erik", "Lara",
        "Matthias", "Johanna", "Samuel", "Elena", "Simon", "Isabella", "Fabian", "Clara", "Philipp", "Nina",
        "Sebastian", "Charlotte", "Daniel", "Katharina", "Tobias", "Mila", "Jan", "Sabrina", "Florian", "Maria"
    ];

    private readonly Point _graz = new(15.4395, 47.0707) { SRID = 4326 }; // Starting location around Graz

    private readonly List<string> _lastNames =
    [
        "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz", "Hoffmann",
        "Schäfer", "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Braun", "Zimmermann",
        "Hartmann", "Krüger", "Schmid", "Lange", "Schmitt", "Werner", "Schmitz", "Krause", "Meier", "Lehmann",
        "Schmid", "Schütz", "Haas", "Peters", "Lang", "Scholz", "Möller", "König", "Walter", "Kaiser",
        "Huber", "Fuchs", "Vogel", "Stein", "Jung", "Otto", "Sommer", "Seidel", "Heinrich", "Brandt"
    ];

    private readonly List<string> _metadataKeys =
    [
        "device_mode", "sim_carrier", "device_movement", "app_version", "os_version",
        "network_type", "battery_level", "storage_available", "wifi_connected", "bluetooth_status",
        "screen_brightness", "locale", "timezone", "location_accuracy", "user_interaction",
        "background_app_usage", "active_hours", "sleep_pattern", "recent_calls", "last_message_sent"
    ];

    private readonly List<MetaData> _metadataList = new();

    private readonly string _outputDir = "./SeedData/";

    private readonly Random _random = new();

    private readonly DateTime _startDate = DateTime.Parse("2024-10-15T00:00:00Z"); // Starting within the last two weeks

    // Todo Specific
    private readonly Dictionary<string, string> _todoDescription = new()
    {
        { "Frühstück zubereiten", "Ein gesundes Frühstück machen." },
        { "Kaffee kochen", "Den Morgenkaffee genießen." },
        { "Medikamente einnehmen", "An die Tabletten denken." },
        { "Zähne putzen", "Mundhygiene am Morgen." },
        { "Gesicht waschen", "Erfrischen und reinigen." },
        { "Duschen", "Morgendliche Hygiene." },
        { "Frische Kleidung anziehen", "Den Tag starten." },
        { "Haare kämmen", "Haarroutine." },
        { "Termine planen", "Die heutigen Termine durchsehen und planen." },
        { "Emails checken", "E-Mails auf wichtige Nachrichten durchsehen." },
        { "Mittagessen vorbereiten", "Gesundes Mittagessen für unterwegs vorbereiten." },
        { "Auto tanken", "Tank auffüllen für die Woche." },
        { "Telefonate führen", "Wichtige Anrufe erledigen." },
        { "Arbeitsplatz aufräumen", "Ordnung am Arbeitsplatz schaffen." },
        { "Wäsche waschen", "Kleidung waschen und trocknen." },
        { "Müll rausbringen", "Abfall entsorgen." },
        { "Haus putzen", "Die Räume sauber machen." },
        { "Boden wischen", "Fußböden reinigen." },
        { "Staub wischen", "Staub auf Oberflächen entfernen." },
        { "Geschirr abwaschen", "Abendessen-Geschirr reinigen." },
        { "Sport treiben", "Tägliche Fitness-Einheit absolvieren." },
        { "Yoga üben", "Entspannung durch Yoga." },
        { "Bücher lesen", "Ein neues Kapitel im Buch lesen." },
        { "Freunde treffen", "Soziale Kontakte pflegen." },
        { "Filme schauen", "Einen neuen Film ansehen." },
        { "Rechnungen zahlen", "Monatliche Rechnungen begleichen." },
        { "Kleidung sortieren", "Ungebrauchte Kleidung aussortieren." },
        { "Wocheneinkauf machen", "Lebensmittel und Haushaltswaren kaufen." },
        { "Abendessen kochen", "Eine gesunde Mahlzeit vorbereiten." },
        { "Mit Haustier spielen", "Zeit mit dem Haustier verbringen." },
        { "Notizen aufschreiben", "Wichtige Gedanken und Ideen festhalten." },
        { "Kalender aktualisieren", "Termine und Deadlines aktualisieren." },
        { "Musik hören", "Neue Musik entdecken und genießen." },
        { "Zimmerpflanzen gießen", "Pflanzenpflege im Haus." },
        { "Werkzeuge organisieren", "Ordnung in der Werkzeugkiste schaffen." },
        { "Lebenslauf aktualisieren", "Berufliche Unterlagen auf den neuesten Stand bringen." },
        { "Nachbarn helfen", "Nachbarn bei Bedarf unterstützen." },
        { "Spieleabend organisieren", "Ein Abend mit Brettspielen und Freunden." },
        { "Gartenarbeit machen", "Garten auf Vordermann bringen." },
        { "Rasen mähen", "Den Rasen im Garten pflegen." },
        { "Fenster putzen", "Fenster für klare Sicht reinigen." },
        { "Auf Spaziergang gehen", "Frische Luft und Bewegung." },
        { "Bastelprojekte starten", "Ein neues DIY-Projekt beginnen." },
        { "Budget überprüfen", "Monatliche Finanzen kontrollieren." },
        { "Reise planen", "Den nächsten Urlaub vorbereiten." },
        { "Fotografieren üben", "Fotografie-Techniken ausprobieren." },
        { "Backen lernen", "Ein neues Rezept ausprobieren." },
        { "Sprachen lernen", "Neue Vokabeln für Fremdsprache lernen." },
        { "Meditieren", "Tägliche Meditation für inneren Frieden." },
        { "Online-Kurs belegen", "Fähigkeiten erweitern." },
        { "Auto waschen", "Autopflege." },
        { "Rezept für die Woche planen", "Essensplanung für die kommenden Tage." },
        { "Haus dekorieren", "Neues Flair im Haus schaffen." },
        { "Badezimmer reinigen", "Badezimmer gründlich putzen." },
        { "Werkstatt aufräumen", "Werkstatt organisieren." },
        { "Akten sortieren", "Wichtige Dokumente ordnen." },
        { "Schlafen gehen", "Genügend Schlaf für Erholung." },
        { "Tee trinken", "Entspannende Teepause einlegen." },
        { "Schulaufgaben helfen", "Kindern bei ihren Hausaufgaben helfen." },
        { "Urlaub buchen", "Den nächsten Urlaub fest buchen." },
        { "Batterien wechseln", "Batterien in Fernbedienungen erneuern." },
        { "Vorräte aufstocken", "Haushaltsbedarf überprüfen und auffüllen." },
        { "Kreativ schreiben", "Eigene Gedanken oder Geschichten verfassen." },
        { "Computer aufräumen", "Dateien sortieren und unnötiges löschen." },
        { "Kleiderschrank ausmisten", "Ungetragene Kleidung aussortieren." },
        { "Zahnarzttermin machen", "Jährliche Kontrolle einplanen." },
        { "Täglich spazieren gehen", "Kleine Runden draußen machen." },
        { "Pflanzen umtopfen", "Pflanzen in größere Töpfe setzen." },
        { "Neuen Podcast hören", "Wissen erweitern durch Podcasts." },
        { "Lustige Videos anschauen", "Lachen und entspannen." },
        { "Neue Rezepte ausprobieren", "Kulinarisch experimentieren." },
        { "Fitness-App nutzen", "Training und Fortschritte verfolgen." },
        { "Abendmeditation", "Tag ruhig ausklingen lassen." },
        { "Geschenk vorbereiten", "Freude für jemand anderen schaffen." },
        { "Einkaufslisten schreiben", "Organisiert und gezielt einkaufen." },
        { "Neues lernen", "Täglich neues Wissen erlangen." },
        { "Newsletter abonnieren", "Aktuelle Infos zu Themen erhalten." },
        { "Nachrichten lesen", "Über aktuelle Geschehnisse informiert bleiben." },
        { "Zeit mit Familie verbringen", "Familienbeziehung stärken." },
        { "Fenster dekorieren", "Für eine gemütliche Atmosphäre sorgen." },
        { "Neue Orte entdecken", "Ein Tagesausflug an neue Orte." },
        { "Energie sparen", "Effizient im Haushalt sein." },
        { "Notfall-Set überprüfen", "Für alle Fälle vorbereitet sein." },
        { "Journaling betreiben", "Gedanken schriftlich festhalten." },
        { "Haushaltsgeräte warten", "Geräte überprüfen und reinigen." },
        { "Komplimente machen", "Freundlichkeit verbreiten." },
        { "Verhandlungsfähigkeiten üben", "Verhandeln üben." },
        { "Online-Bewertungen schreiben", "Gute Produkte oder Dienste bewerten." },
        { "Malkurs besuchen", "Künstlerische Fähigkeiten entwickeln." },
        { "Tanz üben", "Tanzen lernen oder verbessern." },
        { "Haustier baden", "Pflege für Haustiere." },
        { "Berichte lesen", "Berufliche Dokumente studieren." },
        { "Denkmuster reflektieren", "Achtsamkeit und Selbstwahrnehmung." },
        { "Mindmapping betreiben", "Ideen visuell darstellen." },
        { "Social Media Zeit begrenzen", "Digital Balance." },
        { "Fahrrad reparieren", "Fahrrad instand setzen." },
        { "Lauftechnik verbessern", "Effizienter Laufen lernen." },
        { "Technologie-Zeit einschränken", "Mehr Zeit offline." },
        { "Hochwertige Inhalte konsumieren", "Bildende Medien und Inhalte auswählen." },
        { "Pflanzen anbauen", "Eigene Kräuter und Gemüse pflanzen." },
        { "Kreativität fördern", "Künstlerisch betätigen und inspirieren lassen." },
        { "Zeit für Selbstpflege", "Regelmäßige Auszeit zur Erholung." }
    };

    private readonly List<Todo> _todos = new();
    private readonly List<TodoShare> _todoShares = new();
    private readonly IList<UserMetaData> _userMetaDataList = new List<UserMetaData>();
    private readonly List<User> _users = new();

    private string GetValueForMetadataKey(string key)
    {
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
        return value;
    }

    private static StringBuilder AppendLocation(StringBuilder sqlBuilder, Point? location)
    {
        if (location != null)
            sqlBuilder.Append(
                $"ST_SetSRID(ST_MakePoint({location.X.ToString(CultureInfo.InvariantCulture)}, {location.Y.ToString(CultureInfo.InvariantCulture)}), {location.SRID}), ");
        else
            sqlBuilder.Append("NULL, "); // Handle null location
        return sqlBuilder;
    }

    private void WriteSqlStatement(StringBuilder sqlBuilder, string fileName)
    {
        sqlBuilder.Replace(",\n;", ";");
        var outputDir = Path.Combine(_outputDir, fileName);
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);
        File.WriteAllText(outputDir + ".sql", sqlBuilder.ToString());
    }

    public void GenerateUsers()
    {
        var users = (from firstName in _firstNames
            let lName = _lastNames[_random.Next(0, _lastNames.Count)]
            let city = _cities[_random.Next(0, _cities.Count)]
            let id = Guid.NewGuid()
            let email = $"{firstName}.{lName}@{city}.com".ToLower().Replace(" ", "_")
            let createdUtc = DateTime.UtcNow.AddMinutes(_random.Next(-60 * 24 * 30, 60 * 24 * 30))
            let updatedUtc = createdUtc.AddMinutes(_random.Next(0, 60 * 24 * 15))
            select new User
            {
                Id = id,
                CreatedUtc = createdUtc,
                UpdatedUtc = updatedUtc,
                Name = $"{firstName} {lName}",
                Email = email
            }).ToList();
        users.Add(new User
        {
            CreatedUtc = new DateTime(01, 01, 01).ToUniversalTime(),
            UpdatedUtc = new DateTime(01, 01, 01).ToUniversalTime(),
            Email = "pepe@mstoegerer.net",
            Id = Guid.NewGuid(),
            Name = "Lord Pepe",
            ExternalId = "auth0|6750c29642a151293be3cb5f"
        });
        users.Add(new User
        {
            CreatedUtc = new DateTime(01, 01, 01).ToUniversalTime(),
            UpdatedUtc = new DateTime(01, 01, 01).ToUniversalTime(),
            Email = "alois.vollm@gmail.com",
            Id = Guid.NewGuid(),
            Name = "AIlois",
            ExternalId = "google-oauth2|110065317277204435904"
        });

        //output
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO public.user (id, name, email, created_utc, updated_utc, external_id) VALUES ");
        foreach (var user in users)
        {
            _users.Add(user);
            sqlBuilder.Append(
                $"('{user.Id}', " +
                $"'{user.Name}', " +
                $"'{user.Email}', " +
                $"'{user.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}', " +
                $"'{user.UpdatedUtc:yyyy-MM-dd HH:mm:ssZ}'," +
                $"'{user.ExternalId}'),\n");
        }

        sqlBuilder.Append(";");
        sqlBuilder.Replace(",\n;", ";");
        WriteSqlStatement(sqlBuilder, "users");
    }

    public void GenerateTodos()
    {
        foreach (var user in _users)
        {
            var currentLong = _graz.X + (_random.NextDouble() - 0.5) * 0.003;
            var currentLat = _graz.Y + (_random.NextDouble() - 0.5) * 0.003;
            for (var i = 0; i < new Random().Next(0, 200); i++)
            {
                (currentLong, currentLat) = GetRandomPointWithinRadius(currentLong, currentLat, 2.75);
                var location = new Point(currentLong, currentLat) { SRID = 4326 };
                var (title, description) = _todoDescription.ElementAt(_random.Next(0, _todoDescription.Count));
                var randomUser = _users[_random.Next(0, _users.Count)];
                while (randomUser.Id == user.Id)
                    randomUser = _users[_random.Next(0, _users.Count)];
                _todos.Add(new Todo
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Location = location,
                    Title = title,
                    Content = description,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow,
                    DueUtc = DateTime.UtcNow.AddMinutes(new Random().Next(1, 60 * 24 * 30)),
                    AssignedToId = _random.Next(0, 100) < 50 ? user.Id : randomUser.Id
                });
            }
        }


        //output
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "INSERT INTO Todo (id, user_id, location, title, content, created_utc, updated_utc, due_utc, assigned_to_id) VALUES\n");
        var listBuilder = new StringBuilder();
        foreach (var todo in _todos)
        {
            sqlBuilder.Append($"('{todo.Id}', ");
            sqlBuilder.Append($"'{todo.UserId}', ");
            sqlBuilder = AppendLocation(sqlBuilder, todo.Location);
            sqlBuilder.Append($"'{todo.Title}', ");
            sqlBuilder.Append($"'{todo.Content}', ");
            sqlBuilder.Append($"'{todo.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}', ");
            sqlBuilder.Append($"'{todo.UpdatedUtc:yyyy-MM-dd HH:mm:ssZ}', ");
            sqlBuilder.Append($"'{todo.DueUtc:yyyy-MM-dd HH:mm:ssZ}', ");
            sqlBuilder.Append($"'{todo.AssignedToId}'),\n");
        }

        listBuilder.Append("};");
        sqlBuilder.Append(";");
        WriteSqlStatement(sqlBuilder, "todos");
    }

    private IList<Guid> getRandomUserList(int amount)
    {
        var randomUserList = new List<Guid>();
        for (var i = 0; i < amount; i++)
        {
            var randomUser = _users[_random.Next(0, _users.Count)];
            while (randomUserList.Contains(randomUser.Id))
                randomUser = _users[_random.Next(0, _users.Count)];
            randomUserList.Add(randomUser.Id);
        }

        return randomUserList;
    }

    public void GenerateTodoShares()
    {
        foreach (var todo in _todos)
        {
            var sharedById = todo.UserId;
            var shareRnd = _random.Next(0, 5);
            var sharedWithUsers = getRandomUserList(shareRnd);
            foreach (var sharedWithUser in sharedWithUsers)
            {
                var share = new TodoShare
                {
                    Id = Guid.NewGuid(),
                    TodoId = todo.Id,
                    SharedById = sharedById,
                    SharedWithId = sharedWithUser,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                _todoShares.Add(share);
            }
        }
        //Output

        var sqlBuilder = new StringBuilder();
        sqlBuilder.AppendLine(
            "INSERT INTO todo_share (id, todo_id, shared_by_id, shared_with_id, created_utc, updated_utc) VALUES ");
        foreach (var share in _todoShares)
            sqlBuilder.AppendLine($"('{share.Id}', " +
                                  $"'{share.TodoId}', " +
                                  $"'{share.SharedById}', " +
                                  $"'{share.SharedWithId}', " +
                                  $"'{share.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}', " +
                                  $"'{share.UpdatedUtc:yyyy-MM-dd HH:mm:ssZ}'),");
        sqlBuilder.AppendLine(";");
        WriteSqlStatement(sqlBuilder, "todo_shares");
    }

    public void GenerateAttachments()
    {
        foreach (var todo in _todos)
        {
            var attachmentCount = _random.Next(0, 6);
            for (var i = 0; i < attachmentCount; i++)
            {
                var (fileName, extension) = _files.ElementAt(_random.Next(_files.Count));
                var contentType = _contentTypes[extension];
                var data = new byte[_random.Next(10, 50)]; // between 1 KB and 50 KB
                _random.NextBytes(data);
                long size = data.Length;
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
                    CreatedUtc =
                        DateTime.UtcNow.AddMinutes(-_random.Next(0, 60 * 24 * 7)), // Randomly set within the last week
                    UpdatedUtc = DateTime.UtcNow
                };
                _attachments.Add(attachment);
            }
        }

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "INSERT INTO attachment (id, name, file_name, content_type, data, size, todo_id, uploaded_by_id, created_utc, updated_utc) VALUES ");
        foreach (var attachment in _attachments)
        {
            //sql
            sqlBuilder.Append($"('{attachment.Id}', ");
            sqlBuilder.Append($"'{attachment.Name}', ");
            sqlBuilder.Append($"'{attachment.FileName}', ");
            sqlBuilder.Append($"'{attachment.ContentType}', ");
            sqlBuilder.Append($"'{attachment.Data}', ");
            sqlBuilder.Append($"'{attachment.Size}', ");
            sqlBuilder.Append($"'{attachment.TodoId}', ");
            sqlBuilder.Append($"'{attachment.UploadedById}', ");
            sqlBuilder.Append($"'{attachment.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}', ");
            sqlBuilder.Append($"'{attachment.UpdatedUtc:yyyy-MM-dd HH:mm:ssZ}'),\n");
        }

        sqlBuilder.Append(";");
        WriteSqlStatement(sqlBuilder, "attachments");
    }

    private (double Longitude, double Latitude) GetRandomPointWithinRadius(double currentLongitude,
        double currentLatitude, double radius)
    {
        /*// Generate a random distance from the center point within the specified radius

        var distance = _random.NextDouble() * radius;

        // Generate a random angle in radians
        var angle = _random.NextDouble() * 2 * Math.PI;

        // Calculate the offset for x and y based on the random distance and angle
        var offsetX = distance * Math.Cos(angle);
        var offsetY = distance * Math.Sin(angle);

        // Create a new point within the radius around the center point
        var newX = currentLongitude + offsetX; // Longitude
        var newY = currentLatitude + offsetY; // Latitude

        return (newY, newX);*/
        // Radius of the Earth in meters
        const double EarthRadius = 6378137;

        // Convert radius from meters to degrees
        var radiusInDegrees = radius / EarthRadius * (180 / Math.PI);

        // Generate a random distance within the specified radius in degrees
        var distance = _random.NextDouble() * radiusInDegrees;

        // Generate a random angle in radians
        var angle = _random.NextDouble() * 2 * Math.PI;

        // Calculate offset in latitude and longitude
        var offsetLatitude = distance * Math.Cos(angle);
        var offsetLongitude = distance * Math.Sin(angle) / Math.Cos(currentLatitude * Math.PI / 180);

        // New point coordinates
        var newLatitude = currentLatitude + offsetLatitude; // Latitude
        var newLongitude = currentLongitude + offsetLongitude; // Longitude

        return (newLongitude, newLatitude);
    }

    public void GenerateUserMetaData()
    {
        foreach (var user in _users)
        {
            var currentLong = _graz.X + (_random.NextDouble() - 0.5) * 0.003;
            var currentLat = _graz.Y + (_random.NextDouble() - 0.5) * 0.003;
            var currentTimeStamp = DateTime.UtcNow.AddHours(-24 * 7 * _random.Next(15, 50));
            for (var i = 0; i < _random.Next(0, 1500); i++)
            {
                (currentLong, currentLat) = GetRandomPointWithinRadius(currentLong, currentLat, 10);
                var location = new Point(currentLong, currentLat) { SRID = 4326 };
                var userMetaData = new UserMetaData
                {
                    CreatedUtc = currentTimeStamp,
                    Id = Guid.NewGuid(),
                    Location = location,
                    UserId = user.Id
                };
                currentTimeStamp = currentTimeStamp.AddMinutes(5);
                _userMetaDataList.Add(userMetaData);
            }
        }

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "INSERT INTO user_metadata (id,location, user_id, created_utc) VALUES\n");

        foreach (var userMetaData in _userMetaDataList)
        {
            sqlBuilder.Append($"('{userMetaData.Id}', ");
            sqlBuilder = AppendLocation(sqlBuilder, userMetaData.Location);
            sqlBuilder.Append($"'{userMetaData.UserId}', ");
            sqlBuilder.Append($"'{userMetaData.CreatedUtc:yyyy-MM-dd HH:mm:ssZ}'");
            sqlBuilder.Append("),\n");
        }

        sqlBuilder.Append(";");

        WriteSqlStatement(sqlBuilder, "user_metadata");
    }

    public void GenerateMetaData()
    {
        foreach (var userMetaData in _userMetaDataList)
            for (var i = 0; i < _random.Next(5, 51); i++)
            {
                var key = _metadataKeys[_random.Next(_metadataKeys.Count)];
                var value = GetValueForMetadataKey(key);
                var metaDataEntry = new MetaData
                {
                    Id = Guid.NewGuid(),
                    Key = key,
                    Value = value,
                    UserMetaDataId = userMetaData.Id
                };
                _metadataList.Add(metaDataEntry);
            }

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "INSERT INTO meta_data (id, key, value, user_meta_data_id) VALUES\n");
        foreach (var metaData in _metadataList)
        {
            sqlBuilder.Append($"('{metaData.Id}', ");
            sqlBuilder.Append($"'{metaData.Key}', ");
            sqlBuilder.Append($"'{metaData.Value}', ");
            sqlBuilder.Append($"'{metaData.UserMetaDataId}'),\n");
        }

        sqlBuilder.Append(";");

        WriteSqlStatement(sqlBuilder, "meta_data");
    }
}