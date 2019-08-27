export default {
    async fetchAndUnwrapJson(url) {
        return (await fetch(url)).json();
    }
};