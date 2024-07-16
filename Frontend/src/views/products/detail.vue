<template>
  <div v-if="product" class="product-container">
    <el-card class="product-card">
      <div class="product-content">

        <div class="product-image-container">
          <el-image
            class="product-image"
            :src="product.image_url"
            fit="fill"
            alt="Product Image"
          ></el-image>
        </div>

        <div class="product-info">
          <h1>{{ product.product_name }}</h1>
          <div v-if="isOnSale(product.product_id)" class="product-info-sales">
            <div>
              <div class="sales-info">
                <span class="sales-discount">-{{ getSalePercent(product.product_id) }}%</span>
                <div class="time-countdown">
                  <div>Còn</div>
                  <div><el-countdown :value="time" :value-style="{ fontSize: '1.5rem', color: 'white', fontWeight: 'bold' }"/></div>
                </div>
              </div>
              
              <div class="sales-body">
                <div class="sales-body-price">
                  <strong>Giá bán: </strong>
                  <div class="price-sales-body">
                    <span class="products-price">
                      {{ formatPrice(product.product_price) }}
                    </span>
                    <span class="sales-price">{{ formatPrice(getSalePrice(product.product_id)) }}</span>
                  </div>
                </div>
                <p><strong>Mô tả:</strong> {{ product.product_description }}</p>
              </div>
            </div>
          </div>
          <div v-else>
            <div class="product-info-no-sales">
              <p><strong>Giá bán:</strong> {{ formatPrice(product.product_price) }}</p>
              <p><strong>Mô tả:</strong> {{ product.product_description }}</p>
            </div>
          </div>

          <div class="quantity-selector">
            <el-input-number v-model="quantity" :min="1" :max="product.product_quantity"></el-input-number>
            <span class="quantity-label">Sản phẩm còn lại {{ product.product_quantity }}</span>
          </div>
          
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
   <el-card class="feedback-card">
      <div class="feedback-content">
        <h2>Đánh giá của sản phẩm</h2>
        <template v-if="filteredFeedback.length > 0">
          <div v-for="feedback in filteredFeedback" :key="feedback.id" class="single-feedback">
            <el-rate
              v-model="feedback.feedback_point"
              disabled
              show-score
              text-color="#ff9900"
              score-template="{value}"
            />
            <div class="feedback-details" style="display: flex;">
              <div>
                <el-avatar :size="50" :src="circleUrl" />
              <div style="margin-left: 8px;">{{ feedback.username }}</div>
              </div>
              <div style="display: flex; align-items: center;">
                <p style="margin-left: 150px; font-size: 18px;">{{ feedback.feedback }}</p>
                <p style="text-align: end; margin-left: 300px;"> {{ formatDateTime( feedback.feedback_date )}}</p>
              </div>
            </div>
          </div>
        </template>
        <template v-else>
          <p>Chưa có đánh giá nào cho sản phẩm này.</p>
        </template>
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
import { ElMessage } from 'element-plus';
export default {
  data() {
    return {
      quantity: 1, // Default quantity
      time: 0,
      circleUrl:
        'https://cube.elemecdn.com/3/7c/3ea6beec64369c2642b92c6726f1epng.png',
    };
  },
  props: {
    id: {
      type: Number,
      required: true
    }
  },
  computed: {
    ...mapState(['product']),
    ...mapState(['feedbacks']),
    sales() {
      return this.$store.getters.sales;
    },
    filteredFeedback() {
      return this.feedbacks.filter(feedback => feedback.product_id === this.product.product_id);
    }
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
    ...mapActions(['getAllFeedBack']),
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    formatDateTime(dateTimeStr) {
            const dateTime = new Date(dateTimeStr);
            return `${dateTime.toLocaleTimeString()} ngày ${dateTime.toLocaleDateString()}`;
        },
    async buyProduct() {
      try {
        await this.addToCart();
        this.$router.push({ path: '/cart', query: { productId: this.product.product_id } });
      } catch (error) {
        console.error('Error buying product:', error);
        ElMessage.error('Mua sản phẩm không thành công');
      }
    },
    async addToCart() {
      try {
        await this.$store.dispatch('addToCart', { 
            product_id : this.product.product_id,
            quantity : this.quantity
          })
          ElMessage.success('Đã thêm sản phẩm vào giỏ hàng');
      } catch (error) {
        console.error('Error adding to cart:', error);
        ElMessage.error('Thêm vào giỏ hàng không thành công')
      }
    },
    updateTime() {
      if (this.sales.length > 0 && this.sales[0].sale_end_time) {
        this.sale_end_time = this.sales[0].sale_end_time;
        const saleEndTime = new Date(this.sale_end_time);
        const now = Date.now();
        const timeRemaining = saleEndTime.getTime() - now;
        this.time = Date.now() + timeRemaining;
      }
    },
    isOnSale(productId) {
      return this.sales.some(sale => sale.saleItems.some(item => item.product_id === productId));
    },
    getSalePrice(productId) {
      for (const sale of this.sales) {
        const item = sale.saleItems.find(item => item.product_id === productId);
        if (item) {
          return item.product_price - (item.product_price * item.sale_percent / 100);
        }
      }
      return null;
    },
    getSalePercent(productId) {
      for (const sale of this.sales) {
        const item = sale.saleItems.find(item => item.product_id === productId);
        if (item) {
          return item.sale_percent;
        }
      }
      return null;
    }
  },
  created() {
    this.$store.dispatch('getSales');
    this.$store.dispatch('getAllFeedBack')
  },
  mounted() {
    this.$watch(
      () => this.sales,
      (newSales) => {
        if (newSales.length > 0) {
          this.updateTime();
        }
      }
    );
  },
};
</script>
  
