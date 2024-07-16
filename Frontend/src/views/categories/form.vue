<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item :to="{ path: '/category' }">Danh mục</el-breadcrumb-item>
          <el-breadcrumb-item>Thêm danh mục</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
  <el-card style="max-width: 40%; margin-left: 20%; margin-top: 50px;">
    <div class="card-header">
      <div class="content">
        <span>Thêm danh mục</span>
      </div>
      <el-divider></el-divider>
    </div>
    <div class="card-content">
      <el-form :model="form" label-width="auto" style="max-width: 600px; margin-top: 50px; margin-left: 10%;">
      <el-form-item label="Tên danh mục">
        <el-input v-model="form.category_name" />
      </el-form-item>
      <el-form-item >
        <el-input v-model="form.category_id " type="hidden" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit" style="margin-left: auto; margin-right: auto;">Thêm mới</el-button>
      </el-form-item>
    </el-form>
    </div>
  </el-card>
</template>
  
<script >
export default {
    data() {
        return {
        form: {
            
            category_id: 1,
            category_name: '',
        },
        rules: {
            category_name: [
            { required: true, message: 'Vui lòng nhập tên danh mục', trigger: 'blur' }
            ]
        }
        }
    },
    methods: {
        async onSubmit() {
        try {
            await this.$store.dispatch('createCategory', { 
                category_id: 1,
                category_name: this.form.category_name
            
            });
        } catch (error) {
            console.error('Đã xảy ra lỗi khi thêm danh mục:', error);
            alert('Đã xảy ra lỗi khi thêm danh mục. Vui lòng thử lại sau.');
        }
        }
    }
}
</script>
<style>
.el-card .card-header {
  font-size: xx-large;
}
</style>
<style scoped>
.header-breadcrumb{
    height: 30px;
  }
</style>