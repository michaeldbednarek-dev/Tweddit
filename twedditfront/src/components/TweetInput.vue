<template>
  <div class="tweet-input-container">
    <textarea
      v-model="tweet"
      placeholder="What's happening?"
      class="tweet-textarea"
    ></textarea>
    <button @click="postTweet" class="tweet-button">Tweet</button>
    <button @click="postTweet" class="tweet-button">Tweet</button>
  </div>

  <h2>Notifications</h2>
    <ul>
      <li v-for="(notification, index) in notifications" :key="index">{{ notification }}</li>
    </ul>
  </div>
</template>

<script>
export default {
  name: 'TweetInput',
  data() {
    return {
      tweet: '',
    };
  },
  mounted() {
    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5002/notificationHub') // Adjust port if necessary
      .build();

    this.connection.on('ReceiveMessage', (message) => {
      console.log('New notification:', message);
      this.notifications.push(message);
    });

    this.connection
      .start()
      .then(() => {
        console.log('SignalR connected');
      })
      .catch((err) => {
        console.error('Error connecting to SignalR:', err);
      });
  },
  methods: {
    postTweet() {
      if (this.tweet.trim()) {
        this.$emit('tweetPosted', this.tweet);
        this.tweet = ''; // Clear the input after posting
      } else {
        alert('Please enter a tweet');
      }
    },
  },
};
</script>

<style>
.tweet-input-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.tweet-textarea {
  width: 100%;
  height: 80px;
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ddd;
  border-radius: 8px;
}

.tweet-button {
  padding: 10px 20px;
  font-size: 16px;
  color: white;
  background-color: #1da1f2;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.tweet-button:hover {
  background-color: #1a91da;
}
</style>
