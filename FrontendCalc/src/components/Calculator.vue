<script setup lang="ts">
import { onMounted, onUnmounted, PropType, ref } from 'vue';
import Character from './Character.vue';
import type { Character as CharacterType } from '../types/Character';
import { State, User } from '../types/State';
import { debounce } from 'lodash';
import Timer from './Timer.vue';
import { CalculateResult } from '../types/CalculateResult';
import { Spell } from '../types/Spell';

const props = defineProps({ characters: { type: Array as PropType<CharacterType[]>, required: true } });
const emit = defineEmits(['back']);

const state = ref<State>();
const fightState = ref<CalculateResult>();

const sentiel = ref<WakeLockSentinel>();

onMounted(async function () {
  try {
    if ('wakeLock' in navigator) {
      sentiel.value = await navigator.wakeLock.request('screen');
    }
  } catch {
    console.log('No WakeLock :c ')
  }
  const payload = {
    firstUser: { id: props.characters[0].id },
    secondUser: { id: props.characters[1].id },
  }
  const headers = new Headers();
  headers.append('Content-Type', 'application/json');
  fightState.value = await fetch('/api/users/compare', { method: 'POST', headers, body: JSON.stringify(payload)}).then(x => x.json());
  state.value = {
    firstUser: {
      id: props.characters[0].id,
      damage: 0,
      health: Math.round(props.characters[0].vitality),
      score: 0,
      spells: fightState.value!.firstUser.spells,
    },
    secondUser: {
      id: props.characters[1].id,
      damage: 0,
      health: Math.round(props.characters[1].vitality),
      score: 0,
      spells: fightState.value!.secondUser.spells,
    },
  }
  await calculate();
});

async function rawCalculate() {
  if (!state.value || !fightState.value) return;
  const headers = new Headers();
  headers.append('Content-Type', 'application/json');
  const payload = state.value;
  const result: CalculateResult = await fetch('/api/users/calculate', { method: 'POST', headers, body: JSON.stringify(payload)}).then(x => x.json());
  fightState.value.firstUser.scoreHealth = result.firstUser.scoreHealth || 1;
  fightState.value.secondUser.scoreHealth = result.secondUser.scoreHealth || 1;
  fightState.value.firstUser.health = result.firstUser.health;
  fightState.value.secondUser.health = result.secondUser.health;
  fightState.value.firstUser.spells = result.firstUser.spells;
  fightState.value.secondUser.spells = result.secondUser.spells;
}

function updateSpell(user: User, spells: Spell[]) {
  user.spells = spells;
  return calculate()
}

const calculate = debounce(rawCalculate, 800);

onUnmounted(function () {
  try {
    if (sentiel.value) sentiel.value.release();
  } catch {
    console.log('No WakeLock :c ')
  }
})
</script>

<template>
<div v-if="state && fightState" class="fight">
  <div style="display: flex; flex-direction: column">
    <Character
        :entity="props.characters[0]"
        v-model:health="state!.firstUser.health"
        v-model:hits="state!.firstUser.score"
        :max-hits="fightState?.secondUser.scoreHealth"
        :spells="fightState?.firstUser.spells"
        @update:health="calculate"
        @update:hits="calculate"
        @update:spells="updateSpell(state!.firstUser, $event)"
    />
    <div>
      <h3>ХП после боя:</h3>
      <span class="size">{{ fightState.firstUser.health ?? '--' }}</span>
    </div>
  </div>
  <h1 style="margin: 0; padding: 0; font-size: 12px">VS</h1>
  <div style="display: flex; flex-direction: column">
    <Character
        :entity="props.characters[1]"
        v-model:health="state!.secondUser.health"
        v-model:hits="state!.secondUser.score"
        :max-hits="fightState?.firstUser.scoreHealth"
        :spells="fightState?.secondUser.spells"
        @update:health="calculate"
        @update:hits="calculate"
        @update:spells="updateSpell(state!.secondUser, $event)"
    />
    <div>
      <h3>ХП после боя:</h3>
      <span class="size">{{ fightState.secondUser.health ?? '--' }}</span>
    </div>
  </div>
</div>
<div v-else>
  Загрузка...
</div>
<div style="height: 200px;"></div>
<div class="button">
  <Timer>
    <button style="width: 200px;" @click="emit('back')">Назад</button>
  </Timer>
</div>
</template>

<style scoped>
.fight {
  display: flex;
  justify-content: space-between;
  flex-direction: row;
  align-items: center;
}
.size {
  font-size: 22px;
}
</style>
