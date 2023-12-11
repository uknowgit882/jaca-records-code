<template>
  <div class="display" v-if="$store.state.searchResults.data">
    <!-- <div class="box" id="Carousel" v-for="item in $store.state.searchResults.data.results" :key="item.id">Carosuel 
            <div>{{ item.title }}</div>
            <img class="result-thumbnail" v-bind:src="item.thumb" />
        </div> -->
  </div>
  <Carousel :itemsToShow="3.95" :autoplay="3000" :wrapAround="true" :transition="500">
    <Slide v-for="n in 50" :key="n">
      <div class="carousel__item">
        <!-- <v-img :src="this.image[n-1]" height="50px"></v-img> -->
        <img
          src="https://i.discogs.com/xAuuOx6MzpbgVhBF2BD-XEBhJsK1bDfgCbGYlsbTeEE/rs:fit/g:sm/q:90/h:600/w:600/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTI0NjI1/MzI1LTE2NjQxMDIz/NDgtMjQzNy5qcGVn.jpeg" />
      </div>
    </Slide>
    <template #addons>
      <Navigation />
      <Pagination />
    </template>
  </Carousel>
</template>

<script>
import SearchResultComponentVue from "./SearchResultComponent.vue";
import { defineComponent } from 'vue'
import { Carousel, Navigation, Pagination, Slide } from 'vue3-carousel'

import 'vue3-carousel/dist/carousel.css'

export default {
  name: 'Autoplay',
  components: {
    Carousel,
    Slide,
    Navigation,
    Pagination,
    //SearchResultComponentVue
  },
  data() {
    return {
      image: [
        "https://assets.justinmind.com/wp-content/uploads/2018/05/top-10-worst-90s-website-designs-header.png",
      ]
    }
  }
}
</script>

<style scoped>
#Carousel {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 1500px;
  height: 500px;

}

.carousel__slide {
  display: flex;
  padding: 80px;
  padding-top: 33px;
  margin-right: 0.2px;
}

.carousel__viewport {
  perspective: 2000px;
}

.carousel__track {
  transform-style: preserve-3d;
}

.carousel__slide--sliding {
  transition: 0.5s;
}

.carousel__slide {
  opacity: -1;
  transform: rotateY(-20deg) scale(0.9);
}

.carousel__slide--active~.carousel__slide {
  transform: rotateY(20deg) scale(0.94);
}

.carousel__slide--prev {
  opacity: 1;
  transform: rotateY(-18deg) scale(0.93);
}

.carousel__slide--next {
  opacity: 1;
  transform: rotateY(10deg) scale(.95);
}

.carousel__slide--active {
  opacity: 1;
  transform: rotateY(0) scale(1.1);
}

.carousel_pagination {
  display: flex;
  columns: center;
}
</style>