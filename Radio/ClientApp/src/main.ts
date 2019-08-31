import Vue from "vue";
import VueRouter from "vue-router";

import BootstrapVue from "bootstrap-vue";
// @ts-ignore
import VueYoutube from "vue-youtube";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

// @ts-ignore
import Utilities from "./utilities.js";

import App from "./App.vue";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(VueRouter);
Vue.use(VueYoutube);

Vue.prototype.$utilities = Utilities;

const main = () => import("./components/Main.vue");

new Vue({
  render: (h) => h(App),
  router: new VueRouter({
    mode: "history",
    routes: [
      {
        path: "/channel/:channelName/:channelDiscriminator(\\d{1,5})?",
        component: main,
        props: (route) => ({
          channelName: route.params.channelName,
          channelDiscriminator: route.params.channelDiscriminator || 0
        })
      },
      {
        name: "register",
        path: "/register",
        component: () => import("./components/Register.vue"),
        props: (route) => ({
          redirect: route.query.redirect || "/"
        })
      },
      {
        name: "login",
        path: "/login",
        component: () => import("./components/Login.vue"),
        props: (route) => ({
          redirect: route.query.redirect || "/"
        })
      },
      {
        path: "*",
        component: main,
        props: (route) => ({
          channelName: "__auto",
          channelDiscriminator: 0
        })
      }
    ]
  })
}).$mount("#app");
