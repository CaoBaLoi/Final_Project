<template>
  <el-container class="layout-container-demo" style="height: 500px">
    <el-aside width="200px">
      <el-scrollbar>
        <el-menu >
          <router-link to="/dashboard" style="text-decoration: none;">
            <el-menu-item index="1">
              <span>Bảng tổng quan</span>
            </el-menu-item>
          </router-link>
          <router-link to="/category" style="text-decoration: none;">
            <el-menu-item index="2">
              <span>Quản lý danh mục</span>
            </el-menu-item>
          </router-link>
          <router-link to="/product" style="text-decoration: none;">
            <el-menu-item index="3">
              <span>Quản lý sản phẩm</span>
            </el-menu-item>
          </router-link>
        </el-menu>
      </el-scrollbar>
    </el-aside>

      <el-container>
      <el-header style="text-align: right; font-size: 20px">
        <div class="toolbar">
          <div v-if="!loggedIn">
            <el-button style="height: 85%; font-size: medium;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
          </div>
          <div v-else>
            <span style="font-size: large; margin-right: 30px;">Xin chào {{ username }}</span>
            <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;" @click="Logout">Đăng xuất</el-button>
          </div>
        </div>
      </el-header>
      <el-main>
        <el-scrollbar>
          <router-view/>
        </el-scrollbar>
      </el-main>
    </el-container>
  </el-container>
</template>
<script>
import { mapState } from 'vuex';

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
  }
};
</script>

<style scoped>
.layout-container-demo .el-header {
  position: relative;
  background-color: var(--el-color-primary-light-7);
  color: var(--el-text-color-primary);
}
.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background: var(--el-color-primary-light-8);
}
.layout-container-demo .el-menu {
  border-right: none;
}
.layout-container-demo .el-main {
  padding: 0;
}
.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}
</style>
