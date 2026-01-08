using MongoDB.Driver;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using TweetService.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TweetService.Services;
using Microsoft.Extensions.Hosting;

namespace TweetService
{
    public class RabbitMqListener : IHostedService
    {
        private readonly string _queueName = "deletedUser";
        private readonly TweetServiceDB _tweetService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqListener(TweetServiceDB tweetService)
        {
            _tweetService = tweetService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                // Parse and handle the message (assuming the message contains the UserID to delete tweets for)
                await _tweetService.HandleDeletedUserAsync(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Listening for messages...");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}
