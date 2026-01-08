namespace TweetService.Services
{
    using global::TweetService.Models;
    using MongoDB.Driver;
    using System;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public class TweetServiceDB
    {
        private readonly IMongoCollection<TweetDB> _tweetsCollection;

        public TweetServiceDB()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TwedditTweet");
            _tweetsCollection = database.GetCollection<TweetDB>("Tweets");
        }

        public async Task CreateTweetAsync(TweetDto tweet)
        {
            TweetDB newTweet = new TweetDB();

            newTweet.Content = tweet.Tweet;
            newTweet.UserName = tweet.UserName;

            await _tweetsCollection.InsertOneAsync(newTweet);
        }

        public async Task<List<TweetDto>> GetAllTweetsAsync()
        {
            // Retrieve all documents from the database
            var tweetDBs = await _tweetsCollection.Find(tweet => true).ToListAsync();

            // Map each TweetDB to a TweetDto
            var tweetDtos = tweetDBs.Select(tweetDB => new TweetDto
            {
                UserName = tweetDB.UserName,
                Tweet = tweetDB.Content // Map Content from TweetDB to Tweet in TweetDto
            }).ToList();

            return tweetDtos;
        }

        public async Task HandleDeletedUserAsync(string userName)
        {
            var filter = Builders<TweetDB>.Filter.Eq(t => t.UserName, userName);
            var result = await _tweetsCollection.DeleteManyAsync(filter);
            Console.WriteLine($"{result.DeletedCount} tweets deleted for user {userName}");
        }
    }

}
