<template>
  <div>
    <h1>Delete User Page</h1>
    
    <!-- Display decoded JWT data -->
    <div v-if="jwtData">
      <h3>You:</h3>
      <pre>{{ jwtData }}</pre>
    </div>
    
    <!-- Button to trigger delete user action -->
    <button @click="deleteUser" :disabled="isDeleting">
      Delete User
    </button>

    <!-- Show loading indicator while deleting -->
    <div v-if="isDeleting">Deleting user...</div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      jwtData: null,
      isDeleting: false,
    };
  },
  mounted() {
    // Retrieve and decode JWT token
    const token = localStorage.getItem('token');

    if (token) {
          try {
              // Extract the payload
              const payloadBase64 = token.split('.')[1];
              const decodedPayload = atob(payloadBase64);
              const payload = JSON.parse(decodedPayload);

              // Get the 'name' field from the payload
              this.jwtData = payload; // Use a fallback in case 'name' is undefined
          } catch (error) {
              console.error('Error decoding JWT:', error);
          }
      } else {
          console.warn('No token found in localStorage');
      }
  },
  methods: {
    async deleteUser() {
      this.isDeleting = true;

      try {
        // Extract the userId from JWT data
        const userId = this.jwtData?.sub; // Assuming 'sub' holds the userId in the JWT

        if (!userId) {
          console.error('User ID not found in JWT token');
          this.isDeleting = false;
          return;
        }

        // Send request to delete user
        const response = await axios.post('http://localhost:8085/identity/delete', userId,);

        // Handle response
        if (response.status === 200) {
          alert('User deleted successfully!');
        } else {
          alert('Failed to delete user');
        }
      } catch (error) {
        console.error('Error deleting user:', error);
        alert('Error deleting user');
      } finally {
        this.isDeleting = false;
      }
    },
  },
};
</script>

<style scoped>
/* Add your styles here */
</style>
