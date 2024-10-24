using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class TodoGenerator
{
    private readonly Random _random = new();

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

    private readonly List<User> _users =
    [
        new()
        {
            Id = Guid.Parse("971ef1ef-01d4-42c2-8488-20c2e833bdae"),
            Name = "Alexander Schmid",
            Email = "alexander.schmid@hausmannstätten.com",
            Password = "AlexanderSchmid123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("7f0baf3d-8092-4df4-913a-bb8e09f73f79"),
            Name = "Sophie Fuchs",
            Email = "sophie.fuchs@stattegg.com",
            Password = "SophieFuchs123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("0d17acdd-84a3-4320-9848-e85dd0277c43"),
            Name = "Maximilian Fischer",
            Email = "maximilian.fischer@stattegg.com",
            Password = "MaximilianFischer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("0f9ba40a-6aab-4db6-ab07-e0f898557280"),
            Name = "Marie Lang",
            Email = "marie.lang@hart_bei_graz.com",
            Password = "MarieLang123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("a63023a1-7973-494f-922d-40d83d080222"),
            Name = "Paul Sommer",
            Email = "paul.sommer@thal.com",
            Password = "PaulSommer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("27f3749a-f7fc-4ce2-948e-b0712de7c726"),
            Name = "Hannah Scholz",
            Email = "hannah.scholz@seiersberg.com",
            Password = "HannahScholz123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("605d4aee-1c53-43dc-afec-a252f0deb7d2"),
            Name = "Lukas Braun",
            Email = "lukas.braun@graz.com",
            Password = "LukasBraun123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("4d428b23-a114-47b2-9f20-9dcbc0a98304"),
            Name = "Lea Schmitt",
            Email = "lea.schmitt@stattegg.com",
            Password = "LeaSchmitt123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("dd728889-1dd8-48b7-a9df-99fabc3df342"),
            Name = "Felix Sommer",
            Email = "felix.sommer@feldkirchen_bei_graz.com",
            Password = "FelixSommer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("9822adbe-18e7-4577-8aa4-c3a96f6462f2"),
            Name = "Anna Lehmann",
            Email = "anna.lehmann@graz.com",
            Password = "AnnaLehmann123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("6169eed3-018a-4327-9dc8-f3c8895d5bf3"),
            Name = "Leon Walter",
            Email = "leon.walter@gratwein-straßengel.com",
            Password = "LeonWalter123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("7051bbcc-c7a3-4732-b475-abfb31168d5e"),
            Name = "Julia Haas",
            Email = "julia.haas@stattegg.com",
            Password = "JuliaHaas123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("1b967f75-0692-4f7d-b735-fd493860d513"),
            Name = "Jonas Huber",
            Email = "jonas.huber@stattegg.com",
            Password = "JonasHuber123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("14b14ca9-2913-469f-9b2b-43192d3b4bc9"),
            Name = "Laura Schröder",
            Email = "laura.schröder@hausmannstätten.com",
            Password = "LauraSchröder123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("b89e4574-1d2a-445b-9d29-598ef2b85e25"),
            Name = "David Zimmermann",
            Email = "david.zimmermann@feldkirchen_bei_graz.com",
            Password = "DavidZimmermann123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("7cb0ec3f-103b-4ad0-820f-8aff18579727"),
            Name = "Emma Lang",
            Email = "emma.lang@graz.com",
            Password = "EmmaLang123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("28882c3b-59d4-4c50-b60c-7ab67d4ad945"),
            Name = "Finn Meyer",
            Email = "finn.meyer@pirka.com",
            Password = "FinnMeyer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("67113a44-9ba1-4c9e-812d-546c2d150e90"),
            Name = "Lina Schröder",
            Email = "lina.schröder@gössendorf.com",
            Password = "LinaSchröder123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("2fe6cce2-de9f-47b6-8165-35b11beaa9d6"),
            Name = "Luca Weber",
            Email = "luca.weber@feldkirchen_bei_graz.com",
            Password = "LucaWeber123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("8889731e-c990-4416-bf90-e3c8705dd6ef"),
            Name = "Lisa Fuchs",
            Email = "lisa.fuchs@seiersberg.com",
            Password = "LisaFuchs123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("f8bd6986-b73e-4b72-ac9f-4ab0febdad7c"),
            Name = "Tim Bauer",
            Email = "tim.bauer@raaba.com",
            Password = "TimBauer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("291899c3-5950-4c3b-8f4a-c58afb119c2b"),
            Name = "Sarah Klein",
            Email = "sarah.klein@gratwein-straßengel.com",
            Password = "SarahKlein123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("9afa224f-9af1-496a-8936-5dbdcc1f9142"),
            Name = "Julian Richter",
            Email = "julian.richter@gössendorf.com",
            Password = "JulianRichter123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("b26f997e-c862-4b7a-8b41-db19632a07ce"),
            Name = "Mia Lange",
            Email = "mia.lange@raaba.com",
            Password = "MiaLange123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("6cfa35f5-4b07-42f0-9852-9c827a6784b0"),
            Name = "Tom Walter",
            Email = "tom.walter@kalsdorf_bei_graz.com",
            Password = "TomWalter123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("b77c33ef-9670-487c-8c2e-2bf2759c07f1"),
            Name = "Leonie Sommer",
            Email = "leonie.sommer@stattegg.com",
            Password = "LeonieSommer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("dcdb8d32-4e11-4fd9-a895-4a66a921199d"),
            Name = "Nico Schmid",
            Email = "nico.schmid@stattegg.com",
            Password = "NicoSchmid123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("c3e826a2-68a3-46ef-8a77-80ad637e313d"),
            Name = "Amelie Kaiser",
            Email = "amelie.kaiser@seiersberg.com",
            Password = "AmelieKaiser123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("f5a186a6-f05c-4811-b1ac-917e3704aaca"),
            Name = "Erik Bauer",
            Email = "erik.bauer@pirka.com",
            Password = "ErikBauer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("cc2f04b7-ea08-4f6f-916e-d10dd739d925"),
            Name = "Lara Lehmann",
            Email = "lara.lehmann@feldkirchen_bei_graz.com",
            Password = "LaraLehmann123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("d1ce7772-2298-45c9-aee2-80b527f91005"),
            Name = "Matthias Sommer",
            Email = "matthias.sommer@gössendorf.com",
            Password = "MatthiasSommer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("3983e9a0-410a-4b8d-9342-62f66dd6a369"),
            Name = "Johanna Schäfer",
            Email = "johanna.schäfer@graz.com",
            Password = "JohannaSchäfer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("9dd3c416-b267-4536-9a27-b09815f724f8"),
            Name = "Samuel Lange",
            Email = "samuel.lange@thal.com",
            Password = "SamuelLange123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("3522f9c4-c9cf-43be-bc2c-9de720f90d4f"),
            Name = "Elena Kaiser",
            Email = "elena.kaiser@grambach.com",
            Password = "ElenaKaiser123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("6e0dfd33-062d-4987-ba72-48e151677c92"),
            Name = "Simon Kaiser",
            Email = "simon.kaiser@feldkirchen_bei_graz.com",
            Password = "SimonKaiser123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("3d7be304-b7f1-4d66-be60-7acdbd1e24aa"),
            Name = "Isabella Huber",
            Email = "isabella.huber@hausmannstätten.com",
            Password = "IsabellaHuber123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("1e88101b-f472-4179-9a45-eaa44621e522"),
            Name = "Fabian Richter",
            Email = "fabian.richter@gössendorf.com",
            Password = "FabianRichter123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("9c905592-2115-4bdc-9ed8-621d2cf15028"),
            Name = "Clara Huber",
            Email = "clara.huber@kalsdorf_bei_graz.com",
            Password = "ClaraHuber123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("be1a0d2b-f55d-4dc5-b099-2a6b7aea837b"),
            Name = "Philipp Werner",
            Email = "philipp.werner@pirka.com",
            Password = "PhilippWerner123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("021c918d-8648-44e1-a91a-39cfa8c05fe4"),
            Name = "Nina Lang",
            Email = "nina.lang@seiersberg.com",
            Password = "NinaLang123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("26248215-6ede-405e-8e86-3a8e74d871ed"),
            Name = "Sebastian Scholz",
            Email = "sebastian.scholz@thal.com",
            Password = "SebastianScholz123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("c3bcd258-a52c-4288-b91d-3e98c86a9631"),
            Name = "Charlotte Fischer",
            Email = "charlotte.fischer@gratwein-straßengel.com",
            Password = "CharlotteFischer123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("4725be34-2ebc-4669-a944-89a4120baa1f"),
            Name = "Daniel Haas",
            Email = "daniel.haas@graz.com",
            Password = "DanielHaas123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("43e9d8a2-4211-4665-bd8a-5e4b577ba913"),
            Name = "Katharina Krüger",
            Email = "katharina.krüger@hart_bei_graz.com",
            Password = "KatharinaKrüger123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("84aefaab-7099-425c-a366-244a434a4140"),
            Name = "Tobias Schmid",
            Email = "tobias.schmid@raaba.com",
            Password = "TobiasSchmid123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("98d2f0ce-e66b-4ca8-bb2d-91551765b568"),
            Name = "Mila Schütz",
            Email = "mila.schütz@stattegg.com",
            Password = "MilaSchütz123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("d16d645b-d881-4f4b-ab95-22a69e0eca04"),
            Name = "Jan Wagner",
            Email = "jan.wagner@graz.com",
            Password = "JanWagner123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("7c6ae0de-0a98-4458-bac7-344957eadb2d"),
            Name = "Sabrina Neumann",
            Email = "sabrina.neumann@gratwein-straßengel.com",
            Password = "SabrinaNeumann123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("33625073-488a-4913-a528-d72a98693660"),
            Name = "Florian Schulz",
            Email = "florian.schulz@raaba.com",
            Password = "FlorianSchulz123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        },

        new()
        {
            Id = Guid.Parse("1c69a029-0898-428b-8a90-91baff9a2ed0"),
            Name = "Maria Becker",
            Email = "maria.becker@hart_bei_graz.com",
            Password = "MariaBecker123",
            CreatedUtc = DateTime.Parse("31.10.2024 12:45:09"),
            UpdatedUtc = DateTime.Parse("31.10.2024 12:45:09")
        }
    ];

    private readonly List<string> Todos = new()
    {
        // Groceries
        "Milch kaufen", "Brot holen", "Obst und Gemüse besorgen", "Käse kaufen", "Eier holen", "Butter kaufen",
        "Joghurt kaufen", "Nudeln besorgen", "Reis kaufen", "Fleisch für das Abendessen besorgen", "Kaffee kaufen",
        "Tee holen", "Bananen kaufen", "Apfelsaft besorgen", "Kartoffeln holen", "Tomaten kaufen", "Gurken besorgen",
        "Salat kaufen", "Paprika holen", "Zwiebeln besorgen", "Knoblauch kaufen", "Zitronen holen", "Mehl kaufen",
        "Zucker besorgen", "Honig kaufen", "Marmelade holen", "Kekse kaufen", "Schokolade besorgen",
        "Cornflakes kaufen",
        "Müsli holen", "Nüsse besorgen", "Mineralwasser kaufen", "Orangensaft holen", "Joghurt trinken kaufen",
        "Frischkäse holen", "Salz kaufen", "Öl besorgen", "Essig kaufen", "Sahne holen", "Kräuter kaufen",
        "Kaugummi besorgen", "Fisch für das Mittagessen holen", "Tofu besorgen", "Sojamilch kaufen", "Backpulver holen",
        "Schinken kaufen", "Wurst besorgen", "Sekt für den Abend kaufen", "Eis für die Kinder holen",
        "Kaffeesahne kaufen",

        // Housekeeping
        "Wohnzimmer saugen", "Badezimmer reinigen", "Küche wischen", "Fenster putzen", "Staub wischen",
        "Müll rausbringen",
        "Schlafzimmer aufräumen", "Wäsche waschen", "Betten beziehen", "Geschirr spülen", "Boden fegen",
        "Regale abstauben", "Kühlschrank säubern", "Spülmaschine ausräumen", "Wäsche zusammenlegen",
        "Keller aufräumen", "Garten gießen", "Schränke organisieren", "Abfluss reinigen", "Bettwäsche wechseln",
        "Handtücher waschen", "Badewanne reinigen", "Toilette putzen", "Spiegel reinigen", "Mülleimer leeren",
        "Küchenarbeitsfläche abwischen", "Wanddekoration abstauben", "Lampen reinigen", "Büro aufräumen",
        "Esszimmer organisieren", "Flur sauber machen", "Wände abwaschen", "Fensterbänke abwischen",
        "Schubladen sortieren",
        "Post sortieren", "Kamin sauber machen", "Flecken entfernen", "Blumen gießen", "Türgriffe desinfizieren",
        "Geschirr abtrocknen", "Badezimmerteppiche waschen", "Tischdecken reinigen", "Esszimmerstuhlpolster absaugen",
        "Spinnweben entfernen", "Kleider bügeln", "Altpapier wegbringen", "Kerzenständer reinigen",
        "Spülbecken säubern",
        "Kleider sortieren", "Gefriertruhe abtauen", "Mikrowelle reinigen", "Bilderrahmen abstauben",

        // Car-related tasks
        "Tank auffüllen", "Auto waschen", "Ölstand prüfen", "Reifen wechseln", "Scheibenwischerflüssigkeit auffüllen",
        "Luftdruck der Reifen prüfen", "Innenraum reinigen", "Auto staubsaugen", "Scheibenwischer ersetzen",
        "Werkstatttermin vereinbaren", "Bremsen kontrollieren", "Batterie prüfen", "Fahrzeugschein erneuern",
        "Scheiben reinigen", "Auto polieren", "Frostschutzmittel nachfüllen", "Kofferraum aufräumen",
        "Scheibenwischwasser auffüllen", "Verbandskasten prüfen", "Ersatzreifen kontrollieren",
        "Luftfilter reinigen", "Auto winterfest machen", "Reinigungstücher ins Auto legen",
        "Erste-Hilfe-Kasten überprüfen", "Ladekabel für Handy ins Auto legen", "Zündkerzen prüfen",
        "Motorhaube reinigen", "Parktickets kontrollieren", "Handschuhfach organisieren", "Handbremse überprüfen",
        "Autoversicherung erneuern", "Schlüssel kopieren lassen", "Fensterdichtungen pflegen",
        "Abgasanlage überprüfen", "Lichtanlage testen", "Notfallwarnschild ins Auto legen",
        "Radmuttern anziehen", "Kfz-Nummernschilder reinigen", "Inspektionstermin planen",
        "Wagenheber prüfen", "Sicherungen kontrollieren", "Innenbeleuchtung überprüfen", "Klimaanlage testen",
        "Winterreifen montieren", "Sommerreifen lagern", "Rechnung für Inspektion einreichen",
        "Fahrzeuginnenraum desinfizieren", "Staubschutzmatten reinigen", "USB-Ladegerät kaufen",

        // Child-related tasks
        "Kinderzimmer aufräumen", "Hausaufgaben überprüfen", "Schulranzen packen", "Frühstück vorbereiten",
        "Schulbrot einpacken", "Kinder baden", "Bücher vorlesen", "Spielzeug wegräumen",
        "Kinderkleidung waschen", "Zahnarzttermin vereinbaren", "Impftermin machen", "Windeln kaufen",
        "Kinderarzt besuchen", "Spielplatz besuchen", "Mittagessen für Kinder vorbereiten",
        "Schulsachen kaufen", "Kinder ins Bett bringen", "Spielzeit organisieren", "Freizeitaktivitäten planen",
        "Kinderbücher zurückgeben", "Elterngespräch in der Schule vereinbaren", "Kinderfahrräder reparieren",
        "Kuchen für Schule backen", "Hausaufgabenhilfe leisten", "Kleider für Kinder organisieren",
        "Kuscheltiere waschen", "Matschkleidung besorgen", "Schuluniform waschen", "Spieleabend planen",
        "Neue Bastelmaterialien kaufen", "Kinder zum Sport bringen", "Schulprojekte vorbereiten",
        "Kindergeburtstag planen", "Kinderbücher bestellen", "Kinoabend planen", "Schulranzen reparieren",
        "Kinder zum Arzt bringen", "Freunde der Kinder einladen", "Puppen waschen", "Kinderzimmer dekorieren",
        "Kindergarten sachen packen", "Schulzeug kaufen", "Fahrrad für Kinder organisieren",
        "Kindertanzkurs planen", "Lernspiele spielen", "Spielsachen aussortieren", "Lauflernwagen reinigen",
        "Kinderbett beziehen", "Windeln entsorgen", "Bastelmaterial sortieren",

        // Miscellaneous tasks
        "Post abholen", "Arzttermin vereinbaren", "Banktermin machen", "Rechnungen bezahlen",
        "Passfotos machen lassen", "Pakete verschicken", "Handy aufladen", "Telefonate erledigen",
        "Geschenk für Freund kaufen", "Brille putzen", "Notizen sortieren", "Buch zurückgeben",
        "Ferien planen", "Rucksack packen", "Schreibtisch aufräumen", "Termin im Amt machen",
        "Fotos sortieren", "Schlüssel abholen", "Büroutensilien organisieren", "Reise buchen",
        "Tagesplan erstellen", "Fahrrad aufpumpen", "Freunde anrufen", "Blutspendetermin machen",
        "Hausversicherung prüfen", "Freizeitpark besuchen", "Sofa reinigen", "Joggingrunde planen",
        "Schuhpflege machen", "Nagelpflege machen", "Brillenreinigung", "Schneider besuchen",
        "Haartermin vereinbaren", "Schwimmen gehen", "Ergotherapie-Termin ausmachen",
        "Familienabend planen", "Haustier füttern", "Gemüsebeet pflegen", "Yogaübungen machen",
        "Klavier üben", "Post sortieren", "Konzertkarten bestellen", "Rezepte für Woche planen",
        "Museum besuchen", "Neues Rezept ausprobieren", "Wandertag planen", "Werkzeuge organisieren"
    };

    public TodoGenerator()
    {
        var currentLatitude = 47.0707;
        var currentLongitude = 15.4395;
        var graz = new Point(15.4395, 47.0707) { SRID = 4326 }; // Starting location around Graz
        var todos = new List<Todo>();
        foreach (var user in _users)
            for (var i = 0; i < new Random().Next(0, 200); i++)
            {
                currentLatitude += (_random.NextDouble() - 0.5) * 0.003;
                currentLongitude += (_random.NextDouble() - 0.5) * 0.003;
                var location = new Point(currentLongitude, currentLatitude) { SRID = 4326 };
                var (title, description) = _todoDescription.ElementAt(_random.Next(0, _todoDescription.Count));
                todos.Add(new Todo
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,

                    Location = _random.Next(0, 100) < 50 ? null : location,
                    Title = title,
                    Content = description,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow,
                    DueUtc = DateTime.UtcNow.AddMinutes(new Random().Next(1, 60 * 24 * 30)),
                    AssignedToId = _random.Next(0, 100) < 50 ? user.Id : _users[_random.Next(0, _users.Count)].Id
                });
            }


        //output
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "INSERT INTO Todo (Id, UserId,Location, Title, Description,CreatedUtc, UpdatedUtc, DueUtc, AssignedToId) VALUES ");
        var listBuilder = new StringBuilder();
        listBuilder.Append("List<Todo> todos = new List()\n{\n");
        foreach (var todo in todos)
        {
            //list
            listBuilder.Append("new Todo\n{\n");
            listBuilder.Append($"Id = Guid.Parse(\"{todo.Id}\"),\n");
            listBuilder.Append($"UserId = Guid.Parse(\"{todo.UserId}\"),\n");
            /*listBuilder.Append(
                $"Location = new Point({todo.Location.X}, {todo.Location.Y}) {{ SRID = {todo.Location.SRID} }},\n");*/
            if (todo.Location != null)
                sqlBuilder.Append(
                    $"Location = new Point({todo.Location.X}, {todo.Location.Y}) {{ SRID = {todo.Location.SRID} }},\n");
            else
                sqlBuilder.Append(
                    "Location = null,\n");
            listBuilder.Append($"Title = \"{todo.Title}\",\n");
            listBuilder.Append($"Content = \"{todo.Content}\",\n");
            listBuilder.Append($"CreatedUtc = DateTime.Parse(\"{todo.CreatedUtc}\"),\n");
            listBuilder.Append($"UpdatedUtc = DateTime.Parse(\"{todo.UpdatedUtc}\"),\n");
            listBuilder.Append($"DueUtc = DateTime.Parse(\"{todo.DueUtc}\"),\n");
            listBuilder.Append($"AssignedToId = Guid.Parse(\"{todo.AssignedToId}\"),\n");
            listBuilder.Append("},\n");

            //sql
            sqlBuilder.Append(
                $"('${todo.Id}', ");
            sqlBuilder.Append($"'{todo.UserId}', ");
            sqlBuilder.Append($"'{todo.Title}', ");
            sqlBuilder.Append($"'{todo.Content}', ");
            sqlBuilder.Append($"'{todo.CreatedUtc}', ");
            sqlBuilder.Append($"'{todo.UpdatedUtc}', ");
            sqlBuilder.Append($"'{todo.DueUtc}'),\n");
            // Check if Location is set and add it to SQL
            if (todo.Location != null)
                sqlBuilder.Append(
                    $"ST_SetSRID(ST_MakePoint({todo.Location.X}, {todo.Location.Y}), {todo.Location.SRID}), ");
            else
                sqlBuilder.Append("NULL, "); // Handle null location
        }

        listBuilder.Append("};");

        sqlBuilder.Append(";");
        File.WriteAllText("todos.txt", listBuilder.ToString());
        File.WriteAllText("todos.sql", sqlBuilder.ToString());
    }
}