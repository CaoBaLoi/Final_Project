<template> 
<el-container>
  <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/home' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Địa chỉ</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
    <el-main>
  <el-card style="max-width: 800px;">
    <div class="card-header">
      <div class="content">
        <span>Địa chỉ</span>
        <el-button type="primary" size="small" @click="dialogVisible = true">Thêm mới</el-button>
      </div>
      <el-divider></el-divider>
    </div>
    <div class="card-content">
      <div v-for="iaddress in address" :key="iaddress.address_id" class="address-item">
        <p prop="name">{{ iaddress.name }}</p>
        <p prop="phone">{{ iaddress.phone }}</p>
        <p prop="address">{{ iaddress.address }}</p>
        <el-divider></el-divider>
      </div>
    </div>

    <el-dialog title="Thêm địa chỉ mới" v-model="dialogVisible"  style="max-width: 450px;">
      <el-form ref="newAddressForm" :model="newAddress" :rules="rules" label-width="120px">
        <el-form-item label="Họ và tên" prop="name">
          <el-input v-model="newAddress.name"></el-input>
        </el-form-item>
        <el-form-item label="Số điện thoại" prop="phone">
          <el-input v-model="newAddress.phone"></el-input>
        </el-form-item>
        <el-form-item label="Địa chỉ cụ thể" prop="address">
          <el-input v-model="newAddress.address"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
      <div class="dialog-footer">
        <el-button @click="dialogVisible = false">Hủy</el-button>
        <el-button type="primary" @click="saveNewAddress">Lưu</el-button>
      </div>
    </template>
    </el-dialog>
  </el-card>
</el-main>
  </el-container> 
</template>

<script>
import { ElMessage } from 'element-plus';
export default {
 
  data() {
    return {
      
      dialogVisible: false,
      newAddress: {
        name: '',
        phone: '',
        address: ''
      },
      rules: {
        name: [
          { required: true, message: 'Vui lòng nhập họ tên', trigger: 'blur' }
        ],
        phone: [
          { required: true, message: 'Vui lòng nhập số điện thoại', trigger: 'blur' },
          { pattern: /^[0-9]{10}$/, message: 'Số điện thoại không hợp lệ', trigger: 'blur' }
        ],
        address: [
          {required: true, message: 'Vui lòng nhập địa chỉ', trigger: 'blur'}
        ]
      },
    };
  },
  computed: {
    address() {
      return this.$store.getters.address;
    }
  },
  methods: {
    async saveNewAddress() {
      try {
          await this.$refs.newAddressForm.validate();
          await this.$store.dispatch('addAddress', { 
              name : this.newAddress.name,
              phone : this.newAddress.phone,
              address : this.newAddress.address
            });
            this.resetNewAddressForm();
            this.dialogVisible = false;
            await this.$store.dispatch('getAddress');
            ElMessage.success('Thêm địa chỉ thành công')
      } catch (error) {
        console.error('Error adding to address:', error);
        ElMessage.error('Thêm địa chỉ thất bại')
      }
    },
    resetNewAddressForm() {
      this.newAddress = {
        fullname: '',
        phone: '',
        address: ''
      };
    },
  },
  mounted() {
    this.$store.dispatch('getAddress');
  }
};
</script>

<style scoped>
.header-breadcrumb{
    margin-top: 50px;
    height: 30px;
  }
.card-header {
  display: flex;
  flex-direction: column;
}

.content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.address-item {
  margin-bottom: 20px;
}

.el-card .card-header {
  font-size: xx-large;
}

.dialog-footer {
  text-align: right;
  justify-content: flex-end;
}
</style>
