<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Giảm giá</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
  <div class="sales-management">
    <el-card>
      <div slot="header" class="card-header">
        <h3>Quản lý Khuyến mãi</h3>
        <el-button type="primary" @click="dialogVisible = true">Thêm mới</el-button>
        <el-dialog title="Thêm chương trình giảm giá" v-model="dialogVisible" style="max-width: 600px;">
          <el-form :model="saleDTO">
            <el-form-item label="Thời gian bắt đầu" prop="sale_begin_time">
              <el-date-picker v-model="saleDTO.sale_begin_time" type="datetime" placeholder="Chọn ngày"></el-date-picker>
            </el-form-item>
            <el-form-item label="Thời gian kết thúc" prop="sale_end_time">
              <el-date-picker v-model="saleDTO.sale_end_time" type="datetime" placeholder="Chọn ngày"></el-date-picker>
            </el-form-item>

            <el-form-item label="Sản phẩm" prop="sale_items">
              <div v-for="(item, index) in saleDTO.sale_items" :key="index" style="margin-bottom: 10px;">
                <el-input v-model="item.product_id" placeholder="Mã sản phẩm" style="width: 40%; margin-right: 10px;"></el-input>
                <el-input v-model="item.sale_percent" placeholder="% giảm giá" style="width: 40%; margin-right: 10px;"></el-input>
                <el-button type="danger" @click="removeSaleItem(index)">Xóa</el-button>
              </div>
              <el-button type="primary" @click="addSaleItem">Thêm</el-button>
            </el-form-item>
          </el-form>

          <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">Hủy</el-button>
            <el-button type="primary" @click="handleAddSale">Lưu</el-button>
          </span>
        </el-dialog>
      </div>
      <el-divider></el-divider>
      <div class="sales-list">
        <el-table :data="sales" border style="width: 100%">
          <el-table-column prop="sale_id" label="ID" width="80"></el-table-column>
          <el-table-column prop="sale_begin_time" label="Ngày bắt đầu">
            <template #default="{ row }">
              {{ formatDateTime(row.sale_begin_time) }}
            </template>
          </el-table-column>
          <el-table-column prop="sale_end_time" label="Ngày kết thúc">
            <template #default="{ row }">
              {{ formatDateTime(row.sale_end_time) }}
            </template>
          </el-table-column>
          <el-table-column label="Hành động" width="250" class="action">
            <template #default="{ row }">
              <router-link :to="{ name: 'sale.detail', params: { id: row.sale_id } }"><el-button type="text" size="small">Xem chi tiết</el-button></router-link>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-card>
  </div>
</template>

<script>
import { ref, computed } from 'vue';
import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      dialogVisible: false,
      saleDTO: {
        sale_items: [
          {
            product_id: '',
            sale_percent: ''
          }
        ],
        sale_begin_time: '',
        sale_end_time: ''
      }
    };
  },
  computed: {
    sales() {
      return this.$store.getters.sales;
    },
  },
  methods: {
    editSale(row) {
      console.log('Editing sale:', row);
    },
    formatDateTime(dateTimeStr) {
      const dateTime = new Date(dateTimeStr);
      return `${dateTime.toLocaleTimeString()} ngày ${dateTime.toLocaleDateString()}`;
    },
    removeSaleItem(index) {
      this.saleDTO.sale_items.splice(index, 1);
    },
    addSaleItem() {
      this.saleDTO.sale_items.push({
        product_id: '',
        sale_percent: ''
      });
    },
    async handleAddSale() {
      try {
        await this.$store.dispatch('addSale', this.saleDTO);
        ElMessage.success('Thêm chương trình giảm giá thành công');
        this.dialogVisible = false;
        this.resetForm();
      } catch (error) {
        console.error('Error adding sale:', error);
        ElMessage.error('Thêm chương trình giảm giá không thành công');
      }
    },
    resetForm() {
      this.saleDTO = {
        sale_items: [
          {
            product_id: '',
            sale_percent: ''
          }
        ],
        sale_begin_time: '',
        sale_end_time: ''
      };
    },
  },
  created() {
    this.$store.dispatch('getAllSales');
  },
};
</script>

<style scoped>
.header-breadcrumb{
    height: 30px;
  }
.sales-management {
  padding: 0px 20px 20px 20px;
}
.el-card{
  padding: 20px;
}
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.card-header h3 {
  margin: 0;
}
.sales-list {
  margin-top: 20px;
}

.action {
  display: flex;
}
</style>
