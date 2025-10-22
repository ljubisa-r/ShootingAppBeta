<template>
  <div class="edit-term-container">
    <h2>Term details</h2>

    <div v-if="term">
      <form @submit.prevent="saveChanges" class="edit-term-form">
        <div class="form-group">
          <label>ID</label>
          <input type="text" v-model="term.id" disabled />
        </div>

        <div class="form-group">
          <label>Term</label>
          <input type="text" v-model="term.term" required />
        </div>

        <div class="form-group">
          <label>Definition</label>
          <textarea v-model="term.definition"></textarea>
        </div>

        <button type="submit" class="save-button">Save Changes</button>
      </form>
    </div>

    <div v-else>
      <p>Loading term...</p>
    </div>
  </div>
</template>

<script setup>
import axios from 'axios'

const props = defineProps({
  term: {
    type: Object,
    required: true
  }
});

const saveChanges = async () => {
  try {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      alert('No authorization token found. Please login.');
      return;
    }
    // kreiranje novog terma
    if (props.term.id==0) {
      const response = await axios.post('https://localhost:7082/api/Glossary',
          props.term, // body 
          {
            headers: {
              'Authorization': `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          }
        );
    props.term.id=response.data.id;
    alert('Term added successfully!');      
    } 
    else {  // azuriranje terma 
      await axios.put('https://localhost:7082/api/Glossary/update',
      props.term, // body 
      {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      }
    );
    alert('Term updated successfully!');
    }    

  } catch (error) {
    console.error('Error updating term:', error);
    alert('Failed to update term.');
  }
};

</script>

<style scoped>
.edit-term-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 15px;
}

input,
textarea,
select {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
}

.save-button {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 16px;
  border-radius: 6px;
  cursor: pointer;
}

.save-button:hover {
  background-color: #45a049;
}
</style>