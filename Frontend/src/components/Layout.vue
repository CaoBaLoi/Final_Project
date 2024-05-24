<script setup>
import navbar from "../components/Header.vue";
import navfooter from "../components/Footer.vue";
</script>
<template>
  <navbar/>
  <router-view />
  <navfooter />
</template>