<template>
<div>
  <div class="card shadow-sm termlist-view">
    <div class="card-body">     
      <table class="table table-striped align-middle">
        <thead class="table-dark">
          <tr>
            <th>ID</th>
            <th>Term</th>
            <th>Definition</th>
            <th>AuthorId</th>
            <th>Status</th>
            <th>
              <template v-if="token">
                <button @click="newTerm()" class="new-button">New</button>
              </template> 
            </th>
          </tr>
        </thead>
        <tbody>
          <!-- Loader -->
          <tr v-if="loading">
            <td colspan="5" class="text-center text-muted">Loading...</td>
          </tr>

          <!-- Error -->
          <tr v-else-if="error">
            <td colspan="5" class="text-danger text-center">{{ error }}</td>
          </tr>

          <!-- Termini -->
          <tr v-else v-for="term in terms" :key="term.Id">
            <td>{{ term.id }}</td>
            <td>{{ term.term || '-' }}</td>
            <td>{{ term.definition || '-' }}</td>
            <td>{{ term.createdBy }}</td>
            <td>{{ getStatusLabel(term.status) }}</td>

                <!-- Akcije: vidljive samo ako postoji token -->
            <template v-if="token">
              <td><button @click="editTerm(term)" class="edit-button">Edit</button></td>              
              <td><PublishButton :term="term" /></td>
              <td><DeleteButton :term="term" @deleted="removeTerm" /></td>
              <td><ArchiveButton :term="term" /></td>              
            </template>
          </tr>

          <!-- Prazna lista -->
          <tr v-if="!loading && !error && terms.length === 0">
            <td colspan="5" class="text-center text-muted">No terms found</td>
          </tr>
        </tbody>
      </table> 
    </div>
  </div>
  <!-- Modal -->
  <div v-if="showModal" class="modal-backdrop">
    <div class="modal">
      <div class="modal-header">
        <h5 class="modal-title">Term</h5>
        <button @click="closeModal" class="btn-close">X</button>
      </div>

      <div class="modal-body">
        <!-- TermForm se prikazuje unutar modala -->
        <TermForm :term="selectedTerm" @close="closeModal" />
      </div>
    </div>
  </div>
</div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import DeleteButton from './DeleteButton.vue'
import PublishButton from './PublishButton.vue'
import ArchiveButton from './ArchiveButton.vue'
import TermForm from './TermForm.vue'

// state
const terms = ref([])
const loading = ref(true)
const error = ref(null)
const token = ref(localStorage.getItem('jwtToken'))
const selectedTerm = ref(null)
const showModal = ref(false)

// metode
const fetchTerms = async () => {
  loading.value = true
  error.value = null
  try {
    const response = await axios.get('https://localhost:7082/api/Glossary')
    terms.value = response.data
  } catch (err) {
    console.error(err)
    error.value = 'Failed to fetch terms'
  } finally {
    loading.value = false
  }
}

const removeTerm = (id) => {
  terms.value = terms.value.filter(term => term.id !== id)
}

const editTerm = (item) => {
  selectedTerm.value = item
  showModal.value = true
}

const newTerm = () => {
  selectedTerm.value = {
    id: 0,
    term: '',
    definition: '',
    status: 0,
    createdBy: 0
  }
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
  selectedTerm.value = null
  window.location.reload()
}

const getStatusLabel = (status) => {
  switch (status) {
    case 0:
      return 'Draft'
    case 1:
      return 'Published'
    case 2:
      return 'Archived'
    default:
      return 'Unknown'
  }
}

// lifecycle
onMounted(() => {
  fetchTerms()
})
</script>

<style scoped>
/* Jednostavan stil za modal */
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1050;
}

.modal {
  background-color: white;
  border-radius: 8px;
  width: 600px;
  max-width: 90%;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  animation: fadeIn 0.2s ease-in-out;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #ddd;
  padding: 10px 15px;
}

.modal-body {
  padding: 15px;
}

.btn-close {
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
}
.edit-button {
  background-color: #c4dee1;
  color: rgb(10, 0, 0);
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  margin-left: 0.5rem;
}

.edit-button:hover {
  background-color: #969d94;
}
.new-button {
  background-color: #42b983;
  color: rgb(10, 0, 0);
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  margin-left: 0.5rem;
}

.new-button:hover {
  background-color: #36966f;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
</style>