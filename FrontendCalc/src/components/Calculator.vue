<script setup lang="ts">
import { onMounted, PropType, ref } from 'vue';
import Character from './Character.vue';
import type { Character as CharacterType } from '../types/Character';
import type { State } from '../types/State';
import type { FightState } from '../types/FightState';
import type { Result } from '../types/Result';
import { debounce } from 'lodash';
import Timer from './Timer.vue';

const props = defineProps({ characters: { type: Array as PropType<CharacterType[]>, required: true } });
const emit = defineEmits(['back']);

const state = ref<State>();
const fightState = ref<{ firstUser: FightState, secondUser: FightState }>();

onMounted(async function () {
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
      disableAbility: !fightState.value!.firstUser.calculated,
    },
    secondUser: {
      id: props.characters[1].id,
      damage: 0,
      health: Math.round(props.characters[1].vitality),
      score: 0,
      disableAbility: !fightState.value!.secondUser.calculated,
    },
  }
  await calculate();
});

async function rawCalculate() {
  if (!state.value || !fightState.value) return;
  const headers = new Headers();
  headers.append('Content-Type', 'application/json');
  const payload = state.value;
  const result: Result = await fetch('/api/users/calculate', { method: 'POST', headers, body: JSON.stringify(payload)}).then(x => x.json());
  fightState.value.firstUser.scoreHealth = result.firstUser.hits || 1;
  fightState.value.secondUser.scoreHealth = result.secondUser.hits || 1;
  fightState.value.firstUser.health = result.firstUser.health;
  fightState.value.secondUser.health = result.secondUser.health;
}

const calculate = debounce(rawCalculate, 800);
</script>

<template>
<div v-if="state && fightState" class="fight">
  <div style="display: flex; flex-direction: column">
    <Character
        :entity="props.characters[0]"
        v-model:health="state!.firstUser.health"
        v-model:hits="state!.firstUser.score"
        v-model:disableAbility="state!.firstUser.disableAbility"
        :max-hits="fightState?.secondUser.scoreHealth"
        :abilityName="fightState?.firstUser.name"
        @update:health="calculate"
        @update:hits="calculate"
        @update:disableAbility="calculate"
    />
    <div>
      <h3>ХП после боя:</h3>
      <span>{{ fightState.firstUser.health ?? '--' }}</span>
    </div>
  </div>
  <h1 style="margin: 0; padding: 0; font-size: 12px">VS</h1>
  <div style="display: flex; flex-direction: column">
    <Character
        :entity="props.characters[1]"
        v-model:health="state!.secondUser.health"
        v-model:hits="state!.secondUser.score"
        v-model:disableAbility="state!.secondUser.disableAbility"
        :max-hits="fightState?.firstUser.scoreHealth"
        :abilityName="fightState?.secondUser.name"
        @update:health="calculate"
        @update:hits="calculate"
        @update:disableAbility="calculate"
    />
    <div>
      <h3>ХП после боя:</h3>
      <span>{{ fightState.secondUser.health ?? '--' }}</span>
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
</style>
