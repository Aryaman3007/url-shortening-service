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
                      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

Console.WriteLine($"Database Host: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"Database Name: {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"Database User: {Environment.GetEnvironmentVariable("DB_USER")}");

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
            policy.WithOrigins("http://localhost:3000", "https://aryaman3007.github.io") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "10000"));
});

var app = builder.Build();

// Add error handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
});

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only use HTTPS redirection in development
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Try to migrate the database on startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<URLShorteningContext>();
        Console.WriteLine("Attempting database migration...");
        context.Database.Migrate();
        Console.WriteLine("Database migration completed successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

app.Run();
