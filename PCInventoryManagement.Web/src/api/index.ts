import axios from 'axios'
import type { PC, OSType, User, Location, PCLocationHistory } from '../types'

const api = axios.create({
  baseURL: 'http://localhost:5190/api'
})

// PC関連のAPI
export const pcApi = {
  getAll: () => api.get<PC[]>('/PCs'),
  getById: (id: number) => api.get<PC>(`/PCs/${id}`),
  create: (pc: Omit<PC, 'id'>) => api.post<PC>('/PCs', pc),
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

// 拠点関連のAPI
export const locationApi = {
  getAll: () => api.get<Location[]>('/Locations'),
  getById: (id: number) => api.get<Location>(`/Locations/${id}`),
  create: (location: Omit<Location, 'id'>) => api.post<Location>('/Locations', location),
  update: (id: number, location: Partial<Location>) => api.put(`/Locations/${id}`, location),
  delete: (id: number) => api.delete(`/Locations/${id}`)
}

// PC設置場所履歴関連のAPI
export const pcLocationHistoryApi = {
  getByPcId: (pcId: number) => api.get<PCLocationHistory[]>(`/PCs/${pcId}/LocationHistories`),
  create: (pcId: number, history: Omit<PCLocationHistory, 'id'>) => 
    api.post<PCLocationHistory>(`/PCs/${pcId}/LocationHistories`, history),
  update: (pcId: number, historyId: number, history: Partial<PCLocationHistory>) =>
    api.put(`/PCs/${pcId}/LocationHistories/${historyId}`, history),
  delete: (pcId: number, historyId: number) =>
    api.delete(`/PCs/${pcId}/LocationHistories/${historyId}`)
} 