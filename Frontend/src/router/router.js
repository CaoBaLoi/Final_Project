import { createRouter, createWebHistory } from "vue-router";
import Layout from "../components/Layout.vue";
import Home from "../views/Home.vue";
import LayoutDashboard from "../components/LayoutDashboard.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: Layout,
      children: [
        { path: "/", component: Home, name: "home" },
        { path: "/home", component: Home, name: "home" },
        { path: "/product/:id", component: () => import("../views/products/detail.vue"), name: "product.detail", props: route => ({id: Number(route.params.id)}) }
      ],
    },
    { path: "/login", component: () => import("../views/accounts/Login.vue"), name: "login" },
    { path: "/register", component: () => import("../views/accounts/Register.vue"), name: "register" },
    {
      path: "/dashboard",
      component: LayoutDashboard,
      name: "dashboard",
      meta: { requiresAuth: true, requiresRole: 'Admin' } // Đánh dấu route cần xác thực và yêu cầu role 'Admin'
    },
    {
      path: "/category",
      component: LayoutDashboard,
      children: [
        { path: "", component: () => import("../views/categories/index.vue"), name: "category.index" },
        { path: "create", component: () => import("../views/categories/form.vue"), name: "category.create" }
      ]
    },
    {
      path: "/product",
      component: LayoutDashboard,
      children: [
        { path: "", component: () => import("../views/products/index.vue"), name: "product.index" },
        { path: "create", component: () => import("../views/products/form.vue"), name: "product.create" },
      ]
    },
  ],
});

router.beforeEach((to,from, next) => {
  if (to.meta.requiresAuth || to.meta.requiresRole) {
    const loggedIn = localStorage.getItem('loggedIn') === 'true';
    const role = localStorage.getItem('role');

    if (to.meta.requiresAuth && !loggedIn) {
      next('/login');
    } else if (to.meta.requiresRole && role !== 'Admin') {
      next('/');
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
