import Vue from "vue";
import App from "./App.vue";

import BootstrapVue from "bootstrap-vue";
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(VueYoutube);

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
