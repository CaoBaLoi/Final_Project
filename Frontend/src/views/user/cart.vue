<template>
  <el-header class="header-container">
    <div class="header">
      <el-button type="text" class="logo-button">
        <router-link to="/home">
          <img src="https://res.cloudinary.com/dkhf0nsxl/image/upload/v1719386347/Blue_Orange_Modern_Illustration_House_Professional_Real_Estate_Logo_1_j1oy1j.png" class="logo-image" @click="home">
        </router-link>
        <span class="cart-text">| Giỏ hàng</span>
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
  <div class="cart-body">
    <el-table ref="cartTable" :data="cart" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column label="">
        <template v-slot="{ row }">
          <div class="product-info">
            <img v-if="row && row.product.images" :src="row.product.images" alt="Product Image" class="product-image"/>
            <span class="product-name">{{ row && row.product.product_description }}</span>
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="price" label="Đơn giá" width="150">
        <template v-slot="{ row }">
          <span class="price" v-if="isOnSale(row.product_id)">{{ formatPrice(getSalePrice( row && row.product.product_id))}}</span>
          <span class="price" v-else>{{ formatPrice(row && row.product.product_price)}}</span>
        </template>
      </el-table-column>
      <el-table-column label="Số lượng" width="200">
        <template v-slot="{ row }">
          <el-input-number size="small" v-model="row.quantity" @input="updateQuantity(row)" :min="1"></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="Số tiền" class="price" width="150">
        <template v-slot="{ row }">
          <span class="price" v-if="isOnSale(row.product_id)">{{ formatPrice( getSalePrice( row && row.product.product_id) * row.quantity) }}</span>
          <span class="price" v-else>{{ formatPrice( row && row.product.product_price * row.quantity) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Thao tác" width="150">
        <template v-slot="{ row }">
          <el-button type="danger" @click="removeFromCart(row)">Xóa</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="cart-footer">
    <div class="total">
      <span>Thành tiền: {{ formatPrice(totalPrice) }}</span>
    </div>
    <el-button type="primary" @click="checkout">Mua hàng</el-button>
  </div>
  </div>
  <navfooter />
</template>

<script>
import { mapState } from 'vuex';
import { ElMessageBox, ElMessage } from 'element-plus';
export default {
  data() {
    return {
      selectedItems: [],
      productId: null,
    };
  },
  computed: {
    ...mapState(['loggedIn', 'username']),
    cart() {
      return this.$store.getters.cart;
    },
    totalPrice() {
    return this.selectedItems.reduce((total, item) => {
      const price = item.product.product_price;
      const quantity = item.quantity;
      if (this.isOnSale(item.product.product_id)) {
        const salePrice = this.getSalePrice(item.product.product_id);
        total += salePrice * quantity;
      } else {
        total += price * quantity;
      }
      return total;
    }, 0);
  },
    sales() {
      return this.$store.getters.sales;
    },
  },
  methods: {
    async Logout() {
      await this.$store.dispatch('logout');
    },
    async home() {
      localStorage.removeItem('defaultActive');
    },
    async updateQuantity(item) {
        try {
          await this.$store.dispatch('addToCart', { 
              product_id : item.product_id,
              quantity : item.quantity
            });
            await this.$store.dispatch('fetchCart');
      } catch (error) {
        console.error('Error adding to cart:', error);
      }
    },
    handleSelectionChange(val) {
      this.selectedItems = val;
    },
    async removeFromCart(item) {
      try {
          await this.$store.dispatch('removeFromCart', item.product_id);
          ElMessage.success('Sản phẩm đã được xóa khỏi giỏ hàng')
      } catch (error) {
        console.error('Error adding to cart:', error);
        ElMessage.error('Không thể xóa sản phẩm khỏi giỏ hàng')
      }
    },
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    checkout(){
      if (this.selectedItems.length === 0) {
        ElMessageBox.alert('Bạn không chọn sản phẩm nào!','Lỗi',{type:'error'})
        return;
      }
      this.selectedItems.forEach(item => {
        if (this.isOnSale(item.product.product_id)) {
            item.product.product_price = this.getSalePrice(item.product.product_id);
        }
    });
      const items = JSON.stringify(this.selectedItems);
      localStorage.setItem('checkoutItems',items)
      console.log('Items to checkout:', items);
      this.$router.push({ name: 'checkout'});
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
    selectProductInCart() {
      if (this.productId && this.cart.length > 0) {
        const productToSelect = this.cart.find(item => item.product_id === parseInt(this.productId));
        if (productToSelect) {
          this.$nextTick(() => {
            this.$refs.cartTable.toggleRowSelection(productToSelect, true);
          });
          this.selectedItems = [productToSelect];
        }
      }
    },
  },
  watch: {
    cart: {
      handler(newCart) {
        this.selectProductInCart();
      },
      deep: true,
      immediate: true
    }
  },
  mounted() {
    this.$store.dispatch('fetchCart');
    this.productId = this.$route.query.productId;
    this.selectProductInCart();
  },
  created() {
    this.$store.dispatch('getSales');
  },
};
</script>
<script setup>
import navfooter from "../../components/Footer.vue";
</script>
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

.cart-text {
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

.cart-body{
  width: 70%;
  margin-left: auto;
  margin-right: auto;
  margin-top: 50px;
}
.product-image {
  width: 100px;
  height: 100px;
  margin-right: 10px;
}
.product-info {
  display: flex;
  align-items: center;
}
.product-name {
  font-size: 16px;
  font-weight: bold;
}
.total {
  margin-top: 20px;
  font-size: 18px;
  font-weight: bold;
}
.price{
  font-weight: bold;
}
.cart-footer{
  align-items: end;
}
</style>