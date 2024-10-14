<script setup lang="ts">
import { computed, onUnmounted, ref } from 'vue';

const time = ref(120);

const minutes = computed(() => Math.floor(time.value / 60));

const seconds = computed(() => time.value % 60);

const isRunning = ref(false);

const isCompleted = ref(false);

const timer = setInterval(function () {
  if (!isRunning.value || isCompleted.value) return;
  time.value -= 1;
  if (time.value === 0) {
    isCompleted.value = true;
    isRunning.value = false;
    try {
      navigator.vibrate(200);
    } catch {
      console.error('Нет вибрации соре')
    }
  }
}, 1000);

function pause() {
  isRunning.value = false;
}

function start() {
  isRunning.value = true;
}

function change(value: number) {
  time.value += value;
  if (time.value <= 0) {
    time.value = 0;
    isCompleted.value = true;
    isRunning.value = false;
    try {
      navigator.vibrate(1000);
    } catch {
      console.error('Нет вибрации соре')
    }
  }
}

function restart() {
  isRunning.value = false;
  isCompleted.value = false;
  time.value = 120;
}

onUnmounted(function () {
  clearInterval(timer);
})
</script>

<template>
<div class="timer">
  <div class="dial">
    <button @click="change(-1)">
      -1с
    </button>
    <button @click="change(-30)">
      -30с
    </button>
    <h2 style="color: #f9f9f9;">{{ minutes.toString().padStart(2, '0') }} : {{ seconds.toString().padStart(2, '0') }}</h2>
    <button @click="change(30)">
      +30с
    </button>
    <button @click="change(1)">
      +1с
    </button>
  </div>
  <div class="buttons">
    <button v-if="!isRunning && !isCompleted" @click="start">
      Старт
    </button>
    <button v-else-if="!isCompleted" @click="pause">
      Пауза
    </button>
    <button v-else @click="restart">
      Рестарт
    </button>
    <slot>
      ...
    </slot>
  </div>
</div>
</template>

<style scoped>
.timer {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  padding-bottom: 4px;
  .buttons {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    gap: 12px;
    > button {
      width: 200px;
    }
  }
  .dial {
    padding-top: 4px;
    gap: 4px;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    > button {
      font-size: 16px;
      padding: 4px;
    }
    > h2 {
      padding: 0;
      margin: 0;
      font-size: 36px;
    }
  }
}
</style>
