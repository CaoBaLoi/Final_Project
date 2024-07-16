<template>
    <el-header class="header-container">
    <div class="header">
      <el-button type="text" class="logo-button">
        <router-link to="/home">
          <img src="https://res.cloudinary.com/dkhf0nsxl/image/upload/v1719386347/Blue_Orange_Modern_Illustration_House_Professional_Real_Estate_Logo_1_j1oy1j.png" class="logo-image" @click="home">
        </router-link>
        <span class="checkout-text">| Thanh toán</span>
      </el-button>
      <div class="nav-buttons">
        <div class="user-actions">
          <el-dropdown class="user-dropdown">
            <span class="username"><i class="fa-solid fa-user"></i> {{ username }}</span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>
                  <router-link to="/user/profile" class="dropdown-item" style="text-decoration: none; color: inherit;">Tài khoản của tôi</router-link>
                </el-dropdown-item>
                <el-dropdown-item @click="Logout" class="dropdown-item">
                  Đăng xuất
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </div>
    </div>
  </el-header>
  <div>
    <div class="checkout-address">
      <div class="address-text">
        <el-icon><LocationFilled /></el-icon>
        <p style="margin-left: 10px;">Địa chỉ nhận hàng</p>
      </div>
      <div class="address" v-if="selectedAddress">
        <p style="font-weight: bold;">{{ selectedAddress.name }} {{ selectedAddress.phone }}</p>
        <p style="margin-left: 20px;">{{ selectedAddress.address }}</p>
        <el-button type="primary" plain style="margin-left: 40px;" @click="dialogVisible = true">Thay đổi</el-button>
      </div>
    </div>
    <div class="checkout-items">
      <el-table :data="checkoutItems">
        <el-table-column label="Sản phẩm">
        <template v-slot="{ row }">
          <div class="product-info">
            <img v-if="row && row.product.images" :src="row.product.images" alt="Product Image" class="product-image"/>
            <span class="product-name">{{ row && row.product.product_description }}</span>
          </div>
        </template>
      </el-table-column>
        <el-table-column label="Đơn giá" width="150">
          <template v-slot="{ row }">
            {{ formatPrice( row.product.product_price) }}
          </template>
        </el-table-column>
        <el-table-column label="Só lượng" width="100">
          <template v-slot="{ row }">
            {{ row.quantity }}
          </template>
        </el-table-column>
        <el-table-column label="Thành tiền" width="150">
          <template v-slot="{ row }">
            {{ formatPrice( row && row.product.product_price * row.quantity) }}
          </template>
        </el-table-column>
      </el-table>
      <div class="payment">
        <div class="payment-method">
          <h3>Phương thức thanh toán</h3>
          <div class="payment-options">
            <button :class="{ active: selectedMethod === 'cod' }" @click="selectMethod('cod')">Thanh toán khi nhận hàng</button>
            <button :class="{ active: selectedMethod === 'vnpay' }" @click="selectMethod('vnpay')">Thanh toán qua VnPay</button>
          </div>
        </div>
        <div class="payment-details">
          <div v-if="selectedMethod === 'cod'">
            <p>Thanh toán khi nhận hàng</p>
          </div>
          <div v-if="selectedMethod === 'vnpay'">
            <p>Thanh toán qua VnPay</p>
          </div>
        </div>
        <div class="confirm-checkout">
          <p>Tổng tiền hàng: <span style="margin-left: 43px;">{{ formatPrice(totalItemsPrice) }}</span></p>
          <p>Phí vận chuyển: <span style="margin-left: 52px;">{{ formatPrice(shippingFee) }}</span></p>
          <p>Tổng thanh toán: <span style="color: rgb(255,69,0);font-size: x-large;font-weight: bold; margin-left: 10px;">{{ formatPrice(totalAmount) }}</span></p>
        </div>
        <div class="btn-checkout">
          <el-button type="danger" @click="checkout">Đặt hàng</el-button>
        </div>
      </div>
    </div>
    <el-dialog title="Địa chỉ của tôi" v-model="dialogVisible" :close-on-click-modal="false" style="max-width: 450px;">
      <div class="card-content">
        <div v-for="iaddress in address" :key="iaddress.address_id" class="address-items">
          <div class="radio-select">
            <input type="radio" :checked="iaddress === tempSelectedAddress" @change="tempSelectedAddress = iaddress">
          </div>
          <div class="item-select">
            <label>
              <p prop="name">{{ iaddress.name }}</p>
              <p prop="phone">{{ iaddress.phone }}</p>
              <p prop="address">{{ iaddress.address }}</p>
            </label>
            <el-divider></el-divider>
          </div>
          
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancelAddress">Hủy</el-button>
        <el-button type="primary" @click="confirmAddress">Xác nhận</el-button>
      </div>
    </el-dialog>
  </div>
  <navfooter />
</template>

<style>
.header-container {
  height: 100px;
  background-color: antiquewhite;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px;
}

.header {
  width: 75%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-left: auto;
  margin-right: auto;
}

.logo-button {
  height: 100%;
  padding: 0;
  display: flex;
  align-items: center;
  margin-right: 10px;
}

.logo-image {
  max-height: 100px;
  object-fit: contain;
}

.checkout-text {
  font-size: xx-large;
  color: black;
  margin-left: 10px;
}
.user-actions {
  display: flex;
  align-items: center;
}

