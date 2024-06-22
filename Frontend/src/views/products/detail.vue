<template>
  <div v-if="product" class="product-container">
    <el-card class="product-card">
      <div class="product-content">
        <!-- Left half for image -->
        <div class="product-image-container">
          <el-image
            class="product-image"
            :src="product.image_url"
            fit="fill"
            alt="Product Image"
          ></el-image>
        </div>
        
        <!-- Right half for product info -->
        <div class="product-info">
          <h1>{{ product.product_name }}</h1>
          <p><strong>Giá bán:</strong> {{ formatPrice(product.product_price) }}</p>
          <p><strong>Mô tả:</strong> {{ product.product_description }}</p>

          <!-- Quantity selector -->
          <div class="quantity-selector">
            <el-input-number v-model="quantity" :min="1" :max="product.product_quantity"></el-input-number>
            <span class="quantity-label">Sản phẩm còn lại {{ product.product_quantity }}</span>
          </div>
          
          <!-- Purchase button -->
          <div class="button-group">
            <el-button type="success" @click="addToCart">
              <el-icon><shopping-cart /></el-icon>
              Thêm vào giỏ hàng
            </el-button>
            <el-button type="primary" @click="buyProduct">Mua hàng</el-button>
            
          </div>
        </div>
      </div>
    </el-card>
  </div>
  <div v-else>
    <p>Loading...</p>
  </div>
</template>
  
<script>
import { mapState, mapActions} from 'vuex';
import { ShoppingCart } from '@element-plus/icons-vue'
export default {
  data() {
    return {
      quantity: 1, // Default quantity
    };
  },
  props: {
    id: {
      type: Number,
      required: true
    }
  },
  computed: {
    ...mapState(['product', 'loggedIn', 'role']),
  },
  watch: {
    id: {
      immediate: true,
      handler(newId) {
        this.getProduct(newId);
      }
    }
  },
  methods: {
    ...mapActions(['getProduct']),
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
      buyProduct() {
        // Xử lý logic mua hàng, có thể gọi API hoặc thực hiện các thao tác khác
        alert(`Bạn đã mua sản phẩm ${this.product.product_name}`);
      },
      addToCart() {
      // Handle add to cart logic
      console.log(`Adding ${this.quantity} of ${this.product.product_name} to cart`);
    },
  }
};
</script>
  
<style scoped>
.product-container {
  display: flex;
  justify-content: center;
  margin-top: 80px; /* Adjust the margin to create space from the header */
}

.product-card {
  width: 80%; /* Adjust the width as needed */
  max-width: 900px; /* Set a maximum width if desired */
}

.product-content {
  display: flex;
}

.product-image-container {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
  max-width: 300px; /* Fixed width for the image container */
  height: 400px; /* Fixed height for the image container */
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Ensure the image covers the container while maintaining its aspect ratio */
}

.product-info {
  flex: 1;
  padding: 20px;
  box-sizing: border-box; /* Ensure padding doesn't affect width */
  font-size: 1.2em; /* Increase font size */
}

.product-info h2 {
  margin-bottom: 10px;
}

.product-info p {
  margin-bottom: 10px;
}

.quantity-selector {
  display: flex;
  align-items: center;
  margin-top: 20px;
  margin-bottom: 20px;
}

.quantity-label {
  margin-left: 10px; /* Adjust the spacing as needed */
}

.button-group {
  display: flex;
  gap: 10px; /* Adjust the spacing between buttons */
}

.el-button {
  margin-top: 10px; /* Adjust margin to ensure proper spacing */
  font-size: large;
}
.el-button .el-icon{
  margin-right: 10px;
}
</style>
  