<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';
import type { Character } from '../types/Character';
import { useRouter } from 'vue-router';

const store = useStore();
const router = useRouter();
const characters = computed(() => store.characters);

async function open(character: Character) {
  store.visibleCharacter = character;
  await router.replace('/character');
}

function checkIsMe(character: Character) {
  return store.character.name === character.name;
}

async function edit() {
  await router.replace('/edit');
}

function getGender(gender: string) {
  switch (gender) {
    case 'male':
      return 'Мужской';
    case 'female':
      return 'Женский';
    default:
      return 'Боевой вертолет Апач';
  }
}
</script>

<template>
  <div class="body">
    <h2>Персонажи</h2>
    <ul>
      <li v-for="entity in characters">
        <img :src="entity.avatar" :alt="entity.name"/>
        <div>
          <div>
            <strong>Имя: </strong>
            <span>{{ entity.name }}</span>
          </div>
          <div>
            <strong>Пол: </strong>
            <span>{{ getGender(entity.gender) }}</span>
          </div>
          <div>
            <strong>Возраст: </strong>
            <span>{{ entity.age }}</span>
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
        <div class="buttons">
          <button @click="open(entity)">Открыть</button>
          <button v-if="checkIsMe(entity)" @click="edit">Изменить</button>
        </div>
      </li>
    </ul>
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
</style>
