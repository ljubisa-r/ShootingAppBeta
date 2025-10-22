<template>
  <div class="login-form">
    <h2>Login</h2>

    <div class="form-group">
      <label for="username">Username:</label>
      <input
        id="username"
        type="text"
        v-model="username"
        placeholder="Enter your username"
      />
    </div>

    <div class="form-group">
      <label for="password">Password:</label>
      <input
        id="password"
        type="password"
        v-model="password"
        placeholder="Enter your password"
      />
    </div>

    <SubmitButton :username="username" :password="password" />
  </div>
</template>


<script setup>
import { ref } from 'vue'
import SubmitButton from './SubmitButton.vue'

const username = ref('');
const password = ref('');
</script>

<style scoped>
.login-form {
  width: 300px;
  margin: 50px auto;
  background: #fff;
  border-radius: 10px;
  padding: 1.5rem;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

h2 {
  text-align: center;
  margin-bottom: 1rem;
  color: #2c3e50;
}

.form-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 1rem;
}

label {
  font-weight: bold;
  margin-bottom: 0.3rem;
}

input {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 1rem;
}
</style>
