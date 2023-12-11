<template>
  <Carousel :itemsToShow="3.95" :autoplay="3000" :wrapAround="true" :transition="500">
    <Slide v-for="record in carouselRecords" :key="record.id" :carouselSearchCard="record">
      <div v-if="carouselChooser == 'library'" class="carousel__item">
        <img :src="record.record.images[0].uri" />
      </div>
      <div v-else-if="carouselChooser == 'collection'" class="carousel__item">
      </div>
      <div v-else-if="carouselChooser == 'searchAPI'" class="carousel__item">
        <img class="carouselSearchCard_image" :src="record.thumb" />
        <h2 class="carouselSearchCard_year">Year: {{ record.year }}</h2>
        <h3 class="carouselSearchCard_title">{{ record.title }}</h3>
        <!-- <carousel-search-card :carouselSearchCard="record"></carousel-search-card > -->
      </div>
      <div v-else-if="carouselChooser == 'searchLibrary'" class="carousel__item">
        <img :src="record.images[0].uri" />
      </div>
      <div v-else-if="carouselChooser == 'searchCollections'" class="carousel__item">
        <h2>{{ record.name }}</h2>
        <img :src="record.records[0].images[0].uri" />
      </div>
    </Slide>
    <template #addons>
      <Pagination />
      <Navigation />
    </template>
  </Carousel>
</template>


<script>
import { defineComponent } from 'vue'
import { Carousel, Navigation, Pagination, Slide } from 'vue3-carousel'

import 'vue3-carousel/dist/carousel.css'

export default defineComponent({
  name: 'Autoplay',
  components: {
    Carousel,
    Slide,
    Navigation,
    Pagination,
    //CarouselSearchCard
  },
  props: {
    carouselRecords: {
      type: Object,
      required: true
    },
    carouselChooser: {
      type: String,
      required: true
    }
  },
  data() {
    return {
    }
  },
  methods: {
    chosenCarousel() {
      if (this.carouselChooser == 'library') {

        return true;
      }
      return false;
    }
  }
});

</script>

<style scoped>

.carouselSearchCard_container{
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-areas: 
  "image year"
  "title title";
}
.carouselSearchCard_image{
  grid-area: image;
}
.carouselSearchCard_year{
  grid-area: year;
}
.carouselSearchCard_title{
  grid-area: title;
}

#Carousel {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 1500px;
  height: 500px;
}

.carousel__slide {
  display: flex;
  padding: 20px;
}

.carousel__viewport {
  perspective: 2000px;
}

.carousel__track {
  transform-style: preserve-3d;
}

.carousel__item {}

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
</style>