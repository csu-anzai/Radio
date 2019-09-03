<template>
  <b-list-group>
    <b-list-group-item v-for="channel in channels" class="flex-column align-items-start">
      <div class="d-flex w-100 justify-content-between">
        <h5 class="mb-1">
          <router-link :to="channel.url">{{ channel.name === '__auto' ? 'default' : channel.name }}</router-link>
        </h5>
        
        <small>
            <img :src="channel.owner.imageUrl" width="30" height="30"/>
            {{ channel.owner.username }}
        </small>
      </div>

      <p class="mb-1">{{ channel.description }}</p>
    </b-list-group-item>
  </b-list-group>
</template>

<style scoped>
</style>

<script>
export default {
  async beforeCreate() {
    const channels = await this.$utilities.fetchAndUnwrapJson("/channels/all");

    this.channels = channels.map((channel) => {
        channel.url = this.$utilities.channelUrl(channel);
        return channel;
    });
  },
  data() {
    return {
      channels: []
    };
  }
};
</script>