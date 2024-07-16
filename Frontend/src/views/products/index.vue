<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Sản phẩm</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
  <el-card class="product-card">
    <div class="card-header">
      <h3>Danh sách sản phẩm</h3>
      <el-input v-model="search" size="small" placeholder="Nhập từ khóa" style="width: 300px; margin-left: 50%;"/>
      <el-button><router-link to="/product/create" style="text-decoration: none;">Thêm mới</router-link></el-button>
    </div>
    <el-table :data="filterTableData" border style="width: 100%;" height="700px">
      <el-table-column fixed label="ID" prop="product_id" width="50"/>
      <el-table-column label="Hình ảnh" width="125">
        <template #default="scope">
          <img :src="scope.row.image_url" alt="" style="width: 100px; height: 100px; display: block; margin: 0 auto; object-fit: fill;">
        </template>
      </el-table-column>
      <el-table-column label="Tên sản phẩm" prop="product_name"/>
      <el-table-column label="Số lượng" prop="product_quantity" width="80"/>
      <el-table-column label="Giá bán" prop="product_price" width="150">
        <template #default="scope">
          <div>{{ formatPrice(scope.row.product_price) }}</div>
        </template>
      </el-table-column>
      <el-table-column label="Hành động" width="250">
        <template #default="scope">
          <el-button size="small" @click="handleEdit(scope.$index, scope.row)">
            Sửa
          </el-button>
          <el-button
            size="small"
            type="danger"
            @click="dialogVisible2 = true"
          >
            Xóa
          </el-button>
          <el-button size="small" @click="openImportDialog(scope.row)">
            Nhập hàng
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-card>
  <el-dialog v-model="dialogVisible2" title="Xóa sản phẩm" :close-on-click-modal="false" width="20%">
    Bạn chắc chắn rằng muốn xóa sản phẩm?
    <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible2 = false">Hủy bỏ</el-button>
        <el-button type="primary">Xác nhận</el-button>
        </div>
      </template>
  </el-dialog>
  <el-dialog
      title="Nhập sản phẩm"
      v-model="dialogVisible" 
      :close-on-click-modal="false"
      width="30%">
      <el-form :model="importForm" label-width="150px">
        <el-form-item label="Số lượng">
          <el-input v-model="importForm.import_quantity" type="number"></el-input>
        </el-form-item>
        <el-form-item label="Giá">
          <el-input v-model="importForm.import_price" type="number"></el-input>
        </el-form-item>
        <el-form-item label="Ngày nhập hàng">
          <el-date-picker v-model="importForm.import_date" type="datetime" placeholder="Chọn ngày"></el-date-picker>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">Hủy bỏ</el-button>
        <el-button type="primary" @click="importProduct">Xác nhận</el-button>
        </div>
      </template>
    </el-dialog>
</template>

<script setup >
import { ElMessage } from 'element-plus';
import { computed, ref, onMounted } from 'vue';
import { useStore } from 'vuex';

const store = useStore();
const search = ref('');
const dialogVisible = ref(false);
const dialogVisible2 = ref(false);
const importForm = ref({
  product_id: null,
  import_quantity: 1,
  import_price: 0,
  import_date :''
});
const filterTableData = computed(() =>
  store.getters.products.filter(
    (data) =>
      !search.value ||
      data.product_name.toLowerCase().includes(search.value.toLowerCase())
  )
);
const formatPrice = (value) => {
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
};
const handleEdit = (index, row) => {
  console.log(index, row);
};

const handleDelete = (index, row) => {
  console.log(index, row);
};
const openImportDialog = (row) => {
  importForm.value.product_id = row.product_id;
  dialogVisible.value = true;
};
const importProduct = () => {
  store.dispatch('importProduct', importForm.value)
    .then(() => {
      dialogVisible.value = false;
      console.log('Product imported successfully');
      ElMessage.success('Nhập hàng thành công')
      store.dispatch('fetchProducts');
    })
    .catch((error) => {
      console.error('Error importing product:', error.message);
      ElMessage.error('Nhập hàng không thành công')
    });
};
onMounted(() => {
  store.dispatch('fetchProducts');
});
</script>

<style scoped>
.header-breadcrumb{
    height: 30px;
  }
.product-card {
  margin: 0px 20px 20px 20px;
  padding: 20px; 
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.card-header h3 {
  margin: 0;
}

.el-table {
  width: 100%;
}

.el-table-column {
  overflow: hidden;
}

.el-table__body .el-table__column--image img {
  display: block;
  margin: 0 auto;
  max-width: 100px;
  max-height: 100px;
}

.el-button {
  margin-right: 5px;
}

</style>
