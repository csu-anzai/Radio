<template>
  <div>
    <header>
      <nav class="navbar navbar-expand navbar-dark bg-dark">
        <a class="navbar-brand" href="/">
          <img src="/icon/radio.png" width="30" height="30" class="d-inline-block align-top" alt />
          Radio
        </a>

        <button
          type="button"
          class="btn"
          :class="{ 'btn-success': showTrackList, 'btn-danger': !showTrackList }"
          @click="toggleTrackList"
          data-toggle="button"
          aria-pressed="true"
          autocomplete="off"
        >Track List</button>

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
          <ul class="navbar-nav mr-auto"></ul>
        </div>

        <template v-if="isLoggedIn">
          <button
            type="button"
            class="btn btn-warning dropdown-toggle dropdown-toggle-split"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <img v-bind:src="userImage" width="30" height="30" />
            {{ username }}
            <span class="sr-only">Toggle Dropdown</span>
          </button>
          <div class="dropdown-menu">
            <a class="dropdown-item" href="#">Log out</a>
          </div>
        </template>
        <template v-else>
          <button
            type="button"
            class="btn btn-warning">
            <img v-bind:src="userImage" width="30" height="30" />
            {{ username }}
          </button>
        </template>
      </nav>
    </header>
    <Radio
      :showTrackList="showTrackList"
      :channelName="channelName"
      :channelDiscriminator="channelDiscriminator"
    />
  </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import Radio from "./Radio.vue";

export default {
  props: ["channelName", "channelDiscriminator"],
  async beforeCreate() {
    this.isLoggedIn = await this.$utilities.fetchAndUnwrapJson("/user/isLoggedIn");

    const { imageUrl, username } = await this.$utilities.fetchAndUnwrapJson("/user/thumbnail");

    this.userImage = imageUrl;
    this.username = username;
  },
  data() {
    return {
      showTrackList: true,
      isLoggedIn: false,
      userImage: null,
      username: null
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