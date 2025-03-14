<script setup lang="ts">
import { ref, computed } from 'vue'

const props = defineProps<{
  modelValue: boolean
  message: string
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void
}>()

const show = computed({
  get: () => props.modelValue,
  set: (value: boolean) => emit('update:modelValue', value)
})
</script>

<template>
  <v-snackbar
    v-model="show"
    :timeout="5000"
    color="error"
  >
    {{ message }}
  </v-snackbar>
</template>

<script lang="ts">
export default {
  name: 'ErrorSnackbar'
}
</script> 