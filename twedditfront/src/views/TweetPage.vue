<template>
  <div>
    <h1>Post a Tweet, OR ELSE!!!</h1>
    <form @submit.prevent="postTweet">
      <input type="text" v-model="tweet" placeholder="What's on your mind?" />
      <button type="submit">Tweet 2</button>
    </form>
  </div>
</template>

<script>
// Importing the necessary modules
import axios from 'axios';
import { useToast } from 'vue-toastification';

export default {
  data() {
    return {
      tweet: "", // Model to hold the tweet text
    };
  },
  setup() {
    const toast = useToast();
    return { toast };
  },
  methods: {
    async postTweet() {
      // Ensure tweet content is not empty
      if (!this.tweet) {
        this.toast.error("Tweet cannot be empty");
        return;
      }

      // Get the token from localStorage
      const token = localStorage.getItem('token');

      let name = 'Anon'; // Default to an empty string in case the token is missing or invalid

      if (token) {
          try {
              // Extract the payload
              const payloadBase64 = token.split('.')[1];
              const decodedPayload = atob(payloadBase64);
              const payload = JSON.parse(decodedPayload);

              // Get the 'name' field from the payload
              name = payload.preferred_username || ''; // Use a fallback in case 'name' is undefined
          } catch (error) {
              console.error('Error decoding JWT:', error);
          }
      } else {
          console.warn('No token found in localStorage');
      }
      
      
      // Prepare the payload
      const tweetData = {
        Tweet: this.tweet, // Matching the TweetDto structure
        UserName: name, // You can dynamically get the username if needed
      };

      console.log(tweetData);
      try {
        // Send POST request to the backend API 
        //const response = await axios.post("http://localhost:8085/api/tweets", tweetData);
        //const response = await axios.post("http://tweetservice:5001/api/tweets", tweetData); 
        const response = await axios.post("http://localhost:5001/api/tweets", tweetData);
        //const response = await axios.post("http://192.168.99.100:30001/api/tweets", tweetData); 

        // Check if the response is successful
        if (response.status === 200) {
          this.toast.success("Tweet posted successfully!");
          this.tweetContent = ""; // Clear the input field after posting
          
          // If you want to push notifications (using a notification hub, SignalR, etc.), 
          // you can handle it here.
          this.sendNotificationToHub(); // Example method for sending a notification
        } else {
          // Handle failure response from the server
          this.toast.error("Failed to post tweet: " + response.data.message || "Unknown error");
        }
      } catch (err) {
        // Catch and handle any network errors or unexpected errors
        this.toast.error("An error occurred: " + err.message);
      }
    },

    // Example method for sending a notification to the notification hub
    sendNotificationToHub() {
      // You can call your SignalR hub here to send a notification to all connected clients
      // Example:
      // const connection = new signalR.HubConnectionBuilder()
      //     .withUrl("/notificationHub")
      //     .build();

      // connection.invoke("SendMessage", "New tweet posted!", this.tweetContent)
      //     .catch(err => console.error("Error sending message to hub:", err));
    },
  },
};
</script>

<style scoped>
/* Add any styles as needed */
</style>
