using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using TweetService.Models;
using TweetService.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TweetService.Controllers
{
    [ApiController]
    [Route("api/tweets")]
    public class TweetController : ControllerBase
    {
        private const string QUEUE_NAME = "tweets";
        private readonly TweetServiceDB _tweetService;

        public TweetController()
        {
            _tweetService = new TweetServiceDB();
        }

        [HttpPost]
        public async Task<IActionResult> PostTweet([FromBody] TweetDto tweetDto)
        {

            await _tweetService.CreateTweetAsync(tweetDto);

            return Ok("Tweet posted successfully!");

            // Send message to RabbitMQ
            SendToQueue(tweetDto.Tweet);
        }

        [HttpGet("getTweets")]
        public async Task<IActionResult> GetAllTweets()
        {
            var tweets = await _tweetService.GetAllTweetsAsync();
            return Ok(tweets);
        }

        private void SendToQueue(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: QUEUE_NAME,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            
            channel.BasicPublish(exchange: "",
                                routingKey: QUEUE_NAME,
                                basicProperties: null,
                                body: body);
                                
        }

    }
}
