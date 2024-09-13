<script setup lang="ts">

</script>

<template>
<form class="body" enctype="multipart/form-data" action="/api/sign-up" method="POST">
  <h2>Лист персонажа</h2>
  <label>
    <span>E-Mail</span>
    <input type="text" name="email" autocomplete="off" />
  </label>
  <label>
    <span>Пароль</span>
    <input type="password" name="password" autocomplete="off" />
  </label>
  <hr />
  <label>
    <span>Портрет</span>
    <input type="file" name="avatar" accept="image/png, image/jpeg, image/gif" />
  </label>
  <hr />
  <label>
    <span>Имя</span>
    <input type="text" name="name" autocomplete="off" />
  </label>
  <label>
    <span>Возраст</span>
    <input type="number" name="age" autocomplete="off" />
  </label>
  <label>
    <span>Гендер</span>
    <select name="gender">
      <option value="male">Мужской</option>
      <option value="female">Женский</option>
      <option value="omg">Боевой вертолет Апач</option>
    </select>
  </label>
  <hr />
  <label>
    <span>История персонажа</span>
    <textarea name="story" style="height: 300px" />
  </label>
  <hr />
  <label>
    <span>Отжимания</span>
    <input type="number" name="pushUp" autocomplete="off" />
  </label>
  <label>
    <span>Подтягивания</span>
    <input type="number" name="pullUp" autocomplete="off" />
  </label>
  <label>
    <span>Челнок (секунды)</span>
    <input type="number" step=".01" name="runTwenty" autocomplete="off" />
  </label>
  <label>
    <span>Скакалка</span>
    <input type="number" name="rope" autocomplete="off" />
  </label>
  <label>
    <span>Ранг</span>
    <input type="number" name="rang" autocomplete="off" />
  </label>
  <label>
    <span>ОФП</span>
    <input type="number" name="score" autocomplete="off" />
  </label>
  <hr />
  <button>Создать</button>
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
}
h2 {
  color: #ffe071;
  border-bottom: 2px #ffe071 solid;
}
</style>
