import Vue from "vue";
import VueRouter from "vue-router";
import App from "./App.vue";

import BootstrapVue from "bootstrap-vue";
// @ts-ignore
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

// @ts-ignore
import Utilities from "./utilities.js";

import Main from "./components/Main.vue";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(VueRouter);
Vue.use(VueYoutube);

Vue.prototype.$utilities = Utilities;

new Vue({
  render: (h) => h(App),
  router: new VueRouter({
    mode: "history",
    routes: [
      {
        path: "/channel/:channelName/:channelDiscriminator(\\d{1,5})?",
        component: Main,
        props: (route) => ({
          channelName: route.params.channelName,
          channelDiscriminator: route.params.channelDiscriminator || 0
        })
      },
      {
        path: "*",
        component: Main,
        props: (route) => ({
          channelName: "__auto",
          channelDiscriminator: 0
        })
      }
    ]
  })
}).$mount("#app");
