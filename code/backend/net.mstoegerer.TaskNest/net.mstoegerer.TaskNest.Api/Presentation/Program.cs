using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Logging;
using net.mstoegerer.TaskNest.Api.Application.Extensions;
using net.mstoegerer.TaskNest.Api.Domain.Configs;
using net.mstoegerer.TaskNest.Api.Infrastructure;
using net.mstoegerer.TaskNest.Api.Infrastructure.Extensions;
using net.mstoegerer.TaskNest.Api.Presentation.Extensions;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;
using Serilog;

const bool seed = true;
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
    .AddJsonFile("Presentation/appsettings.Development.json", true)
    .AddJsonFile("Presentation/appsettings.Production.json", true);

IConfiguration configuration = configurationBuilder
    .Build();
builder.Host.UseSerilog((ctx, cfg) => { cfg.ReadFrom.Configuration(configuration); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(configuration, builder.Environment.IsDevelopment());
builder.Services.AddApplicationServices();

var auth0Config = configuration.GetConfig<Auth0Config>("Auth0");
builder.Services.AddAuth0(auth0Config);
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestBody;
    o.RequestHeaders.Add("Authorization");
    o.RequestHeaders.Add("X-API-Key");
});
builder.Services.AddApiKey(auth0Config);
var app = builder.Build();

app.UseCurrentUserMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "net.mstoegerer.TaskNest.Api v1"));
    IdentityModelEventSource.ShowPII = true;
    IdentityModelEventSource.LogCompleteSecurityArtifact = true;
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
//app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthentication(); // Enables authentication
app.UseAuthorization(); // Enables authorization

app.MapControllers();
app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.Run();