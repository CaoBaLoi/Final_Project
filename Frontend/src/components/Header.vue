<!-- <template>
<el-header style="text-align: right; height: 50px;">
    <div class="flex items-center" style="height: 100%;">
        <el-button style="height: 85%; font-size: large;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
        <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;"><router-link to="/register" style="text-decoration: none;">Đăng ký</router-link></el-button>
      </div>
</el-header>
</template> -->
<template>
  <el-header style="text-align: right; height: 50px; background-color: antiquewhite; display: flex; align-items: center; justify-content: flex-end;">
    <div>
      <div v-if="!loggedIn" style="display: flex; align-items: center;">
        <el-button style="height: 85%; font-size: large;">
          <router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link>
        </el-button>
        <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;">
          <router-link to="/register" style="text-decoration: none;">Đăng ký</router-link>
        </el-button>
      </div>
      <div v-else style="display: flex; align-items: center;">
        <el-dropdown>
        <span style="font-size: x-large; color: black;">Xin chào {{ username }}</span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item>
              <router-link to="/user" class="dropdown-item" style="text-decoration: none; color: inherit;">Tài khoản của tôi</router-link>
            </el-dropdown-item>
            <el-dropdown-item @click="Logout" class="dropdown-item">
              Đăng xuất
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
      </div>
    </div>
  </el-header>
</template>

<style>
.ml-2 {
  margin-left: 10px;
}
.dropdown-item {
  font-size: large;
  /* padding: 10px 20px; */
  display: block;
  /* width: 100%; */
}
</style>


<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState(['loggedIn', 'username']), // Sử dụng mapState để lấy loggedIn và username từ Vuex
  },
  methods: {
    async Logout() {
      // Gọi action logout từ Vuex khi người dùng nhấn vào nút Đăng xuất
      await this.$store.dispatch('logout');
    }
  }
};
</script>
