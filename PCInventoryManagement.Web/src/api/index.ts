import axios from 'axios'
import type { PC, OSType, User } from '../types'

const api = axios.create({
  baseURL: 'http://localhost:5190/api'
})

// PC関連のAPI
export const pcApi = {
  getAll: () => api.get<PC[]>('/PCs'),
  getById: (id: number) => api.get<PC>(`/PCs/${id}`),
  create: (pc: Omit<PC, 'id' | 'createdAt' | 'updatedAt'>) => api.post<PC>('/PCs', pc),
  update: (id: number, pc: Partial<PC>) => api.put(`/PCs/${id}`, pc),
  delete: (id: number) => api.delete(`/PCs/${id}`)
}

// OS種類関連のAPI
export const osTypeApi = {
  getAll: () => api.get<OSType[]>('/OSTypes'),
  getById: (id: number) => api.get<OSType>(`/OSTypes/${id}`),
  create: (osType: Omit<OSType, 'id'>) => api.post<OSType>('/OSTypes', osType),
  update: (id: number, osType: Partial<OSType>) => api.put(`/OSTypes/${id}`, osType),
  delete: (id: number) => api.delete(`/OSTypes/${id}`)
}

// ユーザー関連のAPI
export const userApi = {
  getAll: () => api.get<User[]>('/Users'),
  getById: (id: number) => api.get<User>(`/Users/${id}`),
  create: (user: Omit<User, 'id'>) => api.post<User>('/Users', user),
  update: (id: number, user: Partial<User>) => api.put(`/Users/${id}`, user),
  delete: (id: number) => api.delete(`/Users/${id}`)
} 