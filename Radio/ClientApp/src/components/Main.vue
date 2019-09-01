<template>
  <div id="app">
    <header>
      <nav class="navbar navbar-expand navbar-dark bg-dark">
        <a class="navbar-brand" href="/">
          <img src="/icon/radio.png" width="30" height="30" class="d-inline-block align-top"/>
          Radio
        </a>

        <template v-if="isChannelPage">
          <button
          type="button"
          class="btn"
          :class="{ 'btn-success': showTrackList, 'btn-danger': !showTrackList }"
          @click="toggleTrackList"
          data-toggle="button"
          aria-pressed="true"
          autocomplete="off"
        >Track List</button>
        </template>

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

        <template v-if="isLoggedIn">
          <b-dropdown class="m-md-2" variant="warning">
            <template slot="button-content">
              <img :src="userImage" width="30" height="30" />
              {{ username }}
            </template>
            <b-dropdown-item @click="logout">Log Out</b-dropdown-item>
          </b-dropdown>
        </template>
        <template v-else>
          <b-dropdown class="m-md-2" variant="warning">
            <template slot="button-content">
              <img :src="userImage" width="30" height="30" />
              {{ username }}
            </template>

            <b-dropdown-item :to="{ name: 'login', query: { redirect: $route.path } }">Login</b-dropdown-item>
            <b-dropdown-item :to="{ name: 'register', query: { redirect: $route.path } }">Register</b-dropdown-item>
          </b-dropdown>
        </template>
      </nav>
    </header>

    <router-view :showTrackList="showTrackList"></router-view>
  </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import axios from "axios";

import Radio from "./Radio.vue";

export default {
  async beforeCreate() {
    this.isLoggedIn = await this.$utilities.fetchAndUnwrapJson("/user/is-logged-in");

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
  computed: {
      isChannelPage() {
          return this.$route.meta.isChannelPage;
      }
  },
  methods: {
    toggleTrackList() {
      this.showTrackList = !this.showTrackList;
    },
    logout() {
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