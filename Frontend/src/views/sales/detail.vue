<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item :to="{ path: '/sale' }">Giảm giá</el-breadcrumb-item>
          <el-breadcrumb-item>Chi tiết giảm giá</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
    <div class="sale-detail">
      <el-card v-if="sale ">
        <div slot="header" class="card-header">
          <h3>Chi tiết Khuyến mãi</h3>
        </div>
        <el-divider></el-divider>
        <div class="detail-content">
          <div class="detail-item">
            <span class="detail-label">ID:</span>
            <span>{{ sale.sale_id }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Ngày bắt đầu:</span>
            <span>{{ formatDateTime(sale.sale_begin_time) }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Ngày kết thúc:</span>
            <span>{{ formatDateTime(sale.sale_end_time) }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Các sản phẩm trong khuyến mãi:</span>
            <el-table :data="sale.saleItems" border style="width: 100%; margin-top: 20px;">
              <el-table-column prop="product_id" label="ID" width="80"></el-table-column>
              <el-table-column label="Hình ảnh" width="125">
                    <template #default="{row}">
                    <img :src="row.image_url" alt="" style="max-width: 100px; max-height: 100px;">
                    </template>
                </el-table-column>
              <el-table-column prop="product_name" label="Tên sản phẩm"></el-table-column>
              <el-table-column prop="product_price" label="Giá" width="300">
                <template #default="{ row }">{{ formatPrice(row.product_price) }}</template>
              </el-table-column>
              <el-table-column prop="sale_percent" label="Phần trăm giảm" width="250">
                <template #default="{ row }">{{ row.sale_percent }}%</template>
              </el-table-column>
            </el-table>
          </div>
        </div>
      </el-card>
      <el-card v-else>
        <div slot="header" class="card-header">
          <h2>Loading...</h2>
        </div>
        <el-divider></el-divider>
        <div class="detail-content">
          <p>Loading sale details...</p>
        </div>
      </el-card>
    </div>
  </template>
  
  <script>
  import { mapState, mapActions } from 'vuex';
  
  export default {
    props: {
      id: {
        type: Number,
        required: true
      }
    },
    watch: {
      id: {
        immediate: true,
        handler(newId) {
          console.log('ID prop changed to:', newId);
          this.getSale(newId);
        }
      }
    },
    computed: {
      ...mapState(['sale']),
    },
    methods: {
      ...mapActions(['getSale']),
      formatDateTime(dateTimeStr) {
        const dateTime = new Date(dateTimeStr);
        return `${dateTime.toLocaleTimeString()} ngày ${dateTime.toLocaleDateString()}`;
      },
      formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    }
  };
  </script>
  
  <style scoped>
  .sale-detail{
    margin: 0px 20px 20px 20px;
  }
  .detail-content {
    padding: 0px 20px 20px 20px;
  }
  .header-breadcrumb{
    height: 30px;
  }
  .card-header h3 {
  margin: 0px 0px 10px 0px;
}
  .detail-item {
    margin-bottom: 10px;
  }
  
  .detail-label {
    font-weight: bold;
    margin-right: 10px;
  }
  </style>
  