<style scoped>
.product-container {
  justify-content: center;
  margin-top: 80px;
}

.product-card {
  width: 80%;
  max-width: 900px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  margin-left: auto;
  margin-right: auto;
}
.feedback-card{
  width: 80%;
  max-width: 900px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  margin-left: auto;
  margin-right: auto;
  margin-top: 30px;
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
  max-width: 300px;
  height: 400px;
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.product-info {
  flex: 1;
  padding: 20px;
  box-sizing: border-box;
  font-size: 1.2em;
}

.product-info h1 {
  margin-bottom: 10px;
  font-size: 1.5em;
}

.product-info p {
  margin-bottom: 10px;
  line-height: 1.6;
}

.product-info-sales,
.product-info-no-sales {
  margin-top: 20px;
  border-top: 1px solid #ccc;
}

.quantity-selector {
  display: flex;
  align-items: center;
  margin-top: 20px;
  margin-bottom: 20px;
}

.quantity-label {
  margin-left: 10px;
}

.button-group {
  display: flex;
  gap: 10px;
  margin-top: 20px;
}

.el-button {
  font-size: 1em;
  padding: 10px 20px;
  border-radius: 4px;
}

.el-button .el-icon {
  margin-right: 10px;
}

.el-button[type="success"] {
  background-color: #67c23a;
  color: white;
  border: none;
}

.el-button[type="primary"] {
  background-color: #409eff;
  color: white;
  border: none;
}

.sales-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 20px;
  background-color: rgb(255, 102, 0);
  padding: 10px;
  border-radius: 4px;
}

.time-countdown {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 1.2em;
  color: white;
}

.sales-discount {
  color: white;
  padding: 4px 8px;
  font-weight: bold;
  border-radius: 4px;
  margin-right: 10px;
}

.sales-body-price {
  display: flex;
  align-items: center;
  margin-top: 10px;
}

.price-sales-body {
  display: flex;
  align-items: center;
}

.price-sales-body .products-price {
  text-decoration: line-through;
  margin-left: 10px;
  color: #999;
}

.price-sales-body .sales-price {
  margin-left: 20px;
  font-size: 1.3em;
  font-weight: bold;
  color: red;
}
</style>

  