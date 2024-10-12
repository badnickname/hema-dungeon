<script setup lang="ts">
import { computed, onMounted, PropType } from 'vue';
import type { Character } from '../types/Character';

const props = defineProps({
  health: { type: Number },
  disableAbility: {type: Boolean },
  maxHits: { type: Number },
  hits: { type: Number },
  entity: { type: Object as PropType<Character>, required: true }
});
const emit = defineEmits(['update:health', 'update:hits', 'update:disableAbility']);

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

const ability = computed({
  get: () => !props.disableAbility,
  set: (value: boolean) => emit('update:disableAbility', !value),
})

function isMinimum() {
  return hits.value < 1;
}
</script>

<template>
<div class="character">
  <h1>{{ entity.name }}</h1>
  <img :src="`/api/image/${entity.avatar}`" :alt="entity.name" />
  <label style="display: flex">
    <span>Способность</span>
    <input v-model="ability" type="checkbox" />
  </label>
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
  width: 140px;
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
  .control {
    display: flex;
    margin-top: 8px;
    flex-direction: row;
    justify-content: space-between;
  }
}
</style>
