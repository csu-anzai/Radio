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
          <b-dropdown class="m-md-2" variant="warning">
            <template slot="button-content">
              <img v-bind:src="userImage" width="30" height="30" />
              {{ username }}
            </template>
            <b-dropdown-item @click="logOut">Log Out</b-dropdown-item>
          </b-dropdown>
        </template>
        <template v-else>
          <b-dropdown class="m-md-2" variant="warning">
            <template slot="button-content">
              <img v-bind:src="userImage" width="30" height="30" />
              {{ username }}
            </template>

            <b-dropdown-item :to="{ name: 'login', query: { redirect: $route.path } }">Login</b-dropdown-item>
            <b-dropdown-item :to="{ name: 'register', query: { redirect: $route.path } }">Register</b-dropdown-item>
          </b-dropdown>
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
import axios from "axios";

export default {
  props: ["channelName", "channelDiscriminator"],
  async beforeCreate() {
    this.isLoggedIn = await this.$utilities.fetchAndUnwrapJson(
      "/user/is-logged-in"
    );

    const { imageUrl, username } = await this.$utilities.fetchAndUnwrapJson(
      "/user/thumbnail"
    );

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
    },
    logOut() {
      axios.post("/account/logout")
      .then((response) => {
        this.$router.go();
      })
      .catch((error) => {
        this.$router.go();
      });
    }
  }
};
</script>

<style scoped>
</style>