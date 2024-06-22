<template>
  <div style="background-color: #fff">
    <div class="" m="t-4" style="width: 70%; margin-left: auto; margin-right: auto;">
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
  <div style="width:60%; margin-left: auto; margin-right: auto; align-items: center; justify-content: center;">
    <el-row class="row-bg">
    <el-col v-for="(product, index) in products" :key="index" :span="4" style="padding-left: 5px; padding-right: 5px;">
      <router-link :to="{ name: 'product.detail', params: { id: product.product_id } }" style="text-decoration: none;">
        <div class="grid-content ep-bg-purple product-item">
          <img :src="product.image_url" alt="Product Image" class="product-image"/>
          <div class="product-info">
            <div class="product-name" >{{ product.product_description }}</div>
            <div class="product-price">{{ formatPrice(product.product_price) }}</div>
          </div>
        </div>
      </router-link>
    </el-col>
  </el-row>
  </div>
</template>
<script  setup>
import { computed, ref, onMounted } from 'vue';
import { useStore } from 'vuex';

  const store = useStore();
  const search = ref('');
  const products = computed(() =>
    store.getters.products.filter(
      (data) =>
        !search.value ||
        data.product_name.toLowerCase().includes(search.value.toLowerCase())
    )
  )
  const formatPrice = (value) => {
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
  };
  onMounted(() => {
    store.dispatch('fetchProducts');
  });
</script>
<style>
body {
  background-color: #f5f5f5; /* Change the background color of the entire webpage */
}
</style>
<style scoped>
.product-item {
  cursor: pointer; /* Đổi con trỏ chuột thành dấu mũi tên khi di chuột vào sản phẩm */
  transition: transform 0.2s ease; /* Hiệu ứng chuyển động mượt mà */
  padding: 10px;
  margin-top: 10px;
  height: 300px; /* Fixed height for the product item */
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  background-color: rgba(255, 165, 0, 0.1)
}

.product-image {
  width: 100%;
  height: 220px;
  object-fit: fill;
}
.product-item:hover {
  transform: translateY(-0.5px); /* Khi di chuột vào, sản phẩm sẽ nhô lên một chút */
  border: 2px solid #ff650c;
}

.product-info {
  background-color: white;
  box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); /* Đổ bóng để tạo độ sâu */
  border-radius: 5px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 80px;
}

.product-name {
  height: 40px; /* Fixed height for two lines of text */
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2; /* Limit to two lines */
  -webkit-box-orient: vertical;
  line-height: 20px; /* Adjust line height to fit two lines within the height */
}

.product-price {
  margin-top: 10px;
  font-size: 16px;
  color: #333;
}
</style>