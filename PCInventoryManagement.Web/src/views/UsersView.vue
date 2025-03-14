<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center mb-4">
          <h1 class="text-h4" style="cursor: pointer" @click="$router.push('/')">ユーザー管理</h1>
          <v-btn color="primary" @click="openDialog()">
            <v-icon start>mdi-plus</v-icon>
            新規ユーザー追加
          </v-btn>
        </div>

        <v-card>
          <v-data-table
            :headers="headers"
            :items="users"
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
            <template v-slot:item.isActive="{ item }">
              <v-chip
                :color="item.isActive ? 'success' : 'error'"
                size="small"
              >
                {{ item.isActive ? '有効' : '無効' }}
              </v-chip>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- ユーザー編集ダイアログ -->
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
                    v-model="editedItem.adAccount"
                    label="ADアカウント"
                    :rules="[required, maxLength(100)]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="editedItem.displayName"
                    label="表示名"
                    :rules="[required, displayName, minLength(2), maxLength(100)]"
                    required
                  ></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-switch
                    v-model="editedItem.isActive"
                    label="有効"
                  ></v-switch>
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
        <v-card-title class="text-h5">ユーザーの削除</v-card-title>
        <v-card-text>
          このユーザーを削除してもよろしいですか？
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
import type { User } from '../types'
import { userApi } from '../api'
import ErrorSnackbar from '../components/ErrorSnackbar.vue'
import { required, maxLength, minLength, displayName } from '../validation/rules'

const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const errorDialog = ref(false)
const errorMessage = ref('')
const form = ref()
const users = ref<User[]>([])
const editedIndex = ref(-1)
const editedItem = ref<Partial<User>>({})
const defaultItem = ref<Partial<User>>({
  id: 0,
  adAccount: '',
  displayName: '',
  isActive: true,
  isDeleted: false
})

const headers = [
  { title: 'ID', key: 'id' },
  { title: 'ADアカウント', key: 'adAccount' },
  { title: '表示名', key: 'displayName' },
  { title: 'ステータス', key: 'isActive' },
  { title: '操作', key: 'actions', sortable: false }
]

const formTitle = computed(() => {
  return editedIndex.value === -1 ? '新規ユーザー追加' : 'ユーザー編集'
})

function openDialog(item?: User) {
  editedIndex.value = item ? users.value.indexOf(item) : -1
  editedItem.value = item ? { ...item } : { ...defaultItem.value }
  dialog.value = true
}

function closeDialog() {
  dialog.value = false
  editedIndex.value = -1
  editedItem.value = { ...defaultItem.value }
  form.value?.reset()
}

function confirmDelete(item: User) {
  editedIndex.value = users.value.indexOf(item)
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
      await userApi.update(id, editedItem.value)
      await fetchData()
    } else {
      // 新規作成処理
      await userApi.create(editedItem.value as Omit<User, 'id' | 'createdAt' | 'updatedAt'>)
      await fetchData()
    }
    closeDialog()
  } catch (error) {
    console.error('Error saving user:', error)
    showError('ユーザーの保存に失敗しました。')
  }
}

async function deleteItem() {
  try {
    const id = editedItem.value.id!
    await userApi.delete(id)
    await fetchData()
    deleteDialog.value = false
  } catch (error) {
    console.error('Error deleting user:', error)
    showError('ユーザーの削除に失敗しました。')
  }
}

async function fetchData() {
  try {
    loading.value = true
    const response = await userApi.getAll()
    users.value = response.data
  } catch (error) {
    console.error('Error fetching users:', error)
    showError('データの取得に失敗しました。')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})
</script> 