<script setup lang="ts">
import { useStore } from '../store';
import { computed, onMounted, ref } from 'vue';
import type { FightCharacter } from '../types/FightCharacter';
import Difference from './Difference.vue';

const store = useStore();
const characters = computed(() => store.characters);
const fightCharacters = computed(() => store.fightCharacters);
const fightStates = computed(() => store.fightStates);

const isNewFight = computed(() => store.fightCharacters.length < 1);
const isFighting = computed(() => store.fightStates.length > 0);

const names = ref<string[]>([]);
function onSelect(event: Event, entity: FightCharacter) {
  const el = document.getElementById((event.currentTarget as any).id ?? '') as HTMLInputElement;
  if (el.checked) names.value.push(entity.character.name); else names.value = names.value.filter(x => x !== entity.character.name);
}
const canSelect = computed(() => names.value.length < 2);

onMounted(store.getFight);
</script>

<template>
  <div>
    <form v-if="isNewFight" class="body" action="/api/fight/users" method="POST">
      <h2>Сражение</h2>
      <h4>Выберите персонажей</h4>
      <ul>
        <li v-for="entity in characters">
          <img :src="entity.avatar" :alt="entity.name"/>
          <div>
            <div>
              <strong>Имя: </strong>
              <span>{{ entity.name }}</span>
            </div>
            <div>
              <strong>Ранг: </strong>
              <span>{{ entity.rang }}</span>
            </div>
            <div>
              <strong>Жизни: </strong>
              <span>{{ entity.vitality.toFixed(1) }}</span>
            </div>
            <div>
              <strong>Выносливость: </strong>
              <span>{{ entity.stamina?.toFixed(1) }}</span>
            </div>
            <div class="stats">
            <span>
              <img src="../assets/power.png" width="16px" height="16px" alt="Power" />
              <span>{{ entity.power.toFixed(1) }}</span>
            </span>
              <span>
              <img src="../assets/wisdom.png" width="16px" height="16px" alt="Wisdom" />
              <span>{{ entity.wisdom.toFixed(1) }}</span>
            </span>
              <span>
              <img src="../assets/agility.png" width="16px" height="16px" alt="Agility" />
              <span>{{ entity.agility.toFixed(1) }}</span>
            </span>
            </div>
          </div>
          <label class="buttons">
            <span>Выбран</span>
            <input type="checkbox" name="ids" :value="entity.id" />
          </label>
        </li>
      </ul>
      <hr />
      <button>Начать</button>
    </form>
    <form v-else-if="!isFighting" class="body" action="/api/fight/state" method="POST">
      <h2>Сражение</h2>
      <h4>Выберите двух бойцов</h4>
      <ul>
        <li v-for="entity in fightCharacters">
          <img :src="entity.character.avatar" :alt="entity.character.name"/>
          <div>
            <div>
              <strong>Имя: </strong>
              <span>{{ entity.character.name }}</span>
            </div>
            <div>
              <strong>Ранг: </strong>
              <span>{{ entity.character.rang }}</span>
            </div>
<!--            <div>-->
<!--              <strong>Жизни: </strong>-->
<!--              <span>{{ entity.health.toFixed(1) }}</span>-->
<!--            </div>-->
            <div>
              <strong>Выносливость: </strong>
              <span>{{ entity.character.stamina?.toFixed(1) }}</span>
            </div>
            <div class="stats">
            <span>
              <img src="../assets/power.png" width="16px" height="16px" alt="Power" />
              <span>{{ entity.character.power.toFixed(1) }}</span>
            </span>
              <span>
              <img src="../assets/wisdom.png" width="16px" height="16px" alt="Wisdom" />
              <span>{{ entity.character.wisdom.toFixed(1) }}</span>
            </span>
              <span>
              <img src="../assets/agility.png" width="16px" height="16px" alt="Agility" />
              <span>{{ entity.character.agility.toFixed(1) }}</span>
            </span>
            </div>
          </div>
          <label class="buttons">
            <span>Выбран</span>
            <input :id="`${entity.character.name}`" type="checkbox" name="ids" :disabled="!(canSelect || names.includes(entity.character.name)) || entity.health < 0.001" :value="entity.character.id" @change="onSelect($event, entity)" />
          </label>
        </li>
      </ul>
      <button :disabled="canSelect">Начать бой</button>
    </form>
    <form v-else class="body" action="/api/fight/state/complete" method="POST">
      <h2>Сражение</h2>
      <h4>Укажите результаты</h4>
      <ul>
        <li v-for="(entity, i) in fightStates" class="body-fight">
          <img :src="entity.character.character.avatar" :alt="entity.character.character.name"/>
          <div>
            <div>
              <div>
                <strong>Имя: </strong>
                <span>{{ entity.character.character.name }}</span>
              </div>
              <div>
                <strong>Ранг: </strong>
                <span>{{ entity.character.character.rang }}</span>
              </div>
