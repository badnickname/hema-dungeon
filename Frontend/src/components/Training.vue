<script setup lang="ts">
import { useStore } from '../store';
import { onMounted, ref } from 'vue';
import type { Visit } from '../types/Visit';
import moment from 'moment';

const store = useStore();

onMounted(() => {
  users.value = store.characters.map(x => ({
    id: x.name,
    character: x,
    date: moment(),
    wasHere: false,
    canSkip: false
  }))
});

const users = ref<Visit[]>();

const visitsDate = ref<string>();

const createDate = ref(`${moment().year().toString().padStart(2, '0')}-${(moment().month() + 1).toString().padStart(2, '0')}-${moment().date().toString().padStart(2, '0')}`);
</script>

<template>
  <form class="body" action="/api/trainings" method="POST">
    <h2>Начать тренировку - {{ visitsDate }}</h2>
    <label>
      <span>Дата тренировки</span>
      <input type="date" v-model="createDate" name="dateTime" />
    </label>
    <ul>
      <li v-for="(visit, i) in users">
        <img :src="visit.character.avatar" :alt="visit.character.name"/>
        <div>
          <div>
            <strong>Имя: </strong>
            <span>{{ visit.character.name }}</span>
          </div>
          <div>
            <strong>Автор: </strong>
            <span>{{ visit.character.author }}</span>
          </div>
        </div>
        <div class="checkboxes">
          <input type="hidden" :name="`users[${i}].id`" :value="visit.character.id" />
          <label>
            <span>Присутствует</span>
            <input type="checkbox" :name="`users[${i}].wasHere`" value="true" :checked="visit.wasHere" />
          </label>
          <label>
            <span>Отсутствует по ув. причине</span>
            <input type="checkbox" :name="`users[${i}].skip`" value="true" :checked="visit.canSkip" />
          </label>
        </div>
        <div class="checkboxes">
          <label>
            <span>Урон, полученный от хлыста</span>
            <input type="number" :name="`users[${i}].damage`" value="0" />
          </label>
        </div>
      </li>
    </ul>
    <button>Начать тренировку</button>
    <RouterLink to="/">Назад</RouterLink>
  </form>
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

button {
  background-color: #ff6800;
  height: fit-content;
  color: #000;
  max-height: 50px;
}

.checkboxes {
  display: flex;
  flex-direction: row;
  gap: 4px;
  > * {
    display: flex;
    flex-direction: column;
    gap: 4px;
    > * {
      flex: none !important;
    }
  }
}

h2 {
  color: #ff6800;
  border-bottom: 2px #ff6800 solid;
}

input[type="checkbox"] {
  width: 24px;
  height: 24px;
}
</style>
