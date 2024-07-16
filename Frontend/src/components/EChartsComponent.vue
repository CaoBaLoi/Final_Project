<template>
  <el-card v-if="showDetails" style="width: 48%; height: 380px; margin-bottom: 20px; position: relative;">
    <div style="display: flex; justify-content: space-between; align-items: center;">
      <h2 style="margin: 0;">Đơn đã đặt</h2>
      <router-link to="/order">
        <el-button type="text" style="text-decoration: underline; font-size: 16px; color: blue;" @click="changeDefaulActive">Xem chi tiết</el-button></router-link>
    </div>
    <div ref="chart" style="width: 100%; height: 350px;"></div>
  </el-card>
  <el-card v-else style="width: 48%; height: 380px; margin-bottom: 20px; margin-left: 30px;">
    <div ref="chart" style="width: 100%; height: 380px;"></div>
  </el-card>
</template>

<script>
import { ref, onMounted, watch, onUnmounted} from 'vue';
import * as echarts from 'echarts';

export default {
  methods:{
    changeDefaulActive(){
      localStorage.setItem('defaultActive', 4);
    }
  },
  name: 'EChartsComponent',
  props: {
    chartData: {
      type: Object,
      required: true
    },
    chartOptions: {
      type: Object,
      default: () => ({})
    },
    showDetails: {
      type: Boolean,
      default: false
    }
  },
  setup(props) {
    const chart = ref(null);
    let myChart = null;

    watch(() => props.chartData, () => {
      renderChart();
    });

    onMounted(() => {
      renderChart();
    });

    const cleanupChart = () => {
      if (myChart) {
        myChart.dispose();
        myChart = null;
      }
    };


    const renderChart = () => {
      if (!chart.value) return;

      if (!myChart) {
        myChart = echarts.init(chart.value);
      }

      myChart.setOption({
        ...props.chartOptions,
        ...props.chartData
      });
    };

    onUnmounted(() => {
      cleanupChart();
    });

    return {
      chart
    };
  }
};
</script>

