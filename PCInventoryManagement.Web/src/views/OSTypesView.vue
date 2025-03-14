<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center mb-4">
          <h1 class="text-h4" style="cursor: pointer" @click="$router.push('/')">OS種類管理</h1>
          <v-btn color="primary" @click="openDialog()">
            <v-icon start>mdi-plus</v-icon>
            新規OS種類追加
          </v-btn>
        </div>

        <v-card>
          <v-data-table
            :headers="headers"
            :items="osTypes"
            :loading="loading"
            class="elevation-1"
          >
            <template v-slot:item.actions="{ item }">
              <v-icon
                size="small"
                class="me-2"
                @click="openDialog(item)"
                role="button"
                aria-label="編集"
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

    <!-- OS種類編集ダイアログ -->
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
                    v-model="editedItem.name"
                    label="OS種類名"
                    :rules="[required, modelName, minLength(2), maxLength(50)]"
                    required
                  ></v-text-field>
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
        <v-card-title class="text-h5">OS種類の削除</v-card-title>
        <v-card-text>
          このOS種類を削除してもよろしいですか？
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
import type { OSType } from '../types'
import { osTypeApi } from '../api'
import ErrorSnackbar from '../components/ErrorSnackbar.vue'
import { required, maxLength, minLength, modelName } from '../validation/rules'

const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const errorDialog = ref(false)
const errorMessage = ref('')
const form = ref()
const osTypes = ref<OSType[]>([])
const editedIndex = ref(-1)
const editedItem = ref<Partial<OSType>>({})
const defaultItem = ref<Partial<OSType>>({
  name: ''
})

const headers = [
  { title: 'OS種類名', key: 'name' },
  { title: '操作', key: 'actions', sortable: false }
]

const formTitle = computed(() => {
  return editedIndex.value === -1 ? '新規OS種類追加' : 'OS種類編集'
})

function openDialog(item?: OSType) {
  editedIndex.value = item ? osTypes.value.indexOf(item) : -1
  editedItem.value = item ? { ...item } : { ...defaultItem.value }
  dialog.value = true
}

function closeDialog() {
  dialog.value = false
  editedIndex.value = -1
  editedItem.value = { ...defaultItem.value }
  form.value?.reset()
}

function confirmDelete(item: OSType) {
  editedIndex.value = osTypes.value.indexOf(item)
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
      await osTypeApi.update(id, editedItem.value)
      await fetchData()
    } else {
      // 新規作成処理
      await osTypeApi.create(editedItem.value as Omit<OSType, 'id' | 'createdAt' | 'updatedAt'>)
      await fetchData()
    }
    closeDialog()
  } catch (error) {
    console.error('Error saving OS type:', error)
    showError('OS種類の保存に失敗しました。')
  }
}

async function deleteItem() {
  try {
    const id = editedItem.value.id!
    await osTypeApi.delete(id)
    await fetchData()
    deleteDialog.value = false
  } catch (error) {
    console.error('Error deleting OS type:', error)
    showError('OS種類の削除に失敗しました。')
  }
}

async function fetchData() {
  try {
    loading.value = true
    const response = await osTypeApi.getAll()
    osTypes.value = response.data
  } catch (error) {
    console.error('Error fetching OS types:', error)
    showError('データの取得に失敗しました。')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})
</script> 