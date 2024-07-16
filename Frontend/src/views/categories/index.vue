<template>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/dashboard' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Danh mục</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
  <el-card class="category-card">
    <div class="card-header">
      <h3>Danh sách danh mục</h3>
      <el-input v-model="search" size="small" placeholder="Nhập từ khóa" style="width: 300px; margin-left: 50%;"/>
      <el-button><router-link to="/category/create" style="text-decoration: none;">Thêm mới</router-link></el-button>
    </div>
    <el-table :data="filterTableData" border style="width: 80%; margin: 20px;">
      <el-table-column label="ID" prop="category_id" />
      <el-table-column label="Tên danh mục" prop="category_name" />
      <el-table-column label="Hành động">
        <template #default="scope">
          <el-button size="small" @click="handleEdit(scope.$index, scope.row)">
            Sửa
          </el-button>
          <el-button
            size="small"
            type="danger"
            @click="handleDelete(scope.$index, scope.row)"
          >
            Xóa
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-card>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue';
import { useStore } from 'vuex';

const store = useStore();
const search = ref('');
const filterTableData = computed(() =>
  store.getters.categories.filter(
    (data) =>
      !search.value ||
      data.category_name.toLowerCase().includes(search.value.toLowerCase())
  )
);

const handleEdit = (index, row) => {
  console.log(index, row);
};

const handleDelete = (index, row) => {
  console.log(index, row);
};

onMounted(() => {
  store.dispatch('fetchCategories');
});
</script>

<style scoped>
.header-breadcrumb{
    height: 30px;
  }
.category-card {
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

.el-button {
  margin-right: 5px;
}
</style>
