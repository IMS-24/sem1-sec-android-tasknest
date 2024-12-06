using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Logging;
using net.mstoegerer.TaskNest.Api.Application.Extensions;
using net.mstoegerer.TaskNest.Api.Domain.Configs;
using net.mstoegerer.TaskNest.Api.Infrastructure;
using net.mstoegerer.TaskNest.Api.Infrastructure.Extensions;
using net.mstoegerer.TaskNest.Api.Presentation.Extensions;
using Serilog;

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
builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.WriteTo.Console();
    cfg.MinimumLevel.Information();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(configuration);
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

// app.UseCurrentUserMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
    IdentityModelEventSource.LogCompleteSecurityArtifact = true;
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