export interface PC {
  id: number
  managementNumber: string
  modelName: string
  osTypeId: number
  currentUserId: number | null
  isDeleted: boolean
  createdAt: string
  updatedAt: string
  osType?: OSType
  currentUser?: User
  pcLocationHistories?: PCLocationHistory[]
}

export interface OSType {
  id: number
  name: string
  isDeleted: boolean
  pcs?: PC[]
}

export interface User {
  id: number
  name: string
  adAccount: string
  locationId: number
  isDeleted: boolean
  location?: Location
  pcs?: PC[]
}

export interface Location {
  id: number
  code: string
  name: string
  isDeleted: boolean
  users?: User[]
}

export interface PCLocationHistory {
  id: number
  pcId: number
  locationId: number
  startDate: string
  endDate: string | null
  location?: Location
} 