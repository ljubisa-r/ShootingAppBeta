<template>
  <div class="app-container">
    <header class="app-header">
      <h1>Glossary App</h1>      
      <LoginButton v-if="!isLoggedIn" />
      <LogoutButton v-else />
    </header>

<!-- MAIN CONTENT -->
    <main class="content">
      <!-- router-view prikazuje trenutnu rutu -->
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import LoginButton from './components/LoginButton.vue';
import LogoutButton from './components/LogoutButton.vue';

const isLoggedIn = ref(false);

onMounted(() => {
  isLoggedIn.value = localStorage.getItem('jwtToken')
})
</script>

<style scoped>
.app-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  font-family: Arial, sans-serif;
}

.app-header {
  background-color: #2c3e50;
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 2rem;
}

.content {
  flex: 1;
  padding: 2rem;
  background-color: #f8f9fa;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  background-color: white;
  border: 1px solid #ddd;
  padding: 0.75rem 1rem;
  margin-bottom: 0.5rem;
  border-radius: 5px;
}
</style>
