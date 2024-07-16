<template>
  <div style="background-color: #fff">
    <div style="width: 60%; margin: 30px auto;">
      <el-carousel trigger="click" height="450px">
        <el-carousel-item>
          <img src="https://gcs.tripi.vn/public-tripi/tripi-feed/img/474095eZH/anh-bia-do-gia-dung-cho-facebook_075556533.jpg" alt="Image 1" style="width: 100%; height: 450px; object-fit: fill;">
        </el-carousel-item>
        <el-carousel-item>
          <img src="https://gcs.tripi.vn/public-tripi/tripi-feed/img/474095Zzw/anh-bia-do-gia-dung-cho-zalo_075556894.jpg" alt="Image 2" style="width: 100%; height: 450px; object-fit: fill;">
        </el-carousel-item>
        <el-carousel-item>
          <img src="https://gcs.tripi.vn/public-tripi/tripi-feed/img/474095XTv/anh-bia-do-gia-dung-nghe-thuat_075558170.jpg" alt="Image 3" style="width: 100%; height: 450px; object-fit: fill;">
        </el-carousel-item>
        <el-carousel-item>
          <img src="https://gcs.tripi.vn/public-tripi/tripi-feed/img/474095Xnz/anh-bia-do-gia-dung-trong-bep_075559608.jpg" alt="Image 4" style="width: 100%; height: 450px; object-fit: fill;">
        </el-carousel-item>
      </el-carousel>
    </div>
  </div>
  <div class="sale">
    <div class="sale-header">
      <div class="sale-header-title"><h3>Giảm giá thần tốc</h3></div>
      <div class="countdown">
        <div>Còn</div>
        <div><el-countdown :value="time" :value-style="{ fontSize: '1.5rem', color: 'red', fontWeight: 'bold' }"/></div>
      </div>
    </div>
    <div class="sale-items">
      <div v-for="sale in sales" :key="sale.sale_id" class="sale-item">
        <div v-for="(item, index) in sale.saleItems" :key="index" class="sale-item-body">
          <router-link :to="{ name: 'product.detail', params: { id: item.product_id } }" style="text-decoration: none;">
            <img :src="item.image_url" alt="Product Image" class="sale-item-image">
            <div class="sale-item-details">
              <div class="sale-item-name">{{ item.product_name }}</div>
              <div class="sale-item-discount">-{{ item.sale_percent }}%</div>
              <div class="sale-item-price">{{ formatPrice(item.product_price - (item.product_price * item.sale_percent / 100)) }}</div>
            </div>
          </router-link>
        </div>
      </div>
    </div>
  </div>
  <div class="product-container">
    <el-input v-model="search" placeholder="Nhập từ khóa" style="width: 300px; margin-left: 70%;"/>
    <el-row class="">
      <el-col v-for="(product, index) in products" :key="index" :span="4" style="padding-left: 5px; padding-right: 5px;">
        <router-link :to="{ name: 'product.detail', params: { id: product.product_id } }" style="text-decoration: none;">
          <div class="product-item">
            <img :src="product.image_url" alt="Product Image" class="product-image"/>
            <div class="product-info">
              <div class="product-name">{{ product.product_description }}</div>
              <div v-if="isOnSale(product.product_id)" class="product-price">
                <span class="sale-price">{{ formatPrice(getSalePrice(product.product_id)) }}</span>
                <span class="sale-discount">-{{ getSalePercent(product.product_id) }}%</span>
              </div>
              <div v-else class="product-price">{{ formatPrice(product.product_price) }}</div>
            </div>
          </div>
        </router-link>
      </el-col>
    </el-row>
  </div>
</template>


<script>
export default {
  data() {
    return {
      search: '',
      time: 0, 
    };
  },
  computed: {
    products() {
      return this.$store.getters.products.filter(
        (data) =>
          !this.search ||
          data.product_name.toLowerCase().includes(this.search.toLowerCase())
      );
    },
    sales() {
      return this.$store.getters.sales;
    },
  },
  methods: {
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
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
  },
  mounted() {
    this.$store.dispatch('fetchProducts');
    // Sử dụng watch để theo dõi sự thay đổi của sales
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
<style>
body {
  background-color: #f5f5f5; 
}
</style>
<style scoped>
.sale {
  padding: 0px 20px 10px 20px;
  margin-bottom: 20px;
  background-color: white;
  width: 60%;
  margin-left: auto;
  margin-right: auto;
  margin-top: 30px;
  box-sizing: border-box;
  border-radius: 8px; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
}

.sale-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 2px solid #ff9800; 
  padding-bottom: 10px;
}

.sale-header-title h3 {
  font-size: 1.5rem;
  color: #ff9800; 
  font-weight: bold; 
}

.countdown {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
  font-size: x-large;
  color: #ff9800; 
}

.sale-items {
  display: flex;
  justify-content: flex-start;
  overflow-x: auto;
  padding-top: 10px; 
}

.sale-item {
  display: flex;
  margin-right: 10px; 
}

.sale-item-body {
  padding: 10px;
  background-color: #fff;
  width: 190px;
  text-align: center;
  position: relative;
  border: 1px solid #eee; 
  border-radius: 8px; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.sale-item-image {
  width: 100%;
  height: 170px;
  object-fit: cover;
  margin-bottom: 10px;
  border-radius: 8px;
}

.sale-item-details {
  display: flex;
  flex-direction: column;
}

.sale-item-name {
  font-size: 1.1rem;
  margin-bottom: 5px;
}

.sale-item-price {
  font-size: 1.25rem;
  color: #d32f2f;
  font-weight: bold; 
  
}

.sale-item-discount {
  position: absolute;
  top: 10px;
  right: 10px;
  background-color: #ff9800; 
  color: white; 
  padding: 4px 8px;
  font-size: 14px;
  font-weight: bold;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}


.product-container {
  width: 60%;
  margin-left: auto;
  margin-right: auto;
  align-items: center;
  justify-content: center;
  margin-top: 30px;
}

.product-item {
  margin-top: 10px;
  height: 300px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  background-color: white;
  position: relative; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  border-radius: 8px; 
  overflow: hidden; 
  transition: transform 0.3s ease, box-shadow 0.3s ease; 
}

.product-image {
  width: 100%;
  height: 220px;
  object-fit: fill; 
  transition: transform 0.3s ease;
}

.product-item:hover {
  transform: translateY(-5px); 
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2); 
}

.product-info {
  background-color: white;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 80px;
  padding: 10px; 
  text-align: center; 
}

.product-name {
  height: 40px;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  line-height: 20px;
  font-weight: bold; 
  color: #555; 
}

.product-price {
  font-size: 18px;
  color: #e53935; 
  font-weight: bold; 
  margin-top: 5px;
}

.sale-discount {
  position: absolute;
  top: 5px;
  right: 5px;
  background-color: #ff9800; 
  color: white; 
  padding: 4px 8px; 
  font-size: 14px;
  font-weight: bold;
  border-radius: 4px; 
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); 
}

</style>
