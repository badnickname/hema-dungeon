<script setup lang="ts">
import { PropType } from 'vue';
import type { FightCharacter } from '../types/FightCharacter';

const props = defineProps({ first: Object as PropType<FightCharacter>, second: Object as PropType<FightCharacter> })

const power = Math.max(props.first!.character.power - props.second!.character.power, 1);
const agility = Math.max(props.first!.character.agility - props.second!.character.agility, 1);
const wisdom = Math.max(props.first!.character.wisdom - props.second!.character.wisdom, 1);
const stamina = Math.max((props.first!.character.stamina ?? 0) - (props.second!.character.stamina ?? 0), 1);

const firstRang = props.first!.character.rang ?? 10;
const secondRang = props.second!.character.rang ?? 10;

const diffRang = firstRang < secondRang ? `${secondRang} - ${firstRang} + 1` : `${firstRang} - ${secondRang} + 1`;
</script>

<template>
<div class="stats">
  <span>( (</span>
  <span>{{ power.toFixed(1).replace('.0', '') }}</span>
  <img src="../assets/power.png" width="16px" height="16px" alt="Power" />
  <span> + </span>
  <span>{{ wisdom.toFixed(1).replace('.0', '') }}</span>
  <img src="../assets/wisdom.png" width="16px" height="16px" alt="Power" />
  <span> + </span>
  <span>{{ agility.toFixed(1).replace('.0', '') }}</span>
  <img src="../assets/agility.png" width="16px" height="16px" alt="Power" />
  <span> + </span>
  <span>{{ stamina.toFixed(1).replace('.0', '') }}</span>
  <span> ) * 5 </span>
  <span>{{ firstRang < secondRang ? '/' : '*' }}</span>
  <span>( </span>
  <span>{{ diffRang }}</span>
  <span>) </span>
</div>
</template>

<style scoped>
.stats {
  width: 300px;
  display: flex;
  flex-direction: row;
  align-items: center;
  > span {
    display: ruby;
  }
}
</style>
