<template>
  <div id="app">
    <header>
      <nav>
        <router-link to="/user">User</router-link>
        <router-link to="/tweet">Post a Tweet</router-link> |
        <router-link to="/dash">Dashboard</router-link>
        <router-link to="/login">Login</router-link>
        <router-link to="/register">register</router-link>
      </nav>
    </header>
    <main>
      <router-view />
    </main>
  </div>
</template>

<script>
export default {
  name: "App",
};
</script>

<style>
/* Add global styles if needed */
header {
  background: #333;
  color: white;
  padding: 1rem;
}
nav a {
  color: white;
  margin-right: 1rem;
  text-decoration: none;
}
nav a.router-link-active {
  font-weight: bold;
  text-decoration: underline;
}
main {
  padding: 2rem;
}
</style>
