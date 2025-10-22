import { createRouter, createWebHistory } from 'vue-router'
import TermList from '../components/TermList.vue'
import LoginForm from '../components/LoginForm.vue'

const routes = [
  {
    path: '/',
    name: 'termlist',
    component: TermList
  },
  {
    path: '/login',
    name: 'login',
    component: LoginForm
  } 
]
 

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
