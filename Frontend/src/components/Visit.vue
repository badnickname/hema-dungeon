<script setup lang="ts">
import { useStore } from '../store';
import { computed, onMounted, ref } from 'vue';
import type { Visit } from '../types/Visit';
import moment from 'moment';

const store = useStore();

onMounted(store.getVisits);

const dates = computed(() => Object.keys(store.visits));

const dateTime = computed(() => {
  console.warn(visitsDate.value)
  const value = convertDate(visitsDate.value ?? '').toISOString();
  console.error(value);
  return value;
});

function convertDate(date: string) {
  return moment(date, 'YYYY-MM-DD').startOf('day').utc(true);
}

const visits = ref<Visit[]>();

const visitsDate = ref<string>();

const createDate = ref(`${moment().year().toString().padStart(2, '0')}-${(moment().month() + 1).toString().padStart(2, '0')}-${moment().date().toString().padStart(2, '0')}`);

function openDate(date: string) {
  visits.value = store.visits[date];
  visitsDate.value = date;
}

function addToday(date: string) {
  visits.value = store.characters.map(x => ({
    id: x.name,
    character: x,
    date: moment('YYYY-MM-DD', date),
    wasHere: false,
    canSkip: false
  }));
  visitsDate.value = date;
}

function back() {
  visits.value = undefined
  visitsDate.value = undefined;
}
</script>

<template>
  <div v-if="!visits" class="body">
    <h2>Внести посещаемость</h2>
    <label>
      <span>Дата тренировки</span>
      <input type="date" v-model="createDate" />
    </label>
    <button @click="addToday(createDate)">Продолжить</button>
    <hr />
    <h2>Посещаемость</h2>
    <ul>
      <li v-for="date in dates">
        <button @click="openDate(date)">{{ date }}</button>
      </li>
    </ul>
    <RouterLink to="/dashboard">
      <button>Назад</button>
    </RouterLink>
  </div>
  <form v-else class="body" action="/api/admin/visits" method="POST">
    <h2>{{ visitsDate }}</h2>
    <input type="hidden" name="dateTime" :value="dateTime" />
    <ul>
      <li v-for="visit in visits">
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
          <label>
            <span>Был на занятии</span>
            <input type="checkbox" name="ids" :value="visit.character.id" :checked="visit.wasHere" />
          </label>
          <label>
            <span>Пропустил по ув. причине</span>
            <input type="checkbox" name="skipIds" :value="visit.character.id" :checked="visit.canSkip" />
          </label>
        </div>
      </li>
    </ul>
    <button>Сохранить</button>
    <a @click="back">Назад</a>
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
  background-color: #ffe071;
  height: fit-content;
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
  color: #ffe071;
  border-bottom: 2px #ffe071 solid;
}

input[type="checkbox"] {
  width: 24px;
  height: 24px;
}
</style>