.username {
  font-size: x-large;
  color: black;
  font-weight: bold;
  margin-left: 10px;
}
.checkout-items{
  width: 60%;
  margin-left: auto;
  margin-right: auto;
  margin-top: 20px;
}
.product-image {
  width: 100px;
  height: 100px;
  margin-right: 10px;
}
.product-info {
  display: flex;
  align-items: center;
  padding-left: 20px;
}
.product-name {
  font-size: 16px;
  font-weight: bold;
}
.checkout-address{
  width: 60%;
  margin-left: auto;
  margin-right: auto;
  margin-top: 50px;
  box-sizing: border-box;
  padding: 0px 10px 10px 30px;
  background-color: white;
}
.address-text{
  display: flex;
  font-size: x-large;
  font-weight: bold;
  color: red;
  align-items: center ;
}
.address-items{
  display: flex;
  box-sizing: border-box;
}
.address{
  display: flex;
  align-items: center;
  font-size: large;
}
.radio-select{
  width: 10%;
  padding-top: 10px;
}
.item-select{
  width: 100%;
}
.dialog-footer{
  text-align: right;
  justify-content: flex-end;
}
.payment{
  background-color: white;
  margin-top: 20px;
  padding: 20px;
}
.payment-method{
  display: flex;
  align-items: center;
  border-bottom: 1px solid #ccc;
}
.payment-options{
  margin-left: 20px;
}
.payment-method button {
  border: 1px solid #ccc;
  padding: 10px;
  cursor: pointer;
  background-color: transparent;
  margin-right: 10px;
  outline: none;
}
.payment-method button.active {
  border-color: red
}
.payment-details {
  margin-top: 10px;
  border-bottom: 1px solid #ccc;
}
.confirm-checkout{
  text-align: end;
}
.btn-checkout{
  text-align: end;
}
</style>
<script setup>
import navfooter from "../../components/Footer.vue";
</script>
<script>
import { mapState } from 'vuex';
import { LocationFilled } from '@element-plus/icons-vue';
import { ElButton } from 'element-plus';
import axios from "axios";
export default {
  data() {
    return {
      dialogVisible: false,
      checkoutItems: [],
      tempSelectedAddress: null,
      selectedAddress: null, 
      defaultAddressIndex: 0,
      selectedMethod: 'cod',
      shippingFee: 20000
    };
  },
  created() {
    const items = localStorage.getItem('checkoutItems');
    if (items) {
      this.checkoutItems = JSON.parse(items);
    }
    const storedMethod = localStorage.getItem('selectedMethod');
    if (storedMethod) {
      this.selectedMethod = storedMethod;
    }
  },
  computed: {
    ...mapState(['loggedIn', 'username']),
    address() {
      return this.$store.getters.address;
    },
    totalItemsPrice() {
      return this.checkoutItems.reduce((total, item) => total + (item.product.product_price * item.quantity), 0);
    },
    totalAmount() {
      return this.totalItemsPrice + this.shippingFee;
    }
  },
  methods: {
    async Logout() {
      await this.$store.dispatch('logout');
    },
    async home() {
      localStorage.removeItem('defaultActive');
    },
    confirmAddress() {
      if (this.tempSelectedAddress) {
        this.selectedAddress = { ...this.tempSelectedAddress };
      }
      this.dialogVisible = false;
    },
    cancelAddress() {
      this.tempSelectedAddress = this.selectedAddress;
      this.dialogVisible = false;
    },
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    selectMethod(method) {
      localStorage.setItem('selectedMethod', method);
      this.selectedMethod = method;
    },
    async checkout() {
      try {
        let payment_method = '';
        let order_status = '';

        if (localStorage.getItem('selectedMethod') === 'vnpay') {
          payment_method = 'vnpay';
          order_status = 'Chờ giao hàng';
        } else {
          payment_method = 'cod'; 
          order_status = 'Chờ xác nhận'; 
        }

        const orderDetails = this.checkoutItems.map(item => ({
          product_id: item.product_id,
          order_detail_quantity: item.quantity,
          order_detail_price: item.product.product_price * item.quantity
        }));
        const addressInfo = this.selectedAddress
          ? `${this.selectedAddress.name}, ${this.selectedAddress.phone}, ${this.selectedAddress.address}`
          : '';

        const orders = {
          order_total_price: this.totalAmount,
          order_address: addressInfo,
          order_status: order_status,
          payment_method: payment_method,
          orderDetails: orderDetails
        };

        if (localStorage.getItem('selectedMethod') === 'vnpay') {
          const response = await axios.post('http://localhost:5183/api/order/place', orders);
          if (response.data.paymentUrl) {
            console.log("paymentUrl: " + response.data.paymentUrl);
            window.location.href = response.data.paymentUrl;
          } else {
            console.log("Không có paymentUrl");
          }
        } else {
          await this.$store.dispatch('addOrder', orders);
        }
      } catch (error) {
        console.error('Lỗi khi thực hiện đơn hàng:', error);
      }
    }

  },
  mounted() {
    this.$store.dispatch('getAddress').then(() => {
      if (this.address.length > 0) {
        this.selectedAddress = this.address[this.defaultAddressIndex];
      }
    });
  },
  beforeRouteLeave(to, from, next) {
    localStorage.removeItem('checkoutItems');
    localStorage.removeItem('selectedMethod')
    next();
  }
};
</script>