<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center mb-4">
          <h1 class="text-h4" style="cursor: pointer" @click="$router.push('/')">PC管理</h1>
          <v-btn color="primary" @click="openDialog()">
            <v-icon start>mdi-plus</v-icon>
            新規PC追加
          </v-btn>
        </div>

        <v-card>
          <v-data-table
            :headers="headers"
            :items="pcs"
            :loading="loading"
            class="elevation-1"
          >
            <template #default:item="{ item }">
              <v-icon
                size="small"
                class="me-2"
                @click="openDialog(item)"
              >
                mdi-pencil
              </v-icon>
              <v-icon
                size="small"
                class="me-2"
                @click="openLocationHistoryDialog(item)"
              >
                mdi-map-marker-path
              </v-icon>
              <v-icon
                size="small"
                @click="confirmDelete(item)"
              >
                mdi-delete
              </v-icon>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- PC編集ダイアログ -->
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ formTitle }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="form" v-model="valid">
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-text-field
                    v-model="editedItem.managementNumber"
                    label="管理番号"
                    :rules="[required, managementNumber, minLength(3), maxLength(20)]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="editedItem.modelName"
                    label="モデル名"
                    :rules="[required, modelName, minLength(2), maxLength(50)]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-select
                    v-model="editedItem.osTypeId"
                    :items="osTypes"
                    item-title="name"
                    item-value="id"
                    label="OS種類"
                    :rules="[required]"
                    required
                  ></v-select>
                </v-col>
                <v-col cols="12">
                  <v-select
                    v-model="editedItem.currentUserId"
                    :items="users"
                    item-title="displayName"
                    item-value="id"
                    label="現在の使用者"
                    clearable
                  ></v-select>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue-darken-1" variant="text" @click="closeDialog">
            キャンセル
          </v-btn>
          <v-btn color="blue-darken-1" variant="text" @click="save">
            保存
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- 削除確認ダイアログ -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title class="text-h5">PCの削除</v-card-title>
        <v-card-text>
          このPCを削除してもよろしいですか？
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue-darken-1" variant="text" @click="deleteDialog = false">
            キャンセル
          </v-btn>
          <v-btn color="blue-darken-1" variant="text" @click="deleteItem">
            削除
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- エラーメッセージ -->
    <ErrorSnackbar
      v-model="errorDialog"
      :message="errorMessage"
    />

    <!-- 設置場所履歴ダイアログ -->
    <v-dialog v-model="locationHistoryDialog" max-width="700px">
      <v-card>
        <v-card-title>
          <span class="text-h5">設置場所履歴</span>
        </v-card-title>

        <v-card-text>
          <v-data-table
            :headers="locationHistoryHeaders"
            :items="locationHistories"
            :loading="locationHistoryLoading"
            class="elevation-1 mb-4"
          >
            <template #default:item="{ item }">
              <v-icon
                size="small"
                class="me-2"
                @click="editLocationHistory(item)"
              >
                mdi-pencil
              </v-icon>
              <v-icon
                size="small"
                @click="deleteLocationHistory(item)"
              >
                mdi-delete
              </v-icon>
            </template>
          </v-data-table>

          <v-btn color="primary" class="mt-4" @click="openLocationHistoryEditDialog()">
            <v-icon start>mdi-plus</v-icon>
            新規履歴追加
          </v-btn>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue-darken-1" variant="text" @click="locationHistoryDialog = false">
            閉じる
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- 設置場所履歴編集ダイアログ -->
    <v-dialog v-model="locationHistoryEditDialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ locationHistoryFormTitle }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="locationHistoryForm" v-model="locationHistoryValid">
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-select
                    v-model="editedLocationHistory.locationId"
                    :items="locations"
                    item-title="name"
                    item-value="id"
                    label="設置場所"
                    :rules="[v => !!v || '設置場所は必須です']"
                    required
                  ></v-select>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="editedLocationHistory.startDate"
                    label="開始日"
                    type="date"
                    :rules="[v => !!v || '開始日は必須です']"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="editedLocationHistory.endDate"
                    label="終了日"
                    type="date"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue-darken-1" variant="text" @click="closeLocationHistoryEditDialog">
            キャンセル
          </v-btn>
          <v-btn color="blue-darken-1" variant="text" @click="saveLocationHistory">
            保存
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import type { PC, OSType, User, Location, PCLocationHistory } from '@/types'
import { pcApi, osTypeApi, userApi, locationApi, pcLocationHistoryApi } from '@/api'
import ErrorSnackbar from '@/components/ErrorSnackbar.vue'
import { required, maxLength, minLength, managementNumber, modelName } from '@/validation/rules'

const router = useRouter()
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const errorDialog = ref(false)
const errorMessage = ref('')
const form = ref()
const pcs = ref<PC[]>([])
const osTypes = ref<OSType[]>([])
const users = ref<User[]>([])
const editedIndex = ref(-1)
const editedItem = ref<Partial<PC>>({})
const defaultItem = ref<Partial<PC>>({
  managementNumber: '',
  modelName: '',
  osTypeId: undefined,
  currentUserId: undefined
})

const locationHistoryDialog = ref(false)
const locationHistoryEditDialog = ref(false)
const locationHistoryLoading = ref(false)
const locationHistoryValid = ref(false)
const locationHistoryForm = ref()
const locations = ref<Location[]>([])
const locationHistories = ref<PCLocationHistory[]>([])
const editedLocationHistory = ref<Partial<PCLocationHistory>>({})
const defaultLocationHistory = ref<Partial<PCLocationHistory>>({
  locationId: undefined,
  startDate: new Date().toISOString().split('T')[0],
  endDate: null
})

