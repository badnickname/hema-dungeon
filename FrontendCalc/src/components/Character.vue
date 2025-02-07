<script setup lang="ts">
import { computed, nextTick, onMounted, PropType } from 'vue';
import type { Character } from '../types/Character';
import type { Spell } from '../types/Spell';

const props = defineProps({
  health: { type: Number },
  spells: { type: Array as PropType<Spell[]>, required: true },
  maxHits: { type: Number },
  hits: { type: Number },
  entity: { type: Object as PropType<Character>, required: true }
});
const emit = defineEmits(['update:health', 'update:hits', 'update:spells']);

onMounted(() => {
  if (!props.health) emit('update:health', props.entity?.vitality ?? 0);
  if (!props.hits) emit('update:hits', 0);
})

const health = computed({
  get: () => props.health ?? 0,
  set: (value: number) => emit('update:health', value),
})

const hits = computed({
  get: () => props.hits ?? 0,
  set: (value: number) => emit('update:hits', value),
})

function updateSpells() {
  nextTick(() => emit('update:spells', props.spells))
}

function isMinimum() {
  return hits.value < 1;
}
</script>

<template>
<div class="character">
  <h1>{{ entity.name }}</h1>
  <img :src="`/api/image/${entity.avatar}`" :alt="entity.name" />
  <div>
    <div v-for="spell in spells" :key="spell.key" class="ability">
      <label>
        <span>{{ spell.key }}</span>
        <input v-if="spell.type === 'P'" v-model="spell.value" class="checkbox" type="checkbox" :true-value="1" :false-value="0" @click="updateSpells" />
        <input v-else v-model="spell.value" type="number" @input="updateSpells" />
      </label>
      <span class="description">{{ spell.description }}</span>
    </div>
  </div>
  <label>
    <span>Жизни</span>
    <div class="health">
      <input type="number" v-model="health" />
      <span>из {{ Math.round(entity.vitality) }}</span>
    </div>
  </label>
  <div>
    <label>
      <span>Очки</span>
      <div class="health">
        <input type="number" v-model="hits" />
        <span>из {{ maxHits }}</span>
      </div>
    </label>
    <div class="control">
      <button @click="hits = (hits ?? 0) - 1" :disabled="isMinimum()">-</button>
      <button @click="hits = (hits ?? 0) + 1">+</button>
    </div>
  </div>
</div>
</template>

<style scoped>
.character {
  display: flex;
  flex-direction: column;
  width: 200px;
  gap: 4px;
  padding: 10px;
  border: #f9f9f9 1px solid;
  border-radius: 12px;
  img {
    width: 120px;
    height: 120px;
  }
  h1 {
    font-size: 16px;
    margin: 0;
    padding: 0;
  }
  input {
    width: 100%;
  }
  label {
    > span {
      color: #ff6800;
    }
  }
  .health {
    display: flex;
    > * {
      flex: 1;
    }
  }
  .ability {
    display: flex;
    flex-direction: column;
    > label {
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: space-between;
      gap: 4px;
      > input {
        max-width: 40px;
      }
    }
    > span {
      max-height: 50px;
      text-overflow: ellipsis;
      overflow: hidden;
    }
  }
  .checkbox {
    width: 25px;
    height: 25px;
  }
  .description {
    font-size: 9px;
    color: white;
  }
  .control {
    display: flex;
    margin-top: 4px;
    flex-direction: row;
    justify-content: space-between;
  }
}
</style>
