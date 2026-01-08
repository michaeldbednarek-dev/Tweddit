<template>
  <div>
    <h1>All Tweets</h1>
    <ul>
      <li v-for="tweet in tweets" :key="tweet.id">
        <strong>{{ tweet.userName }}</strong>: {{ tweet.tweet }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      tweets: [] // Array to hold the tweets
    };
  },
  methods: {
    async fetchTweets() {
      try {
        console.log("tets");
        // Make the GET request to the backend API
        //const response = await axios.get("http://localhost:8081/api/tweets/getTweets");
        const response = await axios.get("http://localhost:5001/api/tweets/getTweets");
        //const response = await axios.get("http://tweetservice:5001/api/tweets/getTweets");
        this.tweets = response.data; // Assign the response data to the tweets array
      } catch (error) {
        console.error("Error fetching tweets:", error);
      }
    }
  },
  created() {
    this.fetchTweets(); // Fetch tweets when the component is created
  }
};
</script>