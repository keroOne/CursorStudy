<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Location } from '@/types'
import { locationApi } from '@/api'

const locations = ref<Location[]>([])
const loading = ref(true)
const dialog = ref(false)
const editedLocation = ref<Location | null>(null)
const isEditing = ref(false)

const headers = [
  { title: '拠点コード', key: 'code' },
  { title: '拠点名', key: 'name' },
  { title: '操作', key: 'actions', sortable: false }
]

const defaultLocation: Omit<Location, 'id'> = {
  code: '',
  name: '',
  isDeleted: false
}

onMounted(async () => {
  await fetchLocations()
})

const fetchLocations = async () => {
  loading.value = true
  try {
    const response = await locationApi.getAll()
    locations.value = response.data.filter(location => !location.isDeleted)
  } catch (error) {
    console.error('拠点の取得に失敗しました:', error)
  } finally {
    loading.value = false
  }
}

const editLocation = (location: Location) => {
  editedLocation.value = { ...location }
  isEditing.value = true
  dialog.value = true
}

const createLocation = () => {
  editedLocation.value = { ...defaultLocation } as Location
  isEditing.value = false
  dialog.value = true
}

const deleteLocation = async (location: Location) => {
  if (!confirm(`拠点「${location.name}」を削除してもよろしいですか？`)) return

  try {
    await locationApi.delete(location.id)
    await fetchLocations()
  } catch (error) {
    console.error('拠点の削除に失敗しました:', error)
  }
}

const save = async () => {
  if (!editedLocation.value) return

  try {
    if (isEditing.value) {
      await locationApi.update(editedLocation.value.id, editedLocation.value)
    } else {
      await locationApi.create(editedLocation.value)
    }
    dialog.value = false
    await fetchLocations()
  } catch (error) {
    console.error('拠点の保存に失敗しました:', error)
  }
}
</script>

<template>
  <div>
    <h1>拠点管理</h1>
    
    <v-card>
      <v-card-title>
        <v-row align="center">
          <v-col cols="8">
            <span class="text-h5">拠点一覧</span>
          </v-col>
          <v-col cols="4" class="text-right">
            <v-btn color="primary" @click="createLocation">
              <v-icon>mdi-plus</v-icon>
              新規作成
            </v-btn>
          </v-col>
        </v-row>
      </v-card-title>

      <v-card-text>
        <v-data-table
          :headers="headers"
          :items="locations"
          :loading="loading"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn icon @click="editLocation(item)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon @click="deleteLocation(item)" color="error">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ isEditing ? '拠点の編集' : '新規拠点の作成' }}</span>
        </v-card-title>

        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="editedLocation.code"
                  label="拠点コード"
                  required
                  :rules="[v => !!v || '拠点コードは必須です']"
                  maxlength="10"
                  counter
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="editedLocation.name"
                  label="拠点名"
                  required
                  :rules="[v => !!v || '拠点名は必須です']"
                  maxlength="20"
                  counter
                ></v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="error" @click="dialog = false">キャンセル</v-btn>
          <v-btn color="primary" @click="save">保存</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template> 