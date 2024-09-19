<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';
import type { Page } from '../types/Page';
import { useRouter } from 'vue-router';
import type { Character } from '../types/Character';

const store = useStore();
const router = useRouter();
const entity = computed(() => store.visibleCharacter!);

async function openPage(page: Page) {
  store.page = page;
  page.isEditing = false;
  await router.replace('/page');
}

async function editPage(page: Page) {
  store.page = page;
  page.isEditing = true;
  await router.replace('/page');
}

async function createPage() {
  store.page = { isEditing: true };
  await router.replace('/page');
}

async function removePage(page: Page) {
  if (!confirm('Вы действительно хотите удалить страницу?')) return;
  await store.removePage(page);
  store.visibleCharacter!.pages = store.visibleCharacter!.pages?.filter(x => x.id !== page.id);
}

function checkIsMe(character: Character) {
  return store.character.name === character.name;
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
    <h2>{{ entity.name }}</h2>
    <img :src="entity.avatar" style="max-width: 300px" :alt="entity.name" />
    <hr />
    <div class="stats">
      <span>
        <strong>Ранг: </strong>
        <span>{{ entity.rang }}</span>
      </span>
      <span>
        <strong>Жизни: </strong>
        <span>{{ entity.vitality.toFixed(1) }}</span>
      </span>
      <span>
        <strong>Выносливость: </strong>
        <span>{{ entity.stamina?.toFixed(1) }}</span>
      </span>
      <span>
        <img src="../assets/power.png" width="16px" height="16px" alt="Power"/>
        <span>{{ entity.power.toFixed(1) }}</span>
      </span>
      <span>
        <img src="../assets/wisdom.png" width="16px" height="16px" alt="Wisdom"/>
        <span>{{ entity.wisdom.toFixed(1) }}</span>
      </span>
      <span>
        <img src="../assets/agility.png" width="16px" height="16px" alt="Agility"/>
        <span>{{ entity.agility.toFixed(1) }}</span>
      </span>
    </div>
    <hr />
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
    <hr />
    <div>
      <strong>Биография: </strong>
      <span style="white-space: pre-line;">{{ entity.story }}</span>
    </div>
    <hr />
    <div>
      <strong>История</strong>
      <ul class="pages">
        <li v-for="page in (entity.pages ?? [])">
          <div>
            <a @click="openPage(page)">{{ page.name }}</a>
          </div>
          <div v-if="checkIsMe(entity)">
            <button @click="editPage(page)">Изменить</button>
            <button @click="removePage(page)">Удалить</button>
          </div>
        </li>
      </ul>
    </div>
    <button v-if="checkIsMe(entity)" @click="createPage">Добавить страницу</button>
    <hr />
    <div>
      <strong>Автор персонажа: </strong>
      <span>{{ entity.author ?? '--' }}</span>
    </div>
  </div>
  <hr />
  <RouterLink to="/dashboard">
    <button>Назад</button>
  </RouterLink>
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

  > div {
    display: flex;
    flex-direction: column;
    gap: 4px;
    align-items: baseline;
    > * {
      text-align: left;
      width: 300px;
    }
  }
}

button {
  background-color: #ffe071;
  height: fit-content;
  width: 300px;
}

.pages {
  display: flex;
  flex-direction: column;
  margin: 0;
  padding: 0;
  gap: 14px;
  > li {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    gap: 10px;
    a {
      color: #ffe071;
      cursor: pointer;
    }
  }
  button {
    width: auto !important;
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
  flex-direction: row !important;
  justify-content: space-between;
  align-items: center !important;
  > * {
    display: flex;
    gap: 4px;
    align-items: center;
    width: auto !important;
  }
}
</style>
