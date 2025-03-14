import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/pcs',
      name: 'pcs',
      component: () => import('../views/PCsView.vue')
    },
    {
      path: '/os-types',
      name: 'os-types',
      component: () => import('../views/OSTypesView.vue')
    },
    {
      path: '/users',
      name: 'users',
      component: () => import('../views/UsersView.vue')
    }
  ]
})

export default router
