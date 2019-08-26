<template>
  <div id="app">
    <Main/>
  </div>
</template>

<script>
import { Component, Vue } from "vue-property-decorator";
import Main from "./components/Main.vue";
import BootstrapVue from "bootstrap-vue";
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

Vue.use(BootstrapVue);
Vue.use(VueYoutube);

export default {
  components: {
    Main
  }
};
</script>

<style>
html, body {
  margin: 0;
  padding: 0;
  overflow: hidden;
}

#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>