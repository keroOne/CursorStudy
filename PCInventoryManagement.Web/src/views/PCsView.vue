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
            <template v-slot:item.actions="{ item }">
              <v-icon
                size="small"
                class="me-2"
                @click="openDialog(item)"
              >
                mdi-pencil
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
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import type { PC, OSType, User } from '../types'
import { pcApi, osTypeApi, userApi } from '../api'
import ErrorSnackbar from '../components/ErrorSnackbar.vue'
import { required, maxLength, minLength, managementNumber, modelName } from '../validation/rules'

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

const headers = [
  { title: '管理番号', key: 'managementNumber' },
  { title: 'モデル名', key: 'modelName' },
  { title: 'OS種類', key: 'osType.name' },
  { title: '現在の使用者', key: 'currentUser.displayName' },
  { title: '操作', key: 'actions', sortable: false }
]

const formTitle = computed(() => {
  return editedIndex.value === -1 ? '新規PC追加' : 'PC編集'
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
      await pcApi.create(editedItem.value as Omit<PC, 'id' | 'createdAt' | 'updatedAt'>)
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
    const [pcsResponse, osTypesResponse, usersResponse] = await Promise.all([
      pcApi.getAll(),
      osTypeApi.getAll(),
      userApi.getAll()
    ])
    pcs.value = pcsResponse.data
    osTypes.value = osTypesResponse.data
    users.value = usersResponse.data
  } catch (error) {
    console.error('Error fetching data:', error)
    showError('データの取得に失敗しました。')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})
</script> 