<template>
  <v-app class="app">
    <app-header></app-header>
    <router-view></router-view>
    <app-loader></app-loader>
    <app-toast></app-toast>
  </v-app>
</template>

<script>
import AppHeader from "./components/Header.vue";
import Loader from "./components/Loader.vue";
import Toast from "./components/Toast.vue";

export default {
  name: "App",
  components: {
    "app-header": AppHeader,
    "app-loader": Loader,
    "app-toast": Toast,
  },
};
</script>

<style>
@import url("https://fonts.googleapis.com/css2?family=Alegreya:ital,wght@0,400..900;1,400..900&family=Winky+Rough:ital,wght@0,300..900;1,300..900&display=swap");

body {
  font-family: "Alegreya", serif;
}

.app {
  position: relative;
}
</style>
