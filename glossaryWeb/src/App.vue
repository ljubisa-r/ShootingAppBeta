<template>
  <div class="app-container">
    <header class="app-header">
      <h1>Glossary App</h1>      
      <LoginButton v-if="!isLoggedIn" />
      <LogoutButton v-else @logout="handleLogout" />
    </header>

<!-- MAIN CONTENT -->
    <main class="content">
      <!-- router-view prikazuje trenutnu rutu -->
      <router-view :key="$route.fullPath" />
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import LoginButton from './components/LoginButton.vue';
import LogoutButton from './components/LogoutButton.vue';

const isLoggedIn = ref(false);


const checkLogin = () => {
  isLoggedIn.value = !!localStorage.getItem('jwtToken')
}


const handleLogout = () => {
  localStorage.removeItem('jwtToken')
  localStorage.removeItem('userId')
  checkLogin()
}

onMounted(() => {
  checkLogin()

  // on changes in other tab
  window.addEventListener('storage', checkLogin)

  // on changes in same tab
  window.addEventListener('tokenChanged', checkLogin)
})


onBeforeUnmount(() => {
  window.removeEventListener('storage', checkLogin)
  window.removeEventListener('tokenChanged', checkLogin)
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
