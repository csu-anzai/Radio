import Vue from "vue";
import App from "./App.vue";

import BootstrapVue from "bootstrap-vue";
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import Utilities from "./utilities.js";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(VueYoutube);

Vue.prototype.$utilities = Utilities;

new Vue({
  render: (h) => h(App),
}).$mount("#app");
