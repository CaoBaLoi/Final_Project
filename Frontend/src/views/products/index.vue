<template>
  <el-button><router-link to="/product/create" style="text-decoration: none;">Thêm mới</router-link></el-button>
    <el-table :data="filterTableData" border style="width: 80%;">
      <el-table-column label="ID" prop="product_id" width="50"/>
      <el-table-column label="Tên sản phẩm" prop="product_name"/>
      <el-table-column label="Hình ảnh" width="125">
        <template #default="scope">
          <img :src="scope.row.image_url" alt="" style="max-width: 100px; max-height: 100px;">
        </template>
      </el-table-column>
      <el-table-column label="Số lượng" prop="product_quantity" width="80"/>
      <el-table-column label="Giá bán" prop="product_price" width="150"/>
      <el-table-column align="right" width="150">
        <template #header>
          <el-input v-model="search" size="small" placeholder="Nhập từ khóa" />
        </template>
        <template #default="scope">
          <el-button size="small" @click="handleEdit(scope.$index, scope.row)">
            Edit
          </el-button>
          <el-button
            size="small"
            type="danger"
            @click="handleDelete(scope.$index, scope.row)"
          >
            Delete
          </el-button>
        </template>
      </el-table-column>
    </el-table>
</template>

<script lang="ts" setup>
  import { computed, ref, onMounted } from 'vue';
  import { useStore } from 'vuex';

  const store = useStore();
  const search = ref('');
  const filterTableData = computed(() =>
    store.getters.products.filter(
      (data) =>
        !search.value ||
        data.product_name.toLowerCase().includes(search.value.toLowerCase())
    )
  );

  const handleEdit = (index, row) => {
    console.log(index, row);
  };

  const handleDelete = (index, row) => {
    console.log(index, row);
  };

  onMounted(() => {
    store.dispatch('fetchProducts');
  });
</script>