const headers = [
  { title: '管理番号', key: 'managementNumber' },
  { title: 'モデル名', key: 'modelName' },
  { title: 'OS種類', key: 'osType.name' },
  { title: '現在の使用者', key: 'currentUser.displayName' },
  { title: '現在の設置場所', key: 'currentLocation.name' },
  { title: '操作', key: 'actions', sortable: false }
]

const locationHistoryHeaders = [
  { title: '設置場所', key: 'location.name' },
  { title: '開始日', key: 'startDate' },
  { title: '終了日', key: 'endDate' },
  { title: '操作', key: 'actions', sortable: false }
]

const formTitle = computed(() => {
  return editedIndex.value === -1 ? '新規PC追加' : 'PC編集'
})

const locationHistoryFormTitle = computed(() => {
  return editedLocationHistory.value.id ? '設置場所履歴の編集' : '新規設置場所履歴の追加'
})

function openDialog(item?: PC) {
  editedIndex.value = item ? pcs.value.indexOf(item) : -1
  editedItem.value = item ? { ...item } : { ...defaultItem.value }
  dialog.value = true
}

function closeDialog() {
  dialog.value = false
  editedIndex.value = -1
  editedItem.value = { ...defaultItem.value }
  form.value?.reset()
}

function confirmDelete(item: PC) {
  editedIndex.value = pcs.value.indexOf(item)
  editedItem.value = { ...item }
  deleteDialog.value = true
}

function showError(message: string) {
  errorMessage.value = message
  errorDialog.value = true
}

async function save() {
  const { valid } = await form.value?.validate()
  if (!valid) return

  try {
    if (editedIndex.value > -1) {
      // 更新処理
      const id = editedItem.value.id!
      await pcApi.update(id, editedItem.value)
      await fetchData()
    } else {
      // 新規作成処理
      const { id, createdAt, updatedAt, ...newPc } = editedItem.value
      await pcApi.create(newPc as Omit<PC, 'id'>)
      await fetchData()
    }
    closeDialog()
  } catch (error) {
    console.error('Error saving PC:', error)
    showError('PCの保存に失敗しました。')
  }
}

async function deleteItem() {
  try {
    const id = editedItem.value.id!
    await pcApi.delete(id)
    await fetchData()
    deleteDialog.value = false
  } catch (error) {
    console.error('Error deleting PC:', error)
    showError('PCの削除に失敗しました。')
  }
}

async function fetchData() {
  try {
    loading.value = true
    const [pcsResponse, osTypesResponse, usersResponse, locationsResponse] = await Promise.all([
      pcApi.getAll(),
      osTypeApi.getAll(),
      userApi.getAll(),
      locationApi.getAll()
    ])
    pcs.value = pcsResponse.data
    osTypes.value = osTypesResponse.data
    users.value = usersResponse.data
    locations.value = locationsResponse.data.filter(location => !location.isDeleted)
  } catch (error) {
    console.error('Error fetching data:', error)
    showError('データの取得に失敗しました。')
  } finally {
    loading.value = false
  }
}

async function fetchLocationHistories(pcId: number) {
  try {
    locationHistoryLoading.value = true
    const response = await pcLocationHistoryApi.getByPcId(pcId)
    locationHistories.value = response.data
  } catch (error) {
    console.error('Error fetching location histories:', error)
    showError('設置場所履歴の取得に失敗しました。')
  } finally {
    locationHistoryLoading.value = false
  }
}

function openLocationHistoryDialog(item: PC) {
  editedItem.value = { ...item }
  fetchLocationHistories(item.id!)
  locationHistoryDialog.value = true
}

function openLocationHistoryEditDialog(item?: PCLocationHistory) {
  editedLocationHistory.value = item ? { ...item } : { ...defaultLocationHistory.value, pcId: editedItem.value.id }
  locationHistoryEditDialog.value = true
}

function closeLocationHistoryEditDialog() {
  locationHistoryEditDialog.value = false
  editedLocationHistory.value = { ...defaultLocationHistory.value }
  locationHistoryForm.value?.reset()
}

async function saveLocationHistory() {
  const { valid } = await locationHistoryForm.value?.validate()
  if (!valid) return

  try {
    if (editedLocationHistory.value.id) {
      await pcLocationHistoryApi.update(editedLocationHistory.value.id, editedLocationHistory.value)
    } else {
      await pcLocationHistoryApi.create(editedLocationHistory.value as Omit<PCLocationHistory, 'id'>)
    }
    await fetchLocationHistories(editedItem.value.id!)
    closeLocationHistoryEditDialog()
  } catch (error) {
    console.error('Error saving location history:', error)
    showError('設置場所履歴の保存に失敗しました。')
  }
}

async function deleteLocationHistory(item: PCLocationHistory) {
  if (!confirm('この設置場所履歴を削除してもよろしいですか？')) return

  try {
    await pcLocationHistoryApi.delete(item.id!)
    await fetchLocationHistories(editedItem.value.id!)
  } catch (error) {
    console.error('Error deleting location history:', error)
    showError('設置場所履歴の削除に失敗しました。')
  }
}

onMounted(() => {
  fetchData()
})
</script> 