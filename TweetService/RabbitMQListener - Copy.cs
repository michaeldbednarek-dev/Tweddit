using MongoDB.Driver;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using TweetService.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TweetService.Services;


namespace TweetService
{
    public class RabbitMqListener2
    {
        private readonly string _queueName = "deletedUser";
        private readonly TweetServiceDB _tweetService;

        public RabbitMqListener2()
        {
            _tweetService = new TweetServiceDB();
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost",
                Port = 5672,          
                UserName = "guest",    
                Password = "guest"     
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                // Parse and handle the message (assuming the message contains the UserID to delete tweets for)
                await _tweetService.HandleDeletedUserAsync(message);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Listening for messages...");

            // Keep the connection alive
            Thread.Sleep(Timeout.Infinite);
        }

        
    }
}
