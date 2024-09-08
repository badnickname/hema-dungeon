<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';

const store = useStore();
const entity = computed(() => store.character);
</script>

<template>
  <div>
    <form class="body" enctype="multipart/form-data" action="/api/avatar" method="POST">
      <h2>Портрет</h2>
      <label>
        <span>Изображение</span>
        <input type="file" name="avatar" />
      </label>
      <hr />
      <button>Сохранить</button>
    </form>
    <form class="body" action="/api/user" method="POST">
      <h2>Персонаж</h2>
      <label>
        <span>Имя</span>
        <input v-model="entity.name" type="text" name="name" autocomplete="off" />
      </label>
      <label>
        <span>Возраст</span>
        <input v-model="entity.age" type="number" name="age" autocomplete="off" />
      </label>
      <label>
        <span>Гендер</span>
        <select v-model="entity.gender" name="gender">
          <option value="male">Мужской</option>
          <option value="female">Женский</option>
          <option value="omg">Боевой вертолет Апач</option>
        </select>
      </label>
      <hr />
      <label>
        <span>История персонажа</span>
        <textarea v-model="entity.story" name="story" style="height: 300px" />
      </label>
      <hr />
      <button>Сохранить</button>
      <RouterLink to="/dashboard">
        <button>Назад</button>
      </RouterLink>
    </form>
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

  label {
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
  width: 100%;
}
h2 {
  color: #ffe071;
  border-bottom: 2px #ffe071 solid;
}
a {
  width: 100%;
  padding: 0;
  margin: 0;
}
</style>
