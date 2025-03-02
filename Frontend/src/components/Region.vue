<script setup lang="ts">
import { useStore } from '../store';
import { useRouter } from 'vue-router';
import { computed, onMounted } from 'vue';
import type { Region } from '../types/Region';
const store = useStore();
const router = useRouter();

onMounted(() => store.getRegions());

const regions = computed(() => store.regions);

function setRegion(region: Region) {
    store.region = region;
    router.replace({ name: 'index' });
    window.location.reload();
}
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
