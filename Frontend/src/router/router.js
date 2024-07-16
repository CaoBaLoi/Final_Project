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
        { path: "/home", component: Home, name: "home" },
        { path: "/product/:id", component: () => import("../views/products/detail.vue"), name: "product.detail", props: route => ({id: Number(route.params.id)}) }
      ],
    },
    { path: "/login", component: () => import("../views/accounts/Login.vue"), name: "login" },
    { path: "/register", component: () => import("../views/accounts/Register.vue"), name: "register" },
    { path: "/forgot-pass", component: () => import("../views/accounts/forgotpass.vue"), name: "forgotpass" },
    { path: "/reset-pass", component: () => import("../views/accounts/resetpass.vue"), name: "resetpass" },
    {
      path: "/dashboard",
      component: LayoutDashboard,
      meta: { requiresAuth: true, requiresRole: 'Admin' },
      children:[
        { path: "", component: () => import("../views/Dashboard.vue"), name: "dashboard" },
      ]
    },
    {
      path: "/category",
      component: LayoutDashboard,
      meta: { requiresAuth: true, requiresRole: 'Admin' },
      children: [
        { path: "", component: () => import("../views/categories/index.vue"), name: "category.index" },
        { path: "create", component: () => import("../views/categories/form.vue"), name: "category.create" }
      ]
    },
    {
      path: "/product",
      component: LayoutDashboard,
      meta: { requiresAuth: true, requiresRole: 'Admin' },
      children: [
        { path: "", component: () => import("../views/products/index.vue"), name: "product.index" },
        { path: "create", component: () => import("../views/products/form.vue"), name: "product.create" },
      ]
    },
    {
      path: "/user",
      component: () => import("../components/LayoutProfile.vue"),
      meta: { requiresAuth: true, requiresRole: 'User' },
      children: [
        { path: "profile", component: () => import("../views/user/profile.vue"), name: "user.profile" },
        { path: "address", component: () => import("../views/user/address.vue"), name: "user.address" },
        { path: "order", component: () => import("../views/user/order.vue"), name: "user.order" },
        { path: "order/detail/:id", component: () => import("../views/user/orderdetail.vue"), name: "user.order.detail", props: route => ({id: Number(route.params.id)})},
      ]
    },
    { path: "/cart",component: () => import("../views/user/cart.vue"), meta: { requiresAuth: true, requiresRole: 'User' }, name: "cart" },
    { path: "/checkout",component: () => import("../views/user/checkout.vue"), meta: { requiresAuth: true, requiresRole: 'User' }, name: "checkout" },
    {
      path: "/sale",
      component: LayoutDashboard,
      meta: { requiresAuth: true, requiresRole: 'Admin' },
      children: [
        { path: "", component: () => import("../views/sales/index.vue"), name: "sale.index" },
        { path: "detail/:id", component: () => import("../views/sales/detail.vue"), name: "sale.detail", props: route => ({id: Number(route.params.id)})},
      ]
    },
    {
      path: "/order",
      component: LayoutDashboard,
      meta: { requiresAuth: true, requiresRole: 'Admin' },
      children: [
        { path: "", component: () => import("../views/orders/order.vue"), name: "order" },
      ]
    },
  ],
});

router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem('loggedIn') === 'true';
  const role = localStorage.getItem('role');

  if (to.meta.requiresAuth && !loggedIn) {
    next('/login');
  } else if (to.meta.requiresRole && role !== to.meta.requiresRole) {
    next('/');
  } else {
    next();
  }
});
export default router;
