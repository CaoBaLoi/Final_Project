<!-- <template>
<el-header style="text-align: right; height: 50px;">
    <div class="flex items-center" style="height: 100%;">
        <el-button style="height: 85%; font-size: large;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
        <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;"><router-link to="/register" style="text-decoration: none;">Đăng ký</router-link></el-button>
      </div>
</el-header>
</template> -->
<template>
  <el-header style="text-align: right; height: 50px;">
    <div class="flex items-center" style="height: 100%;">
      <div v-if="!loggedIn">
        <el-button style="height: 85%; font-size: large;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
        <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;"><router-link to="/register" style="text-decoration: none;">Đăng ký</router-link></el-button>
      </div>
      <div v-else>
        <span style="font-size: large;">Xin chào {{ username }}</span>
        <el-button type="primary" class="ml-2" style="height: 85%; font-size: large;" @click="Logout">Đăng xuất</el-button>
      </div>
    </div>
  </el-header>
</template>

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
