<template>
  <Carousel :itemsToShow="3.95" :autoplay="3000" :wrapAround="true" :transition="500">
    <!--  -->
    <Slide v-for="record in carouselRecords" :key="record.id" :carouselCard="record">
      <div class="carousel__item">
        <img :src="record.record.images[0].uri" />
      </div>
    </Slide>
    <template #addons>
      <Navigation />
      <Pagination />
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
  },
  props: {
    carouselRecords: {
      type: Object,
      required: true
    },
    carouselChooser: {
      type: Number,
      required: true
    }
  },
  data() {
    return {
    }
  }
});
 
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
  padding: 20px;
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
</style>