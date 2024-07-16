<template>
    <el-header class="header-breadcrumb">
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: '/home' }">Trang chủ</el-breadcrumb-item>
          <el-breadcrumb-item>Bảng tổng quan</el-breadcrumb-item>
        </el-breadcrumb>
    </el-header>
    <div>
      <el-card class="card-product" >
        <div style="display: flex; align-items: center;">
          <p style="font-size: 18px; font-weight: bold;">Có {{ totalProducts() }} sản phẩm đang được bán</p>
          <router-link to="/product" style="margin-left: 30px; color: blue;">Xem chi tiết</router-link>
        </div>
        <p style="font-size: 18px; font-weight: bold;">Có {{ product() }} sản phẩm sắp hết hàng</p>
      </el-card>
    </div>
    <div class="chart">
        <EChartsComponent :chart-data="chartDataOrdersMonthly" :chart-options="chartOptionsOrdersMonthly" :show-details="true"/>
        <EChartsComponent :chart-data="chartDataMonthly" :chart-options="chartOptionsMonthly" :show-details="false"/>
    </div>
  </template>
  
  <script>
  import EChartsComponent from '../components/EChartsComponent.vue';
  import { mapGetters } from 'vuex/dist/vuex.cjs.js';
  export default {
    components: {
      EChartsComponent
    },
    computed: {
      ...mapGetters(['products']),
      importproduct() {
        return this.$store.getters.importproduct;
      },
      orders() {
        return this.$store.getters.orders;
      },
      chartDataOrdersMonthly() {
      let monthlyOrders = Array(12).fill(0);
      
      this.orders.forEach(order => {
        let month = new Date(order.order_date).getMonth();
        monthlyOrders[month]++;
      });
      
      return {
        xAxis: {
          type: 'category',
          data: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
        },
        yAxis: {
          type: 'value'
        },
        series: [{
          name: 'Đơn đặt hàng',
          type: 'bar',
          data: monthlyOrders
        }]
      };
    },
    chartOptionsOrdersMonthly() {
      return {
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'shadow'
          },
          formatter: function(params) {
            const month = params[0].name;
            const orders = params[0].value;
            return `${month}<br/>Số đơn hàng: ${orders}`;
          }
        }
      };
    },
      chartDataMonthly() {
        let monthlyProfit = Array(12).fill(0);
        this.orders.forEach(order => {
          let month = new Date(order.order_date).getMonth();
          order.orderDetails.forEach(detail => {
            let orderDetailPrice = detail.order_detail_price;
            let importProduct = this.importproduct.find(product => product.product_id === detail.product_id);
            if (importProduct) {
              let importPrice = importProduct.import_price * detail.order_detail_quantity;
              let profit = orderDetailPrice - importPrice;
              monthlyProfit[month] += profit;
            }
          });
        });
  
        return {
          xAxis: {
            type: 'category',
            data: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
          },
          yAxis: {
            type: 'value'
          },
          series: [{
            name: 'Doanh thu',
            type: 'line',
            data: monthlyProfit
          }]
        };
      },
      chartOptionsMonthly() {
        return {
          title: {
            text: 'Doanh thu',
            textStyle: {
            fontFamily: 'Arial, sans-serif',
            fontSize: 20,
            fontWeight: 'bold', 
            color: '#333' 
          }
          },
          tooltip: {
            trigger: 'axis',
            axisPointer: {
              type: 'line'
            }
          }
        };
      },
    },
    methods:{
      totalProducts() {
        console.log(this.products.length)
        return this.products.length;
      },
      product() {
        console.log(this.products.filter(product => product.product_quantity < 20).length)
        return this.products.filter(product => product.product_quantity < 20).length;
      },
    },
    created() {
      this.$store.dispatch('getImportProduct');
      this.$store.dispatch('getOrders');
      this.$store.dispatch('fetchProducts');
    }
  };
  </script>
  
  <style scoped>
.header-breadcrumb{
    height: 30px;
  }
  .chart{
    margin-left: 20px;
    display: flex;
    margin-top: 30px;
  }
  .card-product{
    width: 25%;
    height: 150px;
    margin-left: 20px;
  }
  </style>
  