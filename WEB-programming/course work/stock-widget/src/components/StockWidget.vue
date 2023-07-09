<template>
  <div class="stock-widget">
    <SearchBar @search="onSearch" />
    <StockList :stocks="filteredStocks" />
  </div>
</template>

<script>
import SearchBar from './SearchBar.vue';
import StockList from './StockList.vue';

export default {
  data() {
    return {
      stocks: [],
      searchQuery: '',
    };
  },
  computed: {
    filteredStocks() {
      if (!this.searchQuery) {
        return this.stocks;
      }

      return this.stocks.filter(
        (stock) =>
          stock.symbol.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          stock.name.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    },
  },
  async mounted() {
    const response = await fetch('data/stocks.json');
    this.stocks = await response.json();
  },
  methods: {
    onSearch(searchQuery) {
      this.searchQuery = searchQuery;
    },
  },
  components: {
    SearchBar,
    StockList,
  },
};
</script>

<style scoped>
.stock-widget {
  background-color: #333;
  color: #fff;
  padding: 20px;
  border-radius: 10px;
  max-width: 1000px;
  margin: 0 auto;
}
</style>
