<template>
  <el-container style="height: 100vh;">
    <!-- Header -->
    <el-header class="header-content" style="text-align: right; font-size: 20px">
        <div class="toolbar">
          <div v-if="!loggedIn">
            <el-button style="height: 85%; font-size: medium;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
          </div>
          <div v-else>
            <span style="font-size: large; margin-right: 30px;">Xin chào {{ username }}</span>
            <el-button type="danger" class="ml-2" style="height: 85%; font-size: large;" @click="Logout">Đăng xuất</el-button>
          </div>
        </div>
    </el-header>

    <!-- Main Container -->
    <el-container>
      <!-- Right Menu -->
      <el-aside width="200px" style="padding: 0;">
        <el-menu
          class="el-menu-vertical-demo"
          background-color="#333"
          text-color="#fff"
          active-text-color="#ffd04b"
          :default-active="activeMenuIndex">
          <router-link to="/dashboard" style="text-decoration: none;">
            <el-menu-item index="1" class="el-menu">
              <span>Bảng tổng quan</span>
            </el-menu-item>
          </router-link>
          <router-link to="/category" style="text-decoration: none;">
            <el-menu-item index="2" class="el-menu">
              <span>Quản lý danh mục</span>
            </el-menu-item>
          </router-link>
          <router-link to="/product" style="text-decoration: none;">
            <el-menu-item index="3" class="el-menu">
              <span>Quản lý sản phẩm</span>
            </el-menu-item>
          </router-link>
          <router-link to="/account" style="text-decoration: none;">
            <el-menu-item index="4" class="el-menu">
              <span>Quản lý người dùng</span>
            </el-menu-item>
          </router-link>
          <router-link to="/sale" style="text-decoration: none;">
            <el-menu-item index="5" class="el-menu">
              <span>Quản lý giảm giá</span>
            </el-menu-item>
          </router-link>
        </el-menu>
      </el-aside>

      <el-container>
      <el-main>
        <el-scrollbar>
          <router-view/>
        </el-scrollbar>
      </el-main>
    </el-container>
    </el-container>
  </el-container>
</template>

<script>
import { mapState } from 'vuex';
import { ref } from 'vue'
export default {
  computed: {
    ...mapState(['loggedIn', 'username']), // Sử dụng mapState để lấy loggedIn và username từ Vuex
  },
  methods: {
    async Logout() {
      localStorage.removeItem('loggedIn');
      localStorage.removeItem('username');
      localStorage.removeItem('token');
      // Cập nhật trạng thái đăng nhập trong Vuex
      this.$store.commit('SET_LOGIN', { loggedIn: false, username: '' });
      // Gọi action logout từ Vuex khi người dùng nhấn vào nút Đăng xuất
      await this.$store.dispatch('logout');
    }
  },
}
const activeMenuIndex = ref('1');
</script>

<style>
.header-content {
  display: flex;
  align-items: center;
  height: 70px;
  padding-left: 20px;
  color: #fff;
  background-color: #409EFF;
  justify-content: end;
}
.el-menu{
  font-size: large;
}
</style>
