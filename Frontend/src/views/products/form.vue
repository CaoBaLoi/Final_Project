<template>
    <div style="width: 70%; margin-left: 10%;">
      <h2>Add Product</h2>
      <el-form :model="form" :rules="rules" ref="productForm" label-width="120px">
        <el-form-item label="Product Name" prop="product_name">
          <el-input v-model="form.product_name"></el-input>
        </el-form-item>
        <el-form-item label="Category Name" prop="category_name">
          <el-select v-model="form.category_name" placeholder="Select category" filterable>
            <el-option v-for="category in categories" :key="category.category_id" :label="category.category_name" :value="category.category_name"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="Tag Name" prop="tag_name">
          <el-select v-model="form.tag_name" placeholder="Select tags" multiple filterable>
            <el-option v-for="tag in tags" :key="tag.tag_id" :label="tag.tag_name" :value="tag.tag_name"></el-option>
          </el-select>
        </el-form-item>
        <!-- Remaining form items -->
        <el-form-item label="Product Price" prop="product_price">
          <el-input type="number" v-model="form.product_price"></el-input>
        </el-form-item>
        <el-form-item label="Product Quantity" prop="product_quantity">
          <el-input type="number" v-model="form.product_quantity"></el-input>
        </el-form-item>
        <el-form-item label="Product Description" prop="product_description">
          <el-input type="textarea" v-model="form.product_description"></el-input>
        </el-form-item>
        <el-form-item label="Image File" prop="image_files">
          <el-upload
            class="upload-demo"
            action="/upload"
            :on-change="handleFileUpload"
            multiple
            :file-list="fileList">
            <el-button size="small" type="primary">Click to Upload</el-button>
          </el-upload>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="addProduct">Add Product</el-button>
        </el-form-item>
      </el-form>
    </div>
  </template>
  
  <script>
export default {
  data() {
    return {
      form: {
        product_name: '',
        category_name: '',
        tag_name: '',
        product_price: '',
        product_quantity: '',
        product_description: '',
      },
      rules: {
        product_name: [
          { required: true, message: 'Vui lòng nhập tên sản phẩm', trigger: 'blur' }
        ],
        category_name: [
          { required: true, message: 'Vui lòng nhập tên danh mục', trigger: 'blur' }
        ],
        tag_name: [
          { required: true, message: 'Vui lòng nhập tên nhãn', trigger: 'blur' }
        ],
        product_price: [
          { required: true, message: 'Vui lòng nhập giá bán', trigger: 'blur' }
        ],
        product_quantity: [
          { required: true, message: 'Vui lòng nhập số lượng', trigger: 'blur' }
        ],
        product_description: [
          { required: true, message: 'Vui lòng nhập mô tả', trigger: 'blur' }
        ]
      }
    }
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
            
        } catch (error) {
            console.error('Đã xảy ra lỗi khi thêm danh mục:', error);
            alert('Đã xảy ra lỗi khi thêm danh mục. Vui lòng thử lại sau.');
        }
        }
  },
  mounted() {
    this.$store.dispatch('fetchCategories');
    this.$store.dispatch('getTagname');
  },
}
  </script>
  