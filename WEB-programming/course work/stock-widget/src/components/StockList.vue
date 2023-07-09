<template>
  <div class="stock-list">
    <swiper :options="swiperOptions"
    :modules="modules"
    :slides-per-view="1"
    :space-between="50"
    navigation
    :pagination="{ clickable: true }"
    :scrollbar="{ draggable: true }"
    @swiper="onSwiper"
    @slideChange="onSlideChange">
      <swiper-slide class="swiper-slide" 
      v-for="(chunk, index) in stockChunks" :key="index">
        <StockItem
          v-for="stock in chunk"
          :key="stock.symbol"
          :symbol="stock.symbol"
          :name="stock.name"
        />
      </swiper-slide>
      <div class="swiper-pagination" slot="pagination"></div>
      <div class="swiper-button-next" slot="button-next"></div>
      <div class="swiper-button-prev" slot="button-prev"></div>
    </swiper>
  </div>
</template>

<script>
import { Navigation, Pagination} from 'swiper';
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue';

// Import Swiper styles
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';


import StockItem from './StockItem.vue';

export default {
  props: {
    stocks: {
      type: Array,
      required: true,
    },
  },
  computed: {
    stockChunks() {
      const chunkSize = 3;
      return this.stocks.reduce((result, stock, index) => {
        if (index % chunkSize === 0) {
          result.push([]);
        }
        result[result.length - 1].push(stock);
        return result;
      }, []);
    },
    swiperOptions() {
      return {
        slidesPerView: 1,
        pagination: {
          el: '.swiper-pagination',
          clickable: true,
        },
        navigation: {
          nextEl: '.swiper-button-next',
          prevEl: '.swiper-button-prev',
        },
      };
    },
  },
  components: {
    Swiper,
    SwiperSlide,
    StockItem,
  },
  // directives: {
  //   swiper: directive,
  // },

  setup() {
      const onSwiper = (swiper) => {
        console.log(swiper);
      };
      const onSlideChange = () => {
        console.log('slide change');
      };
      return {
        onSwiper,
        onSlideChange,
        modules: [Navigation, Pagination],
      };
    },
};
</script>

<style scoped>
.stock-list {
  margin-top: 20px;
  position: relative;
}

.swiper-slide{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin-bottom: 40px;
}
/* .custom-pagination {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  background-color: rgba(255, 255, 255, 0.5);
  border-radius: 5px;
  padding: 5px;
}

.custom-button-next,
.custom-button-prev {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background-color: rgba(255, 255, 255, 0.5);
  padding: 5px;
  border-radius: 5px;
  width: 30px;
  height: 30px;
} */

/* .swiper-pagination {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  background-color: rgba(255, 255, 255, 0.5);
  border-radius: 5px;
  padding: 5px;
} */
.swiper-button-next {
  right: -100px;
}

.swiper-button-prev {
  left: -100px;
}

</style>