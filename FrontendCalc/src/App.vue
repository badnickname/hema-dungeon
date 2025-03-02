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
</script>

<template>
  <div v-if="!region">
    <RegionComponent />
  </div>
  <div v-else-if="needToSelect()" class="list">
    <CharacterList @calculate="calculate" />
  </div>
  <Calculator v-else :characters="characters" @back="characters=[]" />
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
</style>
