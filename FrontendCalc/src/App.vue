<script setup lang="ts">
import CharacterList from './components/CharacterList.vue'
import type { Character } from './types/Character';
import { ref } from 'vue';
import Calculator from './components/Calculator.vue';

const characters = ref<Character[]>([]);

function calculate(entities: Character[]) {
  characters.value = entities;
}

function needToSelect() {
  return characters.value.length < 2;
}
</script>

<template>
  <div v-if="needToSelect()" class="list">
    <CharacterList @calculate="calculate" />
  </div>
  <Calculator v-else :characters="characters" @back="characters=[]" />
</template>

<style scoped>
.list {
  max-width: 600px;
  justify-content: space-between;
  gap: 8px;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
</style>
