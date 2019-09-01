import Vue from "vue";
import VueRouter from "vue-router";

import Main from "./components/Main.vue";

Vue.use(VueRouter);

const radio = () => import("./components/Radio.vue");

export default new VueRouter({
    mode: "history",
    routes: [
        {
            path: "/",
            component: Main,
            children: [
                {
                    path: "/channel/:channelName/:channelDiscriminator(\\d{1,5})?",
                    component: radio,
                    meta: {
                        isChannelPage: true,
                    },
                    props: (route) => ({
                        channelName: route.params.channelName,
                        channelDiscriminator: route.params.channelDiscriminator || 0
                    })
                },
                {
                    name: "register",
                    path: "/register",
                    component: () => import("./components/Register.vue"),
                    meta: {
                        isChannelPage: false,
                    },
                    props: (route) => ({
                        redirect: route.query.redirect || "/"
                    })
                },
                {
                    name: "login",
                    path: "/login",
                    component: () => import("./components/Login.vue"),
                    meta: {
                        isChannelPage: false,
                    },
                    props: (route) => ({
                        redirect: route.query.redirect || "/"
                    })
                },
                {
                    path: "*",
                    component: radio,
                    meta: {
                        isChannelPage: true,
                    },
                    props: (route) => ({
                        channelName: "__auto",
                        channelDiscriminator: 0
                    })
                }
            ]
        }
    ]
});
