using Microsoft.EntityFrameworkCore;
using URLShortening.Data;
using URLShortening.Services;
using URLShortening.Interfaces;
using URLShorteningApi.Models;


// Load environment variables from .env file
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Verify required environment variables
var requiredEnvVars = new[] { "DB_HOST", "DB_PORT", "DB_NAME", "DB_USER", "DB_PASSWORD" };
var missingVars = requiredEnvVars.Where(var => string.IsNullOrEmpty(Environment.GetEnvironmentVariable(var))).ToList();

if (missingVars.Any())
{
    throw new Exception($"Missing required environment variables: {string.Join(", ", missingVars)}");
}

// Build connection string from environment variables
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                      $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                      $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                      $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<URLShorteningContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connecting to React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
