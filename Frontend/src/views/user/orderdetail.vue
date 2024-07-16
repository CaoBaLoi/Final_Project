<template>
    <el-container>
      <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/home' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item :to="{ path: '/user/order' }">Đơn hàng</el-breadcrumb-item>
          <el-breadcrumb-item>Chi tiết đơn hàng</el-breadcrumb-item>
        </el-breadcrumb>
      </el-header>
      <el-main>
        <el-card v-if="order">
          <h2>Chi tiết đơn hàng</h2>
          <el-row :gutter="20">
            <el-col :span="12">
              <el-form :model="order" label-width="150px">
                <el-form-item label="Mã đơn hàng">
                  <el-input v-model="order.orders_id" disabled></el-input>
                </el-form-item>
                <el-form-item label="Ngày đặt hàng">
                  <el-input v-model="order.order_date" disabled :formatter="formatDateTime"></el-input>
                </el-form-item>
                <el-form-item label="Tình trạng">
                  <el-input v-model="order.order_status" disabled></el-input>
                </el-form-item>
                <el-form-item label="Ngày nhận hàng" v-if="receiveddate">
                  <el-input v-model="receiveddate.received_date" disabled :formatter="formatDateTime"></el-input>
                </el-form-item>
              </el-form>
            </el-col>
            <el-col :span="12">
              <el-card style="width: 100%;">
                <h3>Địa chỉ nhận hàng</h3>
                <p>{{ order.order_address }}</p>
              </el-card>
            </el-col>
          </el-row>
          <el-divider></el-divider>
        <div class="order-items">
          <div class="order-items-product">
            <h3>Danh sách sản phẩm</h3>
            <el-table :data="order.orderDetails" style="width: 100%" v-if="order.orderDetails">
              <el-table-column prop="image_url" label="" width="180">
                <template #default="scope">
                  <img :src="scope.row.image_url" alt="product image" style="width: 100px; height: auto;">
                </template>
              </el-table-column>
              <el-table-column prop="product_name" label="Tên sản phẩm" width="250"></el-table-column>
              <el-table-column prop="order_detail_quantity" label="Số lượng" width="100"></el-table-column>
              <el-table-column prop="order_detail_price" label="Tổng tiền hàng" width="120">
                <template #default="scope">
                  {{ formatPrice(scope.row.order_detail_price) }}
                </template>
              </el-table-column>
            </el-table>
          </div>
          <div class="right-order_items">
            <div style="display: flex; align-items: center;">
              <h3>Phương thức thanh toán:</h3>
              <span style="margin-left: 20px; font-size: 22px;">{{ order.payment_method }}</span>
            </div>
            <div style="display: flex; align-items: center;">
              <h3>Phí vận chuyển:</h3>
              <span style="margin-left: 20px; font-size: 22px;">20.000 đ</span>
            </div>
            <div style="display: flex; align-items: center;">
              <h3>Tổng thanh toán:</h3>
              <span style="margin-left: 20px; font-size: 22px;">{{ formatPrice(order.order_total_price) }}</span>
            </div>
            <el-button v-if="order.order_status === 'Chờ xác nhận'" type="danger" @click="cancleOrder(order.orders_id)">
              Hủy đơn hàng
            </el-button>
            <el-button v-if="order.order_status === 'Chờ giao hàng'" type="success" @click="confirmOrder(order.orders_id)">
              Đã nhận hàng
            </el-button>
            <el-button v-if="order.order_status === 'Đã giao' && !allProductsReviewed" type="success" @click="startFeedbackQueue()">
              Đánh giá
            </el-button>
            <el-button v-if="order.order_status === 'Đã giao' && allProductsReviewed" type="primary" disabled>
              Đã đánh giá
            </el-button>
          </div>
          </div>
        </el-card>
      </el-main>
    </el-container>
    <el-dialog
      title="Đánh giá sản phẩm"
      v-model="dialogVisible" 
      :close-on-click-modal="false"
      width="30%">
      <el-form :model="feedback" label-width="150px">
        <el-form-item label="">
          <el-rate v-model="feedback.feedback_point" />
        </el-form-item>
        <el-form-item label="Đánh giá">
          <el-input v-model="feedback.feedback" type="textarea"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">Hủy bỏ</el-button>
        <el-button type="primary" @click="sendFeedback()">Xác nhận</el-button>
        </div>
      </template>
    </el-dialog>
  </template>
  
  <script>

