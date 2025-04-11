using System.Text.Json;
using System.Text.Json.Serialization;
using webapi_pred.Data;
using webapi_pred.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// env variables
var connectionString = Environment.GetEnvironmentVariable("PREDICTOR_DB_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Missing environment variable: PREDICTOR_DB_CONNECTION");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// deal with object cycle and object depth maximum


app.Run();
