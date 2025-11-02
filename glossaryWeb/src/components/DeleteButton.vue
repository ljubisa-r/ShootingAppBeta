<template>
  <button @click="handleDelete" class="delete-button">
    Delete
  </button>
</template>

<script setup>
import axios from 'axios'
import { defineProps, defineEmits } from 'vue'

const props = defineProps({
  term: {
    type: Object,
    required: true
  }
});

const emits = defineEmits(['deleted']); // emit event kada je termin obrisan
const userId = localStorage.getItem('userId');

const handleDelete = async () => {
  if (!confirm(`Are you sure you want to delete term ${props.term.id}?`)) return;

  try {    
    if(userId==props.term.createdBy && props.term.status=="Draft"){
      const token = localStorage.getItem('jwtToken');
      await axios.delete(`https://localhost:7082/api/Glossary/${props.term.id}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    emits('deleted', props.term.id); // obavesti roditelja
    alert(`Term ${props.term.id} deleted successfully`);
  }else {
    alert('You can delete only terms in draft state and created by your self');
  }
  } catch (error) {
    console.error('Delete failed:', error);
    alert('Failed to delete term');
  }
}
</script>

<style scoped>
.delete-button {
  background-color: #e3d6d5;
  color: rgb(10, 0, 0);
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  margin-left: 0.5rem;
}

.delete-button:hover {
  background-color: #dca39c;
}
</style>
