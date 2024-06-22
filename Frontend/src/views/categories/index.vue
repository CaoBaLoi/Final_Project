<template>
  <el-button><router-link to="/category/create" style="text-decoration: none;">Thêm mới</router-link></el-button>
    <el-table :data="filterTableData" style="width: 80%">
      <el-table-column label="ID" prop="category_id" />
      <el-table-column label="Tên danh mục" prop="category_name" />
      <el-table-column align="right">
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
  