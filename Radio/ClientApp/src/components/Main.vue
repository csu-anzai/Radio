<template>
  <div>
    <header>
      <nav class="navbar navbar-expand navbar-dark bg-dark">
        <a class="navbar-brand" href="/">
          <img src="/icon/radio.png" width="30" height="30" class="d-inline-block align-top" alt />
          Radio
        </a>

        <button type="button" class="btn" :class="{ 'btn-success': showTrackList, 'btn-danger': !showTrackList }" @click="toggleTrackList" data-toggle="button" aria-pressed="true" autocomplete="off">
          Track List
          </button>

        <button
          class="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarText">
          <ul class="navbar-nav mr-auto">
          </ul>
        </div>
      </nav>
    </header>
    <Radio :showTrackList="showTrackList" />
  </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import Radio from "./Radio.vue";

export default {
  data() {
    return {
      showTrackList: true
    };
  },
  components: {
    Radio
  },
  methods: {
    toggleTrackList() {
      this.showTrackList = !this.showTrackList;
    }
  }
};
</script>

<style scoped>
</style>