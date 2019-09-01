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
  props: ["showTrackList", "channelName", "channelDiscriminator"],
  beforeRouteEnter(to, from, next) {
    next(async (component) => {
      if (component.connection) {
        await component.negotiateChannelId();
      }
    });
  },
  async beforeRouteUpdate(to, from, next) {
      next();

      await this.updateTrackList();
      await this.negotiateChannelId();
  },
  data() {
    return {
      tracks: [],
      currentTrack: {}
    };
  },
  methods: {
    async ready() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/radio")
        .configureLogging(signalR.LogLevel.None)
        .build();

      this.connection.on("ChannelIdInfo", (channelId) => this.channelId = channelId);

      this.connection.on("SyncVideo", (trackId, timeStampSeconds) => {
        if (this.currentTrackId === trackId) {
          this.player.seekTo(timeStampSeconds);
        } else {
          this.player.loadVideoById(trackId, timeStampSeconds);
          this.currentTrackId = trackId;
          this.updateTrackList();
        }
      });

      this.connection.on("UpdateTrack", (trackId) => {
        if (this.isPlaying) {
          this.player.loadVideoById(trackId);
          this.currentTrackId = trackId;
          this.updateTrackList();
        }
      });

      await this.connection.start();
      await this.negotiateChannelId();
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
    async updateTrackList() {
      this.currentTrack = await this.$utilities.fetchAndUnwrapJson(`/track/current/${this.channelId}`);
      this.tracks = await this.$utilities.fetchAndUnwrapJson(`/track/queue/${this.channelId}`);
    },
    async negotiateChannelId() {
      await this.connection.invoke("Negotiate", this.channelName, this.channelDiscriminator);
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
  height: 92.25vh;
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