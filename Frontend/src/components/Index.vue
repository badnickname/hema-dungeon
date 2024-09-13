<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router';
import { onMounted } from 'vue';
import { useStore } from '../store';

const store = useStore();
const router = useRouter();

onMounted(async () => {
  const result = await store.getCharacter();
  if (result) {
    if (await store.getFight()) {
      await router.replace('/fight');
    } else {
      await router.replace('/dashboard');
    }
  }
})
</script>

<template>
  <div class="body">
    <span>WELCOME TO DEEP DARK FANTASY</span>
    <div class="tools">
      <RouterLink to="login">
        <button>Войти</button>
      </RouterLink>
      <RouterLink to="register">
        <button>Создать персонажа</button>
      </RouterLink>
    </div>
  </div>
</template>

<style scoped>
.body {
  background-color: #3d3d3d;
  height: 100%;
  width: 100%;
  color: #f9f9f9;
}
.tools {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-top: 24px;
  > * {
    display: block;
    width: 100%;
    > button {
      width: 100%;
    }
  }
}
button {
  background-color: #ffe071;
}
</style>
