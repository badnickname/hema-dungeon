<script setup lang="ts">
import { ref } from 'vue';

const email = ref('');
const isSent = ref(false);

async function sendCode() {
  const headers = new Headers();
  headers.append('content-type', 'application/json');
  await fetch('/api/password/reset', { method: 'POST', headers, body: JSON.stringify({ email: email.value }) });
  isSent.value = true;
}
</script>

<template>
  <div v-if="!isSent" class="body">
    <h2>Сбросить пароль</h2>
    <label>
      <span>E-Mail</span>
      <input v-model="email" type="text" name="email" autocomplete="off" />
    </label>
    <hr />
    <button @click="sendCode">Запросить код</button>
  </div>
  <form v-else class="body" method="POST" action="/api/password/commit">
    <h2>Сбросить пароль</h2>
    <span>Письмо скорее всего улетело в спам. Проверьте там</span>
    <input type="hidden" name="email" :value="email" />
    <label>
      <span>Код, отправленный на почту: {{ email }}</span>
      <input type="text" name="code" autocomplete="off" />
    </label>
    <label>
      <span>Новый пароль</span>
      <input type="password" name="password" autocomplete="off" />
    </label>
    <hr />
    <button>Сохранить</button>
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
    color: #ffe071;
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
