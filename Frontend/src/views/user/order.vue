<template>
  <el-container >
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/home' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Đơn hàng</el-breadcrumb-item>
        </el-breadcrumb>
      </el-header>
  <el-main>
  <div class="orders">
    <div class="orders-header">
      <el-select v-model="status" placeholder="Tình trạng">
        <el-option label="Tất cả" value="Tất cả"></el-option>
        <el-option label="Chờ xác nhận" value="Chờ xác nhận"></el-option>
        <el-option label="Chờ giao hàng" value="Chờ giao hàng"></el-option>
        <el-option label="Đã giao" value="Đã giao"></el-option>
        <el-option label="Đã hủy" value="Đã hủy"></el-option>
      </el-select>
    </div>
    <el-divider></el-divider>
    <el-table :data="filteredOrders" style="width: 100%" height="650">
      <el-table-column fixed prop="image_url" label="Hình ảnh" width="170">
        <template #default="scope"> 
          <img v-if="scope.row.orderDetails && scope.row.orderDetails.length > 0" :src="scope.row.orderDetails[0].image_url" alt="product image" class="product-image">
        </template>
      </el-table-column>
      <el-table-column prop="product_name" label="Tên sản phẩm" width="180">
        <template #default="scope"> 
          <div v-if="scope.row.orderDetails && scope.row.orderDetails.length > 0">{{scope.row.orderDetails[0].product_name}}</div>
        </template>
      </el-table-column>
      <el-table-column prop="order_date" label="Ngày đặt" width="180">
        <template #default="scope"> 
          <div>{{ formatDateTime(scope.row.order_date)}}</div>
        </template>
      </el-table-column>
      <el-table-column prop="order_status" label="Tình trạng" width="180"></el-table-column>
      <el-table-column prop="order_total_price" label="Tổng tiền" width="180"></el-table-column>
      <el-table-column label="Chi tiết" width="120">
        <template #default="scope">
          <router-link :to="{ name: 'user.order.detail', params: { id: scope.row.orders_id} }" >
            Xem chi tiết
          </router-link>
        </template>
      </el-table-column>
    </el-table>
  </div>
</el-main>
  </el-container>
</template>

<script>
export default {
  data() {
    return {
      status: 'Tất cả',
    }
  },
  computed: {
    orders() {
      return this.$store.getters.orders || [];
    },
    filteredOrders() {
      if (this.status === 'Tất cả') {
        return this.orders;
      }
      return this.orders.filter(order => order.order_status === this.status);
    },
  },
  methods:{
    formatDateTime(dateTimeStr) {
            const dateTime = new Date(dateTimeStr);
            return `${dateTime.toLocaleTimeString()} ngày ${dateTime.toLocaleDateString()}`;
        },
  },
  mounted() {
    this.$store.dispatch('getOrdersByUser');
  }
};
</script>

<style scoped>
.header-breadcrumb{
    margin-top: 50px;
    height: 30px;
  }
.orders {
  width: 80%;
  background-color: white;
  padding: 20px;
}
.orders-header {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 20px;
}
.product-image {
  width: 150px;
  height: 150px;
  object-fit: fill;
}
</style>
