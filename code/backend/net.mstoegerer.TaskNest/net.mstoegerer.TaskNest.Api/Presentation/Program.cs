using System.Text;
using Microsoft.IdentityModel.Tokens;
using net.mstoegerer.TaskNest.Api.Application.Extensions;
using net.mstoegerer.TaskNest.Api.Application.Services;
using net.mstoegerer.TaskNest.Api.Infrastructure;
using net.mstoegerer.TaskNest.Api.Infrastructure.Extensions;

const bool seed = false;
if (seed)
{
    var seedGenerator = new SeedDataGenerator();
    seedGenerator.GenerateUsers();
    seedGenerator.GenerateTodos();
    seedGenerator.GenerateAttachments();
    seedGenerator.GenerateTodoShares();
    seedGenerator.GenerateUserMetaData();
    seedGenerator.GenerateMetaData();
}

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Presentation/appsettings.json", true)
    .AddJsonFile("Presentation/appsettings.Local.json", true);
IConfiguration configuration = configurationBuilder
    .Build();
// Add services to the container.
// Add JWT settings from configuration (appsettings.json or environment variables)
var jwtSettings = configuration.GetSection("JwtSettings");
builder.Services.AddSingleton(new JwtService(
    jwtSettings["Key"],
    jwtSettings["Issuer"],
    jwtSettings["Audience"]
));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(configuration);
builder.Services.AddApplicationServices();


// Configure JWT authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) // Replace with your key
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }


//app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthentication(); // Enables authentication
app.UseAuthorization(); // Enables authorization

app.MapControllers();


app.Run();