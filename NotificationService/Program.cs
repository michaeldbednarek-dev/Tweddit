using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<tweetNotificationService>(); // Register NotificationService
var app = builder.Build();

// Set up RabbitMQ connection and message handling
/*
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "tweets", durable: false, exclusive: false, autoDelete: false, arguments: null);

// Set up the consumer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // Notify the front end via SignalR
    var hubContext = app.Services.GetRequiredService<IHubContext<NotificationHub>>();
    await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
};


// Start consuming messages
channel.BasicConsume(queue: "tweet", autoAck: true, consumer: consumer);
*/

app.MapHub<NotificationHub>("/notificationHub"); // WHY YOU NOt WORKING THOUGH?!?!??!

app.Run();


/*
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<tweetNotificationService>(); // Register NotificationService

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/