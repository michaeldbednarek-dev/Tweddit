<!-- src/components/Register.vue -->
<template>
  <div class="register">
    <h2>Register</h2>
    <form @submit.prevent="register">
      <div class="form-group">
        <label for="username">User name:</label>
        <input
          type="text"
          id="username"
          v-model="username"
          required
          placeholder="Enter your user name"
        />
      </div>

      <div class="form-group">
        <label for="email">Email:</label>
        <input
          type="email"
          id="email"
          v-model="email"
          required
          placeholder="Enter your email"
        />
      </div>
      
      <div class="form-group">
        <label for="password">Password:</label>
        <input
          type="password"
          id="password"
          v-model="password"
          required
          placeholder="Enter your password"
        />
      </div>

      <button type="submit">Register</button>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

      <!-- Login navigation button -->
      <p>
        Already have an account?
        <button @click="goToLogin" class="link-button">Login here</button>
      </p>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      username: '',
      email: '',
      password: '',
      errorMessage: '',
    };
  },
  methods: {
    async register() {
      try {
        console.log("I reach here");
        console.log(this.password);

        const userData=
        {
          username: this.username,
          email: this.email,
          password: this.password,
        }

        const response = await axios.post("http://localhost:8085/identity/register", userData);

        

        if (response.data.success) {
          this.$router.push('/login');
        } else {
          this.errorMessage = 'Registration failed. Please try again.';
        }
      } catch (error) {
        this.errorMessage = error.response ? error.response.data.message : 'An error occurred. Please try again.';
      }
    },
    goToLogin() {
      this.$router.push('/login');
    }
  },
};
</script>

<style scoped>
.register {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
}

.form-group {
  margin-bottom: 15px;
}

input[type="text"],
input[type="email"],
input[type="password"] {
  width: 100%;
  padding: 10px;
  border-radius: 4px;
  border: 1px solid #ddd;
}

button {
  padding: 10px 20px;
  background-color: #42b983;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>