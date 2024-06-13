<template>
    <el-form :model="form" label-width="auto" style="max-width: 600px; margin-top: 50px; margin-left: 10%;">
      <el-form-item label="Tên danh mục">
        <el-input v-model="form.category_name" />
      </el-form-item>
      <el-form-item >
        <el-input v-model="form.category_id " type="hidden" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit" style="margin-left: auto; margin-right: auto;">Create</el-button>
      </el-form-item>
    </el-form>
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
  