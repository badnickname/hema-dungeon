<script setup lang="ts">
import { useStore } from '../store';
import { computed } from 'vue';

const store = useStore();
const entity = computed(() => store.character);
</script>

<template>
  <div>
    <form class="body" enctype="multipart/form-data" action="/api/state" method="POST">
      <h2>Статы (временно так)</h2>
      <label>
        <span>Отжимания</span>
        <input v-model="entity.pushUp" type="number" name="pushUp" autocomplete="off" />
      </label>
      <label>
        <span>Подтягивания</span>
        <input v-model="entity.pullUp" type="number" name="pullUp" autocomplete="off" />
      </label>
      <label>
        <span>Пресс</span>
        <input v-model="entity.abdominal" type="number" name="abdominal" autocomplete="off" />
      </label>
      <label>
        <span>Челнок 4х14</span>
        <input v-model="entity.runTwenty" type="number" step=".01" name="runTwenty" autocomplete="off" />
      </label>
      <label>
        <span>Челнок 15 м</span>
        <input v-model="entity.runFifteen" type="number" name="runFifteen" autocomplete="off" />
      </label>
      <label>
        <span>Скакалка</span>
        <input v-model="entity.rope" type="number" name="rope" autocomplete="off" />
      </label>
      <label>
        <span>Ранг</span>
        <input v-model="entity.rang" type="number" name="rang" autocomplete="off" />
      </label>
      <label>
        <span>Баллы за ОФП</span>
        <input v-model="entity.score" type="number" name="score" autocomplete="off" />
      </label>
      <hr />
      <button>Сохранить</button>
    </form>
    <form class="body" enctype="multipart/form-data" action="/api/avatar" method="POST">
      <h2>Портрет</h2>
      <label>
        <span>Изображение</span>
        <input type="file" name="avatar" accept="image/png, image/jpeg, image/gif" />
      </label>
      <hr />
      <button>Сохранить</button>
    </form>
    <form class="body" action="/api/user" method="POST">
      <h2>Персонаж</h2>
      <label>
        <span>Автор персонажа</span>
        <input v-model="entity.author" type="text" name="author" autocomplete="off" />
      </label>
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
  background-color: #ff6800;
  color: #000;
  width: 100%;
}
h2 {
  color: #ff6800;
  border-bottom: 2px #ff6800 solid;
}
a {
  width: 100%;
  padding: 0;
  margin: 0;
}
</style>
