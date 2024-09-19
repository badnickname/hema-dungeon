<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';
import { useRouter } from 'vue-router';

const store = useStore();
const router = useRouter();
const entity = computed(() => store.page!);

async function back() {
  store.page = undefined;
  await router.replace('/character');
}
</script>

<template>
  <div v-if="!entity.isEditing" class="body">
    <h2>{{ entity.name }}</h2>
    <hr />
    <p style="white-space: pre-line; text-align: left;">{{ entity.description }}</p>
    <hr />
    <button @click="back">Назад</button>
  </div>
  <div v-else class="body">
    <form class="body" enctype="multipart/form-data" action="/api/pages" method="POST">
      <input type="hidden" name="id" :value="entity.id" />
      <label>
        <span>Название главы</span>
        <input v-model="entity.name" type="text" name="name" autocomplete="off" />
      </label>
      <label>
        <span>Текст главы</span>
        <textarea v-model="entity.description" name="description" style="height: 300px" />
      </label>
      <hr />
      <button>Сохранить</button>
    </form>
    <button @click="back">Назад</button>
  </div>
</template>

<style scoped>
.body {
  background-color: #3d3d3d;
  height: 100%;
  width: 100%;
  color: #f9f9f9;

  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-top: 24px;

  hr {
    width: 100%;
  }

  > label {
    display: flex;
    flex-direction: column;
    gap: 4px;
    align-items: baseline;
    > * {
      text-align: left;
      width: 300px;
    }
  }
}

button {
  background-color: #ff6800;
  height: fit-content;
  width: 300px;
}

h2 {
  color: #ff6800;
  border-bottom: 2px #ff6800 solid;
}
</style>
