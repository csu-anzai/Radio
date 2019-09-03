export default {
    async fetchAndUnwrapJson(url) {
        return (await fetch(url)).json();
    },
    channelUrl(channel) {
        if (channel.name === "__auto") {
            return "/";
        } else if (channel.discriminator === 0) {
            return `/channel/${channel.name}`;
        } else {
            return `/channel/${channel.name}/${channel.discriminator}`;
        }
    }
};