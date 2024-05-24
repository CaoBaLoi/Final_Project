import { createRouter, createWebHistory } from "vue-router";
import Login from "../views/Login.vue";
import Layout from "../components/Layout.vue";
import Home from "../views/Home.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: Layout,
      children: [
        { path: "/", component: Home, name: "Home" },
        { path: "/login", component: Login, name: "Login" },
      ],
    },
  ],
});

export default router;