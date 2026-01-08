using MongoDB.Driver;
using TweetService;
using TweetService.Models;
using Prometheus;
using TweetService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:4173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Register TweetServiceDB and RabbitMqListener as background services
builder.Services.AddSingleton<TweetServiceDB>();
builder.Services.AddHostedService<RabbitMqListener>();

var app = builder.Build();

// Add Prometheus middleware to expose metrics
app.UseMetricServer(); // Exposes metrics at /metrics
app.UseHttpMetrics();  // Collects HTTP request metrics

app.MapGet("/api/tweets", () => "New tweet made!!!");

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
