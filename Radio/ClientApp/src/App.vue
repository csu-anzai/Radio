<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>

<script>
import { Component, Vue } from "vue-property-decorator";

export default { };
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