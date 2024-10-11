<script setup lang="ts">
import { onMounted, ref } from 'vue';
import type { Character } from '../types/Character';

const emit = defineEmits(['calculate'])

const characters = ref<Character[]>([]);

onMounted(async () => {
  characters.value = await fetch('/api/users').then(x => x.json());
  characters.value = characters.value.filter(x => !x.name.startsWith('Плотников'))
})

function pick(entity: Character) {
  entity.isPicked = !entity.isPicked;
}

function isBlocked(entity: Character) {
  return characters.value.filter(x => x.isPicked).length > 1 && !entity.isPicked;
}

function isSelected() {
  return characters.value.filter(x => x.isPicked).length > 1;
}

function calculate() {
  emit('calculate', characters.value.filter(x => x.isPicked));
}
</script>

<template>
  <label v-for="entity in characters" class="block" :style="entity.isPicked ? 'border-color: #ff6800' : ''">
    <h1 style="grid-area: name">{{ entity.name }}</h1>
    <h3 style="grid-area: author">{{ entity.author }}</h3>
    <img :src="`/api/image/${entity.avatar}`" :alt="entity.name" style="grid-area: icon" />
    <input type="checkbox" :disabled="isBlocked(entity)" :value="entity.isPicked" @change="pick(entity)" style="grid-area: pick" >
  </label>
  <div v-if="isSelected()" class="button">
    <button @click="calculate">Рассчитать</button>
  </div>
</template>

<style scoped>
.block {
  border: #f9f9f9 1px solid;
  border-radius: 12px;
  display: grid;
  grid-template-areas: 'name name' 'author author' 'pick icon';
  padding: 4px;
  width: 90px;
  height: 100px;
  img {
    height: 36px;
    width: 36px;
  }
  h1 {
    font-size: 16px;
    margin: 0;
    padding: 0;
  }
  h3 {
    font-size: 8px;
    margin: 0;
    padding: 0;
  }
}
</style>
