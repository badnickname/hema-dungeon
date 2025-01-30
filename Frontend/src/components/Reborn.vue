<script setup lang="ts">

</script>

<template>
<form class="body" enctype="multipart/form-data" action="/api/reborn" method="POST">
  <h2>Лист персонажа</h2>
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
  <button>Переродиться</button>
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
  background-color: #ff6800;
  color: #000;
}
h2 {
  color: #ff6800;
  border-bottom: 2px #ff6800 solid;
}
</style>
