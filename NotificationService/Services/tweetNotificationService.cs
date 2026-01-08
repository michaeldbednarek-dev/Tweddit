using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class tweetNotificationService
{
    private const string QUEUE_NAME = "tweet_notifications";
    private readonly IHubContext<NotificationHub> _hubContext;

    public tweetNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
        StartListening();
    }

    private void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: QUEUE_NAME,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received '{message}'");

            // Notify the front-end using SignalR
            await NotifyFrontend(message);
        };

        channel.BasicConsume(queue: QUEUE_NAME,
                             autoAck: true,
                             consumer: consumer);
    }

    private async Task NotifyFrontend(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
