<template>
  <button @click="goToLogin" class="login-button">
    Login
  </button>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter();

const goToLogin = () => {
  router.push({ name: 'login' }); // ide na rutu sa imenom 'login'
}
</script>

<style scoped>
.login-button {
  background-color: #42b983;
  color: white;
  border: none;
  padding: 0.6rem 1.2rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1rem;
}

.login-button:hover {
  background-color: #36966f;
}
</style>
