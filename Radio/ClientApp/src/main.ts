import Vue from "vue";

import BootstrapVue from "bootstrap-vue";
// @ts-ignore
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

// @ts-ignore
import Utilities from "./utilities.js";
import router from "./router";

import App from "./App.vue";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(VueYoutube);

Vue.prototype.$utilities = Utilities;

new Vue({
  render: (h) => h(App),
  router
}).$mount("#app");
