<script setup lang="ts">
import { RouterView, useRouter } from 'vue-router';
import { computed } from 'vue';

const router = useRouter();

const isMainPage = computed(() => router.currentRoute.value.name === 'index');

function resetRegion() {
  localStorage.removeItem('region');
  window.location.reload();
}

</script>

<template>
  <img src="./assets/logo.png" alt="logo" />
<RouterView></RouterView>
  <div v-if="isMainPage" class="link-1" @click="resetRegion">Сменить регион</div>
</template>

<style scoped>
.body {
  background-color: #3d3d3d;
  height: 100%;
  width: 100%;
  color: #f9f9f9;
}
.link-1 {
  margin-top: 30px;
  color: #ff6800;
  cursor: pointer;
}
.link-1:hover {
  text-decoration: underline;
}
</style>
