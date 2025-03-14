<script setup lang="ts">
// コンポーネントのロジックはここに記述
</script>

<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4 mb-4">PC管理システムへようこそ</h1>
        <v-card>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="4">
                <v-card to="/pcs" class="mx-auto" hover>
                  <v-card-title>
                    <v-icon start>mdi-desktop-classic</v-icon>
                    PC管理
                  </v-card-title>
                  <v-card-text>
                    PCの一覧表示、追加、編集、削除を行います。
                  </v-card-text>
                </v-card>
              </v-col>
              <v-col cols="12" sm="4">
                <v-card to="/os-types" class="mx-auto" hover>
                  <v-card-title>
                    <v-icon start>mdi-microsoft-windows</v-icon>
                    OS種類管理
                  </v-card-title>
                  <v-card-text>
                    OS種類の一覧表示、追加、編集、削除を行います。
                  </v-card-text>
                </v-card>
              </v-col>
              <v-col cols="12" sm="4">
                <v-card to="/users" class="mx-auto" hover>
                  <v-card-title>
                    <v-icon start>mdi-account-group</v-icon>
                    ユーザー管理
                  </v-card-title>
                  <v-card-text>
                    ユーザーの一覧表示、追加、編集、削除を行います。
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<style scoped>
.v-card {
  height: 100%;
}
</style>