import { ElMessageBox, ElMessage } from 'element-plus';
import { mapState, mapActions } from 'vuex';
  export default {
    data() {
      return {
        dialogVisible: false,
        feedbackQueue: [],
        feedback:{
          product_id:'',
          feedback:'',
          feedback_date:'',
          feedback_point:0
        }
      };
    },
    props: {
    id: {
      type: Number,
      required: true
    }
  },
    computed:{
      ...mapState(['order']),
      ...mapState(['receiveddate']),
      ...mapState(['feedbacks']),
      allProductsReviewed() {
      return this.order.orderDetails.every(detail => this.hasFeedback(detail));
    }
    },
    created() {
    this.getFeedBackByUser();
  },
    watch: {
      id: {
        immediate: true,
        handler(newId) {
          this.getOrder(newId);
          this.getReceivedDate(newId);
        }
      }
    },
    methods:{
      ...mapActions(['getOrder']),
      ...mapActions(['getReceivedDate']),
      ...mapActions(['addFeedBack']), 
      ...mapActions(['getFeedBackByUser']),
    formatPrice(value) {
      return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' đ';
    },
    getCurrentDateInGMT7() {
      const offset = 7;
      const date = new Date();
      const utcDate = new Date(date.getTime() + (date.getTimezoneOffset() * 60000));
      const gmt7Date = new Date(utcDate.getTime() + (offset * 3600000));
      return gmt7Date.toISOString();
    },
    formatDateTime(dateTimeStr) {
        const dateTime = new Date(dateTimeStr);
        return `${dateTime.toLocaleTimeString()} ngày ${dateTime.toLocaleDateString()}`;
    },
    confirmOrder(orders_id) {
      ElMessageBox.confirm('Xác nhận đã nhận được hàng?', 'Xác nhận', {
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Hủy bỏ',
        type: 'warning'
      }).then(() => {
        const status = 'Đã giao';
        this.$store.dispatch('changeStatusOrder', {status, order_id: orders_id}).then(() => {
          const received_date = this.getCurrentDateInGMT7();
          this.$store.dispatch('addReceivedDate',{order_id: orders_id,received_date})
          this.getOrder(orders_id)
          ElMessage.success('Cập nhật thành công');
        }).catch(error => {
          console.error('Lỗi khi cập nhật trạng thái đơn hàng:', error);
          ElMessage.error('Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng');
        });
      }).catch(() => {
      });
    },
    cancleOrder(orders_id) {
      ElMessageBox.confirm('Bạn muốn hủy đơn hàng?', 'Xác nhận', {
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Hủy bỏ',
        type: 'warning'
      }).then(() => {
        const cencle_status = 'Đã hủy';
        this.$store.dispatch('changeStatusOrder', { status: cencle_status, order_id: orders_id })
          .then(async () => {
            try {
              const order = await this.getOrder(orders_id);
              if (order && order.orderDetails) {
                for (const detail of order.orderDetails) {
                  const product_id = detail.product_id;
                  const order_detail_quantity = detail.order_detail_quantity;
                  try {
                    await this.$store.dispatch('updateProductQuantity', { product_id, quantity: order_detail_quantity });
                    console.log('Quantity updated successfully for product:', product_id);
                  } catch (error) {
                    console.error('Failed to update quantity for product:', product_id, error);
                  }
                }
              } else {
                console.error('Order details are missing or empty');
              }
              ElMessage.success('Đơn hàng đã được hủy');
            } catch (error) {
              console.error('Failed to fetch order details:', error);
              ElMessage.error('Đã xảy ra lỗi khi hủy đơn hàng');
            }
          })
          .catch(error => {
            console.error('Lỗi khi cập nhật trạng thái đơn hàng:', error);
            ElMessage.error('Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng');
          });
      }).catch(() => {});
    },
    startFeedbackQueue() {
      this.feedbackQueue = this.order.orderDetails.filter(detail => !this.hasFeedback(detail));
      this.openFeedbackDialog();
    },
    hasFeedback(detail) {
      const receivedDate = this.receiveddate ? new Date(this.receiveddate.received_date) : null;
      
      return receivedDate &&  this.feedbacks.some(fb => fb.product_id === detail.product_id && new Date(fb.feedback_date) >= receivedDate);
    },
     openFeedbackDialog() {
      if (this.feedbackQueue.length > 0) {
        const detail = this.feedbackQueue.shift(); 
        this.feedback.product_id = detail.product_id; 
        this.dialogVisible = true; 
      } else {
        this.dialogVisible = false;
      }
    },
    async sendFeedback() {
      try {
        const { product_id, feedback, feedback_point } = this.feedback;
        const feedback_date = this.getCurrentDateInGMT7();
        await this.$store.dispatch('addFeedBack', { product_id, feedback, feedback_date, feedback_point });
        ElMessage.success('Cảm ơn bạn đã để lại đánh giá');
        this.feedback.feedback_point = 0;
        this.feedback.feedback = '';
        this.dialogVisible = false;
        this.$nextTick(() => {
          this.openFeedbackDialog();
        });

      } catch (error) {
        console.error('Lỗi khi gửi phản hồi:', error);
        ElMessage.error('Đã xảy ra lỗi khi gửi phản hồi');
      }
    },
    },
  };
  </script>

  <style scoped>
  .header-breadcrumb{
    margin-top: 50px;
    height: 30px;
  }
  .el-card {
    margin-bottom: 20px;
    width: 80%;
  }
  .order-items{
    display: flex;
    width: 100%;
  }
  .order-items-product{
    width: 70%;
  }
  </style>
  