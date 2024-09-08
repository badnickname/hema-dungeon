<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';

const store = useStore();
const entity = computed(() => store.visibleCharacter!);

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
      <span>{{ entity.story }}</span>
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

h2 {
  color: #ffe071;
  border-bottom: 2px #ffe071 solid;
}
</style>
