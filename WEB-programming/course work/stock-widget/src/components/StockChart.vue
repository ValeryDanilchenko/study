<!-- <template>
  <div>
    <Bar :chart-data="chartData" />
  </div>
</template>

<script>
// import LineChart from './LineChart.js'
import { Bar } from 'vue-chartjs'
import { Chart, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js'

Chart.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

export default {
  // name: 'BarChart',
  components: { Bar },
  data() {
    return {
      chartData: {
        labels: [ 'January', 'February', 'March'],
        datasets: [
          {
            label: 'Data One',
            backgroundColor: '#f87979',
            data: [40, 20, 12]
          }
        ]
      }
    }
  }
}
</script>
 -->


 <template>
  <div class="canvas">
    <!-- <canvas id="stockChart" ref="canvas"></canvas> -->
    <!-- <Line :data="chartData"/> -->
    <Line :data="chartData" :options="chartOptions" />
  </div>
</template>

<script>
import StockDataService from '../services/StockDataService';
import { Line } from "vue-chartjs";
import axios from "axios";

import { Chart as ChartJS, Title, Tooltip, Legend, CategoryScale, LinearScale, LineController, LineElement, PointElement} from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, CategoryScale, LinearScale, LineController, LineElement, PointElement)

export default {
  name: 'LineChart',
  components: { Line },
  props: {
      symbol: {
        type: String,
        required: true,
      },
  },
  // computed:{
  //   chartData(){},
  //   chartOptions(){}
  // },
  data() {
    return {
      chartData: {
        labels: [],
        datasets: [{
          label: `Stock Price ${this.symbol}`,
          backgroundColor: "rgba(154, 205, 50, 0.2)",
          borderColor: "rgba(154, 205, 50, 1)",
          pointRadius: 0,
          data: [],
          fill: false,
          tension: 0.1
        }]
      },
      chartOptions:{
        responsive: true,
        maintainAspectRatio: false,
        scales: {},
      },

    }
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
      const apiKey = "TB46Y2SQFVGPS4U7";
      const url = `https://www.alphavantage.co/query?function=TIME\_SERIES\_INTRADAY&symbol=${this.symbol}&interval=60min&apikey=${apiKey}`;
      console.log('URL ', url);

      let timeSeries = null;

      while (!timeSeries) {
        try {
          const response = await axios.get(url);
          timeSeries = response.data["Time Series (60min)"];

          if (!timeSeries) {
            await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
          }
        } catch (error) {
          console.error("Error fetching stock data:", error);
          await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
        }
      }

      const chartData = {
        labels: [],
        datasets: [
          {
            label: `Stock Price ${this.symbol}`,
            backgroundColor: "rgba(154, 205, 50, 0.2)",
            borderColor: "rgba(154, 205, 50, 1)",
            pointRadius: 0,
            data: [],
            fill: false,
            tension: 0.1
          },
        ],
      };

      console.log('timeSeries ', timeSeries);
      for (const time in timeSeries) {
        chartData.labels.push(time.split(" ")[1].split(":")[0]);
        chartData.datasets[0].data.push(parseFloat(timeSeries[time]["4. close"]));
      }
      this.chartData = chartData;
    },

    // async fetchStockData() {
    //   const stockData = await StockDataService.getStockData(this.symbol);
    //   this.chartData.labels = stockData.dates;
    //   this.chartData.datasets[0].data = stockData.prices;
    //   console.log(this.chartData);
    // },

    startAutoUpdate() {
      this.updateInterval = setInterval(() => {
        this.fetchStockData();
      },   60 * 1000); 
    },
    stopAutoUpdate() {
      clearInterval(this.updateInterval);
    },
  },
};
</script>

<style scoped>
.canvas {
  max-width: 1000px;
  max-height: 500px;
}
</style>
