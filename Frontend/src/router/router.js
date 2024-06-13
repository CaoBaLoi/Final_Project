import { createRouter, createWebHistory } from "vue-router";
import Layout from "../components/Layout.vue";
import Home from "../views/Home.vue";
import LayoutDashborad from "../components/LayoutDashboard.vue"

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      component: Layout,
      children: [
        { path: "/", component: Home, name: "home" },
        { path :"/home", component: Home, name: "home"},
      ],
    },
    { path: "/login", component: () => import("../views/accounts/Login.vue"), name: "login" },
    { path: "/register", component: () => import("../views/accounts/Register.vue"), name: "register" },
    { 
      path: "/dashboard",
      component: LayoutDashborad,
      name: "dashborad"
    },
    {
      path: "/category",
      component: LayoutDashborad,
      children: [
        { path: "", component: () => import("../views/categories/index.vue"), name: "category.index"},
        { path: "create", component: ()=> import("../views/categories/form.vue"), name: "category.create"}
      ]
    },
    {
      path: "/product",
      component: LayoutDashborad,
      children: [
        { path: "", component: () => import("../views/products/index.vue"), name: "product.index"},
        { path: "create", component: ()=> import("../views/products/form.vue"), name: "product.create"},
      ]
    }
  ],
});

export default router;