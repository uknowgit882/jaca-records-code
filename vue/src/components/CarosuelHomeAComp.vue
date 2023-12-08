<template>
    <div class="display" v-if="$store.state.searchResults.data">
        <!-- <div class="box" id="Carousel" v-for="item in $store.state.searchResults.data.results" :key="item.id">Carosuel 
            <div>{{ item.title }}</div>
            <img class="result-thumbnail" v-bind:src="item.thumb" />
        </div> -->
    </div>
    <Carousel :itemsToShow="3.95" :wrapAround="true" :transition="500">
      <Slide v-for="n in 50" :key="n">
        <div class="carousel__item">
            <!-- <v-img :src="this.image[n-1]" height="50px"></v-img> -->
            <img src="https://www.lpl.com/content/dam/lpl-www/images/newsroom/read/insider/insider-blog-meme-stocks-what-do-they-mean_article-hero-450x450.png"/>
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

export default{
    name: 'Autoplay',
  components: {
    Carousel,
    Slide,
    Navigation,
    Pagination,
    SearchResultComponentVue
  },
  data() {
    return {
        image: [
            "https://www.theinterrobang.ca/images/interrobang/030819/B8QC6DAZ9PWRK7M2.jpg",
            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSUEIbxGhXurYC9gDshTflQ0DpnZyNqzWDvTiuUchMApUMdj-f4kPid9aW6-E2qrVshy-A&usqp=CAU",
            "https://www.lifewire.com/thmb/PiAB30sXKFcKSO_r72hP5-Hwq_E=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/BabywithFistmeme2-0c317ccc4d3a44c68098cd08f4ecac74.jpg"
        ]
    }
  }
}
</script>

<style scoped>
#Carousel{
    display: flex;
  align-items: center;
  justify-content: center;
    width: 1500px;
    height: 500px;

}
.carousel__slide {
  padding: 5px;
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
  opacity: 0.9;
  transform: rotateY(-20deg) scale(0.9);
}

.carousel__slide--active ~ .carousel__slide {
  transform: rotateY(20deg) scale(0.9);
}

.carousel__slide--prev {
  opacity: 1;
  transform: rotateY(-10deg) scale(0.95);
}

.carousel__slide--next {
  opacity: 1;
  transform: rotateY(10deg) scale(0.95);
}

.carousel__slide--active {
  opacity: 1;
  transform: rotateY(0) scale(1.1);
}
</style>