<template>
  <button @click="handleArchive" class="archive-button">
    Archive
  </button>
</template>

<script setup>
import axios from 'axios'
import { defineProps, defineEmits } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter();
const props = defineProps({
  term: {
    type: Object,
    required: true
  }
});

const handleArchive = async () => {
  if (!confirm(`Are you sure you want to archive term ${props.term.id}?`)) return;

  try { 
    if (props.term.status==1) {
      const token = localStorage.getItem('jwtToken');
      await axios.put(`https://localhost:7082/api/Glossary/${props.term.id}`,
      {},
      {headers: { Authorization: `Bearer ${token}` }}
      );
      alert(`Term ${props.term.id} archived successfully`);
      window.location.reload();
    } else {
      alert('You can archive only terms in published state');
    }   
    
  } catch (error) {
    console.error('Archived failed:', error);
    alert('Failed to archive term');
  }
}
</script>

<style scoped>
.archive-button {
  background-color: #e3d6d5;
  color: rgb(10, 0, 0);
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  margin-left: 0.5rem;
}

.archive-button:hover {
  background-color: #dca39c;
}
</style>
