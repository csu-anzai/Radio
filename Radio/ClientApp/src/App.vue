<template>
  <div id="app">
    <Main/>
  </div>
</template>

<script>
import { Component, Vue } from "vue-property-decorator";
import Main from "./components/Main.vue";

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