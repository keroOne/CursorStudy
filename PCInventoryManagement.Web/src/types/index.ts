export interface PC {
  id: number
  managementNumber: string
  modelName: string
  osTypeId: number
  currentUserId: number
  isDeleted: boolean
  createdAt: string
  updatedAt: string
  osType?: OSType
  currentUser?: User
}

export interface OSType {
  id: number
  name: string
  isDeleted: boolean
}

export interface User {
  id: number
  adAccount: string
  displayName: string
  isActive: boolean
  isDeleted: boolean
} 