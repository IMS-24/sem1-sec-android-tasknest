using System.Text;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure;

public class UserGenerator
{
    private readonly Random _random = new();

    private readonly List<string> Cities =
    [
        "Graz", "Seiersberg", "Raaba", "Feldkirchen bei Graz", "Hart bei Graz", "Grambach", "Gössendorf",
        "Kalsdorf bei Graz", "Stattegg", "Thal", "Hausmannstätten", "Gratwein-Straßengel", "Pirka"
    ];

    private readonly List<string> FirstNames =
    [
        "Alexander", "Sophie", "Maximilian", "Marie", "Paul", "Hannah", "Lukas", "Lea", "Felix", "Anna",
        "Leon", "Julia", "Jonas", "Laura", "David", "Emma", "Finn", "Lina", "Luca", "Lisa",
        "Tim", "Sarah", "Julian", "Mia", "Tom", "Leonie", "Nico", "Amelie", "Erik", "Lara",
        "Matthias", "Johanna", "Samuel", "Elena", "Simon", "Isabella", "Fabian", "Clara", "Philipp", "Nina",
        "Sebastian", "Charlotte", "Daniel", "Katharina", "Tobias", "Mila", "Jan", "Sabrina", "Florian", "Maria"
    ];

    private readonly List<string> LastNames =
    [
        "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz", "Hoffmann",
        "Schäfer", "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Braun", "Zimmermann",
        "Hartmann", "Krüger", "Schmid", "Lange", "Schmitt", "Werner", "Schmitz", "Krause", "Meier", "Lehmann",
        "Schmid", "Schütz", "Haas", "Peters", "Lang", "Scholz", "Möller", "König", "Walter", "Kaiser",
        "Huber", "Fuchs", "Vogel", "Stein", "Jung", "Otto", "Sommer", "Seidel", "Heinrich", "Brandt"
    ];

    public UserGenerator()
    {
        var users = (from firstName in FirstNames
            let lName = LastNames[_random.Next(0, LastNames.Count)]
            let city = Cities[_random.Next(0, Cities.Count)]
            let id = Guid.NewGuid()
            let email = $"{firstName}.{lName}@{city}.com".ToLower().Replace(" ", "_")
            let password = $"{firstName}{lName}123"
            let createdUtc = DateTime.UtcNow
            let updatedUtc = DateTime.UtcNow
            select new User
            {
                Id = id,
                CreatedUtc = createdUtc,
                UpdatedUtc = updatedUtc,
                Name = $"{firstName} {lName}",
                Email = email,
                Password = password
            }).ToList();


        //output
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO User (Id, Name, Email, Password, CreatedUtc, UpdatedUtc) VALUES ");
        var listBuilder = new StringBuilder();
        listBuilder.Append("var users = new List<User>\n{\n");
        foreach (var user in users)
        {
            listBuilder.Append("\tnew User\n\t{\n");
            listBuilder.Append($"\t\tId = Guid.Parse(\"{user.Id}\"),\n");
            listBuilder.Append($"\t\tName = \"{user.Name}\",\n");
            listBuilder.Append($"\t\tEmail = \"{user.Email}\",\n");
            listBuilder.Append($"\t\tPassword = \"{user.Password}\",\n");
            listBuilder.Append($"\t\tCreatedUtc = DateTime.Parse(\"{user.CreatedUtc}\"),\n");
            listBuilder.Append($"\t\tUpdatedUtc = DateTime.Parse(\"{user.UpdatedUtc}\"),\n");
            listBuilder.Append("\t},\n");

            sqlBuilder.Append(
                $"('{user.Id}', '{user.Name}', '{user.Email}', '{user.Password}', '{user.CreatedUtc}', '{user.UpdatedUtc}'),\n");
        }

        File.WriteAllText("users.txt", listBuilder.ToString());
        File.WriteAllText("users.sql", sqlBuilder.ToString());
    }
}