<!--              <div>-->
<!--                <strong>Жизни: </strong>-->
<!--                <span>{{ entity.character.health.toFixed(1) }}</span>-->
<!--              </div>-->
              <div>
                <strong>Выносливость: </strong>
                <span>{{ entity.character.character.stamina?.toFixed(1) }}</span>
              </div>
              <div class="stats">
              <span>
                <img src="../assets/power.png" width="16px" height="16px" alt="Power" />
                <span>{{ entity.character.character.power.toFixed(1) }}</span>
              </span>
                <span>
                <img src="../assets/wisdom.png" width="16px" height="16px" alt="Wisdom" />
                <span>{{ entity.character.character.wisdom.toFixed(1) }}</span>
              </span>
                <span>
                <img src="../assets/agility.png" width="16px" height="16px" alt="Agility" />
                <span>{{ entity.character.character.agility.toFixed(1) }}</span>
              </span>
              </div>
            </div>
<!--            <div>-->
<!--              <strong>Очки жизней: </strong>-->
<!--              <span>{{ entity.scoreHealth }}</span>-->
<!--            </div>-->
            <div>
              <strong>Урон: </strong>
              <span>{{ entity.damage.toFixed(1) }}</span>
            </div>
            <div v-if="entity.isOpened" class="damage">
              <strong>Расчёт урона: </strong>
              <Difference :first="entity.character" :second="fightStates[(i + 1) % 2].character" />
            </div>
            <div v-else>
              <a @click="entity.isOpened = true">А ЧТО С УРОНОМ???</a>
            </div>
            <label class="buttons">
              <strong>{{ entity.name }}<span v-if="entity.calculated">(посчитано)</span></strong>
              <span :class="entity.calculated ? 'accepted' : ''">{{ entity.description }}</span>
            </label>
          </div>
        </li>
      </ul>
      <button>Завершить бой</button>
    </form>
    <form action="/api/fight/complete" method="POST" onsubmit="return confirm('Вы уверены, что хотите прекратить сражения?')">
      <button v-if="!isNewFight">Остановить</button>
    </form>
    <RouterLink v-if="isNewFight" to="/dashboard">
      <button>Вернуться</button>
    </RouterLink>
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

  ul {
    display: flex;
    margin: 0;
    flex-direction: column;
    gap: 4px;
    align-items: baseline;
    padding: 0;

    > li {
      width: 320px;
      justify-content: space-between;
      gap: 10px;
      display: flex;
      margin-bottom: 24px;

      > img {
        max-width: 36px;
        height: fit-content;
        max-height: 50px;
      }
      > div {
        width: 100%;
        text-align: left;
      }
      > :first-child,
      > :last-child {
        flex: 1;
      }
      > :nth-child(2) {
        flex: 5;
      }
    }
  }
}

.body-fight {
  flex-direction: row;
  > div {
    display: flex;
    flex-direction: column;
    gap: 14px;
  }
  a {
    color: #ffe071;
    cursor: pointer;
  }
  .stats {
    > * {
      flex: 1 !important;
    }
  }
  .damage {
    display: flex;
    flex-direction: column;
  }
}

button {
  background-color: #ffe071;
  height: fit-content;
}

.buttons {
  display: flex;
  flex-direction: column;
  gap: 4px;
  > * {
    flex: none !important;
  }
}

h2 {
  color: #ffe071;
  border-bottom: 2px #ffe071 solid;
}

.stats {
  display: flex;
  gap: 4px;
  margin-top: 8px;
  > * {
    display: flex;
    gap: 4px;
    align-items: center;
  }
}

input[type="checkbox"] {
  width: 24px;
  height: 24px;
}

.accepted {
  color: #616161;
}
</style>
