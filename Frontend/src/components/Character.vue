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
    <div class="stats">
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
      <span style="white-space: pre;">{{ entity.story }}</span>
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
