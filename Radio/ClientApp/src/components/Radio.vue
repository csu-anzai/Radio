<template>
  <div
    class="main"
    :class="{ 'video-and-track-list': showTrackList, 'video-only': !showTrackList }"
  >
    <div class="track-list" v-if="showTrackList">
      <ul class="list-group">
        <li class="list-group-item active">{{ currentTrack.title }}</li>
        <li class="list-group-item" v-for="track in tracks">{{ track.title }}</li>
      </ul>
    </div>
    <div>
      <youtube
        ref="youtube"
        @ready="ready"
        @playing="playing"
        @paused="paused"
        width="100%"
        height="100%"
      ></youtube>
    </div>
  </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import * as signalR from "@aspnet/signalr";

export default {
  props: ["showTrackList"],
  data() {
    return {
      tracks: [],
      currentTrack: {}
    };
  },
  methods: {
    ready() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/radio")
        .build();

      this.connection.on("UpdateTrack", (trackId) => {
        if (this.isPlaying) {
          this.player.loadVideoById(trackId);
          this.currentTrackId = trackId;
          this.updateTrackList();
        }
      });

      this.connection.on("SyncVideo", (trackId, timeStampSeconds) => {
        if (this.currentTrackId === trackId) {
          this.player.seekTo(timeStampSeconds);
        } else {
          this.player.loadVideoById(trackId, timeStampSeconds);
          this.currentTrackId = trackId;
        }

        this.updateTrackList();
      });

      this.connection.start().then(() => this.connection.invoke("Connected"));
    },
    playing() {
      if (!this.isPlaying) {
        this.isPlaying = true;
        this.connection.invoke("Played");
      }
    },
    paused() {
      this.isPlaying = false;
    },
    updateTrackList() {
      fetch("/Track/Current").then((response) => {
        response.json().then((data) => {
          this.currentTrack = data;
        });
      });

      fetch("/Track/Queue").then((response) => {
        response.json().then((data) => {
          this.tracks = data;
        });
      });
    }
  },
  computed: {
    player() {
      return this.$refs.youtube.player;
    }
  }
};
</script>

<style scoped>
div.main {
  display: grid;
  grid-template-rows: 1fr;
  height: 94.25vh;
}

div.main.video-and-track-list {
  grid-template-columns: 25% 1fr;
}

div.main.video-only {
  grid-template-columns: 1fr;
}

div.track-list {
  overflow-y: scroll;
}

li.list-group-item {
  border-radius: 0px;
}
</style>