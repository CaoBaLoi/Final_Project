<template>
  <el-header class="header-container">
    <div class="header">
      <el-button type="text" class="logo-button">
        <router-link to="/home">
          <img src="https://res.cloudinary.com/dkhf0nsxl/image/upload/v1719386347/Blue_Orange_Modern_Illustration_House_Professional_Real_Estate_Logo_1_j1oy1j.png" class="logo-image" @click="home">
        </router-link>
      </el-button>
      <div class="nav-buttons">
        <div v-if="!loggedIn" class="auth-buttons">
          <el-button class="login-button">
            <router-link to="/login" style="text-decoration: none;">Đăng nhập</router-link>
          </el-button>
          <el-button type="primary" class="register-button">
            <router-link to="/register" style="text-decoration: none;">Đăng ký</router-link>
          </el-button>
        </div>
        <div v-else class="user-actions">
          <el-dropdown class="cart-dropdown">
            <span class="el-dropdown-link">
              <el-icon><shopping-cart /></el-icon>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item v-if="cart.length === 0">Không có gì trong giỏ hàng</el-dropdown-item>
                <el-dropdown-item v-for="(item, index) in limitedCart" :key="item.cart_id">
                  <div class="cart-item">
                    <img :src="item.product.images[0]" class="cart-item-image" />
                    <span class="cart-item-details" v-if="isOnSale(item.product.product_id)">
                      <span class="cart-item-name">{{ truncatedName(item.product.product_name) }}</span>
                      - {{ formatPrice(getSalePrice(item.product.product_id)) }}
                    </span>
                    <span class="cart-item-details" v-else>
                      <span class="cart-item-name">{{ truncatedName(item.product.product_name) }}</span>
                      - {{ formatPrice( item.product.product_price) }}
                    </span>
                  </div>
                </el-dropdown-item>
                <div v-if="cart.length > 0" class="cart-footer">
                  <router-link to="/cart">
                    <el-button type="danger" class="view-cart-button">Xem giỏ hàng</el-button>
                  </router-link>
                </div>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
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

.nav-buttons {
  display: flex;
  align-items: center;
}

.auth-buttons {
  display: flex;
  align-items: center;
}

.login-button,
.register-button {
  height: 85%;
  font-size: large;
}

.login-button {
  margin-right: 10px;
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

.cart-dropdown {
  margin-right: 20px;
}

.cart-item {
  display: flex;
  align-items: center;
}

.cart-item-image {
  width: 80px;
  height: 50px;
  object-fit: cover;
  margin-right: 10px;
}

.cart-item-details {
  font-size: large;
}

.dropdown-item {
  font-size: large;
}

.el-dropdown-link {
  font-size: xx-large;
  color: black;
}

.cart-footer {
  text-align: right;
  padding: 10px;
}

.view-cart-button {
  margin-top: 10px;
}
</style>

<script>
import { mapState } from 'vuex';
import { ShoppingCart } from '@element-plus/icons-vue';

export default {
  computed: {
    ...mapState(['loggedIn', 'username']),
    cart() {
      return this.$store.getters.cart;
    },
    limitedCart() {
      return this.cart.slice(0, 5);
    },
    sales() {
      return this.$store.getters.sales;
    },
  },
  methods: {
    truncatedName(name) {
      return name.length > 20 ? name.substring(0, 20) + '...' : name;
    },
    async Logout() {
      await this.$store.dispatch('logout');
    },
    async home() {
      localStorage.removeItem('defaultActive');
    },
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    isOnSale(productId) {
      return this.sales.some(sale => sale.saleItems.some(item => item.product_id === productId));
    },
    getSalePrice(productId) {
      for (const sale of this.sales) {
        const item = sale.saleItems.find(item => item.product_id === productId);
        if (item) {
          return item.product_price - (item.product_price * item.sale_percent / 100);
        }
      }
      return null;
    },
  },
  mounted() {
    this.$store.dispatch('fetchCart');
  },
  created() {
    this.$store.dispatch('getSales');
  },
};
</script>
