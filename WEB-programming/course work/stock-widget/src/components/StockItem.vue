<template>
  <div class="stock-item">
    <div class="stock-chart">
      <canvas ref="chart"></canvas>
    </div>
    <div class="stock-info">
      <div class="stock-name">{{ name }}</div>
      <div class="stock-symbol">{{ symbol }}</div>
      <div class="stock-price">${{ price.toFixed(2) }}</div>
    </div>
  </div>
</template>

<script>
import { Chart } from 'vue-chartjs';
import StockDataService from '../services/StockDataService';

export default {
  props: {
    symbol: {
      type: String,
      required: true,
    },
    name: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      price: 0,
      chart: null,
    };
  },
  mounted() {
    this.fetchStockData();
    this.startAutoUpdate();
  },
  beforeDestroy() {
    this.stopAutoUpdate();
  },
  methods: {
    async fetchStockData() {
      const stockData = await StockDataService.getStockData(this.symbol);
      this.price = stockData.price;

      this.chart = new Chart(this.$refs.chart, {
        type: 'line',
        data: {
          labels: stockData.dates,
          datasets: [
            {
              label: this.symbol,
              data: stockData.prices,
              borderColor: 'rgba(75, 192, 192, 1)',
              backgroundColor: 'rgba(75, 192, 192, 0.2)',
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
        },
      });
    },
    startAutoUpdate() {
      this.updateInterval = setInterval(() => {
        this.fetchStockData();
      },  5 * 60 * 1000); 
    },
    stopAutoUpdate() {
      clearInterval(this.updateInterval);
    },
  },
};
</script>

<style scoped>
.stock-item {
  display: flex;
  flex-direction: row;
  align-items: center;
  padding: 10px;
  border-radius: 5px;
  background-color: #444;
  margin: 10px;
  width: 80%;
  height: 200px;
  box-sizing: border-box;
}

.stock-name,
.stock-symbol,
.stock-price {
  font-size: 14px;
  margin-bottom: 5px;
}

.stock-chart {
  display: flex;
  flex-grow: 1;
  width: 100%;
  height: 150px;
  background-color: #555;
  border-radius: 5px;
  padding: 10px;
}


.stock-info {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  width: 100%;
  padding: 10px;
}
</style>