<script setup lang="ts">

</script>

<template>
  <form class="body" action="/api/sign-in" method="POST">
    <h2>Вход</h2>
    <label>
      <span>E-Mail</span>
      <input type="text" name="email" autocomplete="off" />
    </label>
    <label>
      <span>Пароль</span>
      <input type="password" name="password" autocomplete="off" />
    </label>
    <hr />
    <button>Войти</button>
    <RouterLink to="/password">Сбросить пароль</RouterLink>
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

  a {
    color: #ff6800;
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
