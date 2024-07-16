<template>
  <el-container style="height: 100vh;">
    <el-header><navbar/></el-header>
    <el-container>
      <el-aside width="200px" style="margin-left: 15%; margin-top: 70px">
        <el-menu
          class="el-menu-vertical-demo"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
          :default-active="defaultActive"
          @select="handleMenuSelect">
          <router-link to="/user/profile" style="text-decoration: none;">
            <el-menu-item index="profile" class="el-menu">
              <span>Thông tin cá nhân</span>
            </el-menu-item>
          </router-link>
          <router-link to="/user/address" style="text-decoration: none;">
            <el-menu-item index="address" class="el-menu">
              <span>Địa chỉ</span>
            </el-menu-item>
          </router-link>
          <router-link to="/user/order" style="text-decoration: none;">
            <el-menu-item index="order" class="el-menu">
              <span>Đơn hàng</span>
            </el-menu-item>
          </router-link>
        </el-menu>
      </el-aside>

      <el-main>
        <el-scrollbar>
          <router-view/>
        </el-scrollbar>
      </el-main>
    </el-container>
  </el-container>
  <navfooter />
</template>

<script setup>
import navbar from "../components/Header.vue";
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import navfooter from "../components/Footer.vue";
const router = useRouter();
const defaultActive = ref('profile');
onMounted(() => {
  const storedActive = localStorage.getItem('defaultActive');
  if (storedActive) {
    defaultActive.value = storedActive;
  }
});

const handleMenuSelect = (index) => {
  defaultActive.value = index;
  localStorage.setItem('defaultActive', index);
};
</script>

<script>
import { mapState } from 'vuex';

export default {
  setup() {
    return {
      defaultActive,
      router,
    };
  },
  methods: {
    async Logout() {
    
      await this.$store.dispatch('logout');
    }
  },
};
</script>

<style>
</style>
