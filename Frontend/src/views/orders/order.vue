<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Đơn hàng</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
    <el-card class="order-card">
      <div class="card-header">
        <h3>Quản lý đơn hàng</h3>
      </div>
      <div class="orders-header">
        <el-select v-model="order_status" placeholder="Tình trạng">
          <el-option label="Tất cả" value="Tất cả"></el-option>
          <el-option label="Chờ xác nhận" value="Chờ xác nhận"></el-option>
          <el-option label="Chờ giao hàng" value="Chờ giao hàng"></el-option>
          <el-option label="Đã giao" value="Đã giao"></el-option>
          <el-option label="Đã hủy" value="Đã hủy"></el-option>
        </el-select>
      </div>
      <el-table border :data="filteredOrders" style="width: 100%;" height="700">
        <el-table-column fixed label="Mã đơn hàng" prop="orders_id" width="100"/>
        <el-table-column label="Tổng tiền" prop="order_total_price" width="150">
          <template #default="scope">
            <div>{{ formatPrice(scope.row.order_total_price) }}</div>
          </template>
        </el-table-column>
        <el-table-column label="Địa chỉ" prop="order_address"/>
        <el-table-column label="Trạng thái" prop="order_status" width="300"/>
        <el-table-column label="Hành động" width="155">
          <template #default="scope">
          <el-button v-if="scope.row.order_status === 'Chờ xác nhận'" size="small" type="success" @click="changeStatus(scope.row.orders_id)">
            Xác nhận đơn hàng
          </el-button>
        </template>
        </el-table-column>
      </el-table>
    </el-card>
  </template>
  
<script>
import { ElMessage } from 'element-plus';
  export default {
    data() {
      return {
        order_status: 'Tất cả',
      };
    },
    computed: {
      orders() {
        return this.$store.getters.orders;
        },
      filteredOrders() {
      if (this.order_status === 'Tất cả') {
        return this.orders;
      }
      return this.orders.filter(order => order.order_status === this.order_status);
    },
    },
    methods: {
      changeStatus(orders_id) {
        const status = 'Chờ giao hàng'
        this.$store.dispatch('changeStatusOrder', {status,order_id: orders_id}).then(() => {
          this.$store.dispatch('getOrders');
          ElMessage.success('Cập nhật trạng thái đơn hàng thành công');
        }).catch(error => {
          console.error('Lỗi khi cập nhật trạng thái đơn hàng:', error);
          ElMessage.error('Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng');
        });
      },
      formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    },
    created() {
    this.$store.dispatch('getOrders');
  },
  };
</script>
  
<style scoped>
.order-card {
    margin: 0px 20px 20px 20px;
    padding: 20px;
}
.orders-header {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 20px;
}
.card-header h3 {
  margin: 0px 0px 10px 0px;
}
.header-breadcrumb{
    height: 30px;
  }
</style>
  