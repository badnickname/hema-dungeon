<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router';
import { computed, onMounted, ref } from 'vue';
import { useStore } from '../store';

const store = useStore();
const router = useRouter();
const loading = ref(true);

const isLoggedIn = computed(() => store.character.id !== '0');

const isDead = computed(() => Boolean(store.character.isDead));

const isFightStarted = ref(false);

onMounted(async () => {
  const uri = window.location.search.substring(1);
  const params = new URLSearchParams(uri);

  if (store.isRefreshed === undefined) {
    store.isRefreshed = !!params.get('dashboard');
  } else {
    store.isRefreshed = false;
  }

  const result = await store.getCharacter();
  isFightStarted.value = store.fightCharacters.length > 0;
  if (result) {
    await store.getFight();
    if (await store.getFight() && store.isRefreshed) {
      await router.replace('/fight');
    } else if (store.isRefreshed) {
      await router.replace('/dashboard');
    }
  }
  loading.value = false;
})
</script>

<template>
  <div v-if="loading" class="body">
    Загрузка...
  </div>
  <div v-else-if="!isLoggedIn" class="body">
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
  <div v-else-if="isDead">
    <div>
      <p style="color: #f9f9f9;">Вы мертвы <3</p>
      <RouterLink to="/reborn">
        <button>Переродиться!</button>
      </RouterLink>
    </div>
  </div>
  <div v-else class="body">
    <div class="icons">
      <RouterLink to="/dashboard" class="character-ico" />
      <a href="https://calc.hema-dungeon.ru/" class="fight-ico" />
      <RouterLink to="/visit" class="schedule-ico" />
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
  background-color: #ff6800;
  color: #000;
}

.icons {
  display: flex;
  justify-content: space-between;
}

.fight-ico {
  cursor: pointer;
  display: block;
  width: 100px;
  height: 100px;
  background: url('../assets/fight.png');
  background-size: contain;
}
.fight-ico:hover,
.fight-ico:active {
  background: url('../assets/fight_clicked.png');
  background-size: contain;
}

.character-ico {
  cursor: pointer;
  display: block;
  width: 100px;
  height: 100px;
  background: url('../assets/character.png');
  background-size: contain;
}
.character-ico:hover,
.character-ico:active {
  background: url('../assets/character_clicked.png');
  background-size: contain;
}

.schedule-ico {
  cursor: pointer;
  display: block;
  width: 100px;
  height: 100px;
  background: url('../assets/schedule.png');
  background-size: contain;
}
.schedule-ico:hover,
.schedule-ico:active {
  background: url('../assets/schedule_clicked.png');
  background-size: contain;
}
</style>
