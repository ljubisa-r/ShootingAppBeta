<template>
  <button @click="loginUser" class="login-button">Submit</button>
</template>

<script setup>
import axios from 'axios'
import { useRouter } from 'vue-router'
import { defineProps } from 'vue'

const props = defineProps({
  username: String,
  password: String
})

const router = useRouter();

const loginUser = async () => {
  try {
      const response = await axios.post('https://localhost:7082/api/Auth/login', {
        username: props.username,
        password: props.password
      })
      const token = response.data.token || response.data;
    
      if (token)
      {
        localStorage.setItem('jwtToken', token);          
       
        const userId = getUserId(token);         
        if (userId) 
          localStorage.setItem('userId', userId);
        
        window.dispatchEvent(new Event('tokenChanged'))
        router.push({ path: '/' });    
      } else {
        alert('No token received!');
      }
  } catch (error) {
    console.error('Login failed:', error);
    alert('Login failed! Check console for details.');
    }
}

function getUserId(token) {
  const decoded = parseJwt(token);
  if (!decoded) return null;
  return decoded.userId || decoded.id || decoded.sub || decoded.nameid || null;
}

function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    console.error('Invalid token:', e);
    return null;
  }
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
  width: 100%;
}

.login-button:hover {
  background-color: #36966f;
}
</style>
