<template>
  <el-container style="height: 100vh;">
    <el-header class="header-content" style="text-align: right; font-size: 20px">
      <div class="toolbar">
        <div v-if="!loggedIn">
          <el-button style="height: 85%; font-size: medium;"><router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link></el-button>
        </div>
        <div v-else>
          <span style="font-size: large; margin-right: 30px;"><i class="fa-solid fa-user"></i> {{ username }}</span>
          <el-button type="danger" class="ml-2" style="height: 85%; font-size: large;" @click="Logout">Đăng xuất</el-button>
        </div>
      </div>
    </el-header>

    <el-container>
      <el-aside width="250px" style="padding: 0; height: 700px; background-color: #545c64;">
        <el-menu
          class="el-menu-vertical-demo"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
          :default-active="defaultActive"
          @select="handleMenuSelect">
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
          <router-link to="/order" style="text-decoration: none;">
            <el-menu-item index="4" class="el-menu">
              <span>Quản lý đơn hàng</span>
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

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
const router = useRouter();
const defaultActive = ref('1');

onMounted(() => {
  const storedActive = localStorage.getItem('defaultActive');
  if (storedActive !== null) {
    defaultActive.value = storedActive;
  } else {
    defaultActive.value = '1';
  }
});
watch(() => router.path, (newPath, oldPath) => {
  const storedActive = localStorage.getItem('defaultActive');
  if (storedActive !== null) {
    defaultActive.value = storedActive;
  } else {
    defaultActive.value = '1';
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
  computed: {
    ...mapState(['loggedIn', 'username']),
  },
  setup() {
    return {
      defaultActive,
      handleMenuSelect,
    };
  },
  methods: {
    async Logout() {

      await this.$store.dispatch('logout');
    }
  },
  created(){
    
  }
};
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
.el-menu {
  font-size: large;
  border-color: transparent !important;
  
}
.el-menu-item span{
  padding-left: 20px !important;
}
</style>
