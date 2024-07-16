<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item :to="{ path: '/product' }">Sản phẩm</el-breadcrumb-item>
          <el-breadcrumb-item>Thêm sản phẩm </el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
  <el-card style="max-width: 40%; margin-left: 20%; margin-top: 50px;">
    <div class="card-header">
      <div class="content">
        <span>Thêm sản phẩm</span>
      </div>
      <el-divider></el-divider>
    </div>
    <div class="card-content">
      <el-form :model="form" :rules="rules" ref="productForm" label-width="120px">
      <el-form-item label="Tên sản phẩm" prop="product_name">
        <el-input v-model="form.product_name"></el-input>
      </el-form-item>
      <el-form-item label="Tên danh mục" prop="category_name">
        <el-select v-model="form.category_name" placeholder="Chọn danh mục" filterable allow-create>
          <el-option v-for="category in categories" :key="category.category_id" :label="category.category_name" :value="category.category_name"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="Tên nhãn" prop="tag_name">
        <el-select v-model="form.tag_name" placeholder="Chọn nhãn" multiple filterable allow-create>
          <el-option v-for="tag in tags" :key="tag.tag_id" :label="tag.tag_name" :value="tag.tag_name"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="Giá" prop="product_price">
        <el-input type="number" v-model="form.product_price"></el-input>
      </el-form-item>
      <el-form-item label="Số lượng" prop="product_quantity">
        <el-input type="number" v-model="form.product_quantity"></el-input>
      </el-form-item>
      <el-form-item label="Mô tả" prop="product_description">
        <el-input type="textarea" v-model="form.product_description"></el-input>
      </el-form-item>
      <el-form-item label="Hình ảnh" prop="image_file">
        <input ref="filesInput" type="file" multiple @change="handleFileChange" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="addProduct" style="margin-left: auto; margin-right: auto;">Thêm mới</el-button>
      </el-form-item>
    </el-form>
    </div>
  </el-card>
</template>

<script>
import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      form: {
        product_name: '',
        category_name: '',
        tag_name: [],
        product_price: '',
        product_quantity: '',
        product_description: '',
        image_file: null,
      },
      rules: {
        product_name: [{ required: true, message: 'Vui lòng nhập tên sản phẩm', trigger: 'blur' }],
        category_name: [{ required: true, message: 'Vui lòng nhập tên danh mục', trigger: 'blur' }],
        tag_name: [{ required: true, message: 'Vui lòng nhập tên nhãn', trigger: 'blur' }],
        product_price: [{ required: true, message: 'Vui lòng nhập giá bán', trigger: 'blur' }],
        product_quantity: [{ required: true, message: 'Vui lòng nhập số lượng', trigger: 'blur' }],
        product_description: [{ required: true, message: 'Vui lòng nhập mô tả', trigger: 'blur' }],
      }
    };
  },
  computed: {
    categories() {
      return this.$store.getters.categories;
    },
    tags() {
      return this.$store.getters.tags;
    }
  },
  methods: {
    async addProduct() {
      try {
        const formData = new FormData();
        formData.append('product_name', this.form.product_name);
        formData.append('category_name', this.form.category_name);
        formData.append('tag_name', this.form.tag_name);
        formData.append('product_price', this.form.product_price);
        formData.append('product_quantity', this.form.product_quantity);
        formData.append('product_description', this.form.product_description);

        if (this.form.image_file) {
          for (let i = 0; i < this.form.image_file.length; i++) {
            formData.append('image_file', this.form.image_file[i]);
          }
        }

        await this.$store.dispatch('createProduct', formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        });
        ElMessage.success('Thêm sản phẩm thành công')
      } catch (error) {
        console.error('Đã xảy ra lỗi khi thêm sản phẩm:', error);
        ElMessage.error('Thêm sản phẩm không thành công')
      }
    },
    handleFileChange(event) {
      const files = event.target.files;
      if (files && files.length > 0) {
        this.form.image_file = Array.from(files);
      } else {
        this.form.image_file = null;
      }
    },
  },
  mounted() {
    this.$store.dispatch('fetchCategories');
    this.$store.dispatch('getTagname');
  },
};
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