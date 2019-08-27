import Vue from "vue";
import App from "./App.vue";

Vue.config.productionTip = false;

Vue.mixin({
  methods: {
    async fetchAndUnwrapJson(url) {
      return (await fetch(url)).json();
    }
  }
});

new Vue({
  render: (h) => h(App),
}).$mount("#app");
