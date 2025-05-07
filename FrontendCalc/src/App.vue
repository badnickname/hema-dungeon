<script setup lang="ts">
import CharacterList from './components/CharacterList.vue'
import type { Character } from './types/Character';
import { computed, ref } from 'vue';
import Calculator from './components/Calculator.vue';
import type { Region } from './types/Region';
import RegionComponent from './components/Region.vue'

const characters = ref<Character[]>([]);

function calculate(entities: Character[]) {
  characters.value = entities;
}

function needToSelect() {
  return characters.value.length < 2;
}

const region = computed(() => {
  const item = localStorage.getItem('region');
  if (!item) return null;
  return JSON.parse(item) as Region;
});

function resetRegion() {
  localStorage.removeItem('region');
  window.location.reload();
}
</script>

<template>
  <div v-if="!region">
    <RegionComponent />
  </div>
  <div v-else-if="needToSelect()" class="list">
    <CharacterList @calculate="calculate" />
  </div>
  <Calculator v-else :characters="characters" @back="characters=[]" />
  <div v-if="region && needToSelect()" class="link-1" @click="resetRegion">Сменить регион</div>
</template>

<style scoped>
.list {
  max-width: 600px;
  justify-content: center;
  gap: 8px;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
.link-1 {
  margin-top: 30px;
  color: #ff6800;
  cursor: pointer;
}
.link-1:hover {
  text-decoration: underline;
}
</style>
