<script setup lang="ts">
import { onMounted, ref } from 'vue';
import type { Region } from '../types/Region';

function setRegion(value: Region) {
  if (!value) localStorage.removeItem('region');
  else localStorage.setItem('region', JSON.stringify(value));
  window.location.reload();
}

const regions = ref<Region[]>([]);

onMounted(async function () {
  regions.value = await fetch('/api/regions').then(x => x.json());
})

</script>

<template>
  <div class="body">
    <h2>Выберите регион</h2>
    <ul>
      <li v-for="region in regions">
        <button @click="setRegion(region)">{{ region.name }}</button>
      </li>
    </ul>
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

  ul {
    list-style-type: none;
    margin: 0;
    padding: 0;
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
