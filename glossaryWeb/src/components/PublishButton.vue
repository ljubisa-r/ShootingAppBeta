<template>
  <button @click="handlePublish" class="publish-button">
    Publish
  </button>
</template>

<script setup>
import axios from 'axios'
import { defineProps} from 'vue'

const userId = localStorage.getItem('userId');

const props = defineProps({
  term: {
    type: Object,
    required: true
  }
})

const isValidTerm = validateTerm(props.term);
 
const handlePublish = async () => {  
  try {
    
    if (isValidTerm.valid) {
        const token = localStorage.getItem('jwtToken');
        await axios.put(`https://localhost:7082/api/Glossary/publish`, 
        props.term,
        { headers: { Authorization: `Bearer ${token}` }
        });
        alert(`Term "${props.term.id}" published successfully`);
        window.location.reload();
    } 
    else {
      alert(isValidTerm.message);
    }
    
  } catch (error) {
    console.error('Publish  failed:', error);
    alert('Failed to publish  term' );
  }
}

function validateTerm(term) {
  const forbiddenWords = ['lorem', 'test', 'sample'];

  // Proveri da li term postoji i nije prazan
  if (!term.term || term.term.trim() === '') {
    return { valid: false, message: 'Term cannot be empty.' };
  }

  // Proveri da li definicija ima najmanje 30 karaktera
  if (!term.definition || term.definition.trim().length < 30) {
    return { valid: false, message: 'Definition must be at least 30 characters long.' };
  }

  // Proveri da li sadrži zabranjene reči (case-insensitive)
  const defLower = term.definition.toLowerCase();
  const hasForbidden = forbiddenWords.some(word => defLower.includes(word));
  if (hasForbidden) {
    return { valid: false, message: 'Definition contains forbidden words (lorem, test, or sample).' };
  }

  //Ako su svi uslovi ispunjeni
  return { valid: true, message: 'Term is valid.' };
}

</script>

<style scoped>
.publish-button {
  background-color: #d3ebaf;
  color: rgb(10, 0, 0);
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  margin-left: 0.5rem;
}

.publish-button:hover {
  background-color: #9ff082;
}
</style>
