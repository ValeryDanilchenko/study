<template>
    <div class="search-bar">
      <input
        type="text"
        v-model="searchQuery"
        @input="onSearchInput"
        placeholder="Search by symbol or name"
      />
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        searchQuery: '',
      };
    },
    methods: {
      onSearchInput() {
        this.$emit('search', this.searchQuery);
      },
    },
  };
  </script>
  
  <style scoped>
  .search-bar {
    margin-bottom: 10px;
  }
  
  input {
    width: 100%;
    padding: 5px;
    font-size: 14px;
  }
  </style>
  