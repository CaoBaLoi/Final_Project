<template>
  <el-container>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/home' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Thông tin cá nhân</el-breadcrumb-item>
        </el-breadcrumb>
      </el-header>
      <el-main>
  <el-card style="max-width: 600px; " v-if="profile">
    <div class="card-header">
      <span>Thông tin cá nhân</span>
      <el-divider></el-divider>
    </div>
    <div class="card-content">
      <el-form ref="form" :model="form" :rules="rules" label-width="120px">
        <el-form-item label="Họ và tên" prop="fullname">
          <el-input v-model="form.fullname"></el-input>
        </el-form-item>
        <el-form-item label="Số điện thoại" prop="phone">
          <el-input v-model="form.phone"></el-input>
        </el-form-item>
        <el-form-item label="Địa chỉ" prop="address">
          <el-input v-model="form.address"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="saveProfile">Lưu</el-button>
        </el-form-item>
      </el-form>
    </div>
  </el-card>
</el-main>
  </el-container>
</template>

<script>
import { ElMessage } from 'element-plus';


export default {
  data(){
    return{
      form:{
        fullname: '',
        phone: '',
        address: '',
      },
      rules: {
        fullname: [
          { required: true, message: 'Vui lòng nhập họ tên', trigger: 'blur' }
        ],
        phone: [
          { required: true, message: 'Vui lòng nhập số điện thoại', trigger: 'blur' },
          { pattern: /^[0-9]{10}$/, message: 'Số điện thoại không hợp lệ', trigger: 'blur' }
        ],
        address: [
          {required: true, message: 'Vui lòng nhập địa chỉ', trigger: 'blur'}
        ]
      },
    }
  },
  computed: {
    profile() {
      return this.$store.getters.profile;
    }
  },
  watch: {
    profile: {
      immediate: true,
      handler(newProfile) {
        this.form = { ...newProfile };
      }
    }
  },
  methods: {
    async saveProfile() {
      try {
          await this.$store.dispatch('addProfile', { 
              fullname : this.form.fullname,
              phone : this.form.phone,
              address : this.form.address
            });
            ElMessage.success('Cập nhật thông tin thành công')
      } catch (error) {
        console.error('Error adding to profile:', error);
        ElMessage.error('Cập nhật thông tin thất bại')
      }
    },
  },
  mounted() {
    this.$store.dispatch('getProfile');
  },
};
</script>

<style scoped>
.header-breadcrumb{
    margin-top: 50px;
    height: 30px;
  }
.card-header {
  display: flex;
  flex-direction: column;
}

.card-content {
  padding-top: 20px;
}

.el-card .card-header {
  font-size: xx-large;
}
</style>
