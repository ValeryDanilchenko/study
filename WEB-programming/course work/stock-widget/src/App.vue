<template>
  <div id="app">
    <StockWidget />
    <!-- <StockChart/> -->
  </div>
</template>

<script>
import StockWidget from './components/StockWidget.vue';
// import StockChart from './components/StockChart.vue';

export default {
  name: 'App',
  components: {
    // StockChart
    StockWidget
  },
};
</script>