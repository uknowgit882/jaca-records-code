<template>
  <button @click="getCarouselObject">Print to log</button>
  <button @click="displayCard2">Print card to log</button>
  <div v-if="showCard" class="popup">
        <div class="popup-inner">
          <div class="popup-inner-inner">
            <button @click="showCard = !showCard">x</button>
          <h3 >MyCardPopup</h3>
          <ActiveCardComponent class="popup-inner-inner" v-bind:activeCard="this.activeCard" :cardType="carouselChooser"></ActiveCardComponent>
          <!-- <p>Current Slide: {{this.$props.carouselRecords[this.$refs.myCarousel.data.currentSlide.value]}}</p> -->
          <p >Current Slide: {{this.$props.carouselRecords[0].id}}</p>
          <p >Current Slide: {{this.$refs.myCarousel.data.currentSlide}}</p>
          <p >Active Record: {{ this.activeCard }}</p>
        </div>
        </div>
      </div>
  <!-- <Carousel :ref="carousel => {myCarousel}" :itemsToShow="3.95" :autoplay="autoplay ? 3000 : false" :wrapAround="true" :transition="500" > -->
  <Carousel ref="myCarousel" :itemsToShow="3.95" :autoplay="autoplay ? 3000 : false" :wrapAround="true" :transition="500" >
    <Slide v-for="record in carouselRecords" :key="record.id" :carouselSearchCard="record" @click="showCard = !showCard">
      <div v-if="carouselChooser == 'library'" class="carousel__item">
        <img :src="record.record.images[0].uri" />
      </div>
      <div v-else-if="carouselChooser == 'collection'" class="carousel__item">
        <div class="stackOne" >
        <img :src="record.images[0].uri" style="height: 150px; width: 150px" />
      </div>
      </div>
      <div v-else-if="carouselChooser == 'searchAPI'" class="carousel__item">
        <h3 class="carouselSearchCard_title">{{ record.title }}</h3>
        <p class="carouselSearchCard_year">{{ record.year }}</p>
        <img class="carouselSearchCard_image" :src="record.thumb" style="height: 150px; width: 150px" />
        <!-- <carousel-search-card :carouselSearchCard="record"></carousel-search-card > -->
      </div>
      <div v-else-if="carouselChooser == 'searchLibrary'" class="carousel__item">
        <p></p>
        <img :src="record.images[0].uri" style="height: 150px; width: 150px" />
        <p></p>
      </div>
      <div v-else-if="carouselChooser == 'searchCollections'" class="carousel__item">
        <h3>{{ record.name }}</h3>
        <div class="stackOne">
            <img :src="record.records[0].images[0].uri" style="height: 150px; width: 150px" />
        </div>
        <p></p>
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
import { ref, onMounted } from 'vue'
import ActiveCardComponent from './ActiveCardComponent.vue';

import 'vue3-carousel/dist/carousel.css'

const myCarousel = ref(null); 

export default defineComponent({
  name: 'Autoplay',
  components: {
    Carousel,
    Slide,
    Navigation,
    Pagination,
    ActiveCardComponent
    //PopUpComponent
  },
  props: {
    carouselRecords: {
      type: Object,
      required: true
    },
    carouselChooser: {
      type: String,
      required: true
    },
    autoplay: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      showCard: false,
      myCarousel: []
    }
  },
  computed:{
    displayCard(){
      let cardNumber = this.$refs.myCarousel.data.value;
      let records = this.$props.carouselRecords;
      let cardToDisplay = records[cardNumber];
      console.log(cardToDisplay);
      return cardToDisplay;
    },
    activeCard(){
      return this.$props.carouselRecords[this.$refs.myCarousel.data.currentSlide.value];
    }
  },
  methods: {
    chosenCarousel() {
      if (this.carouselChooser == 'library') {

        return true;
      }
      return false;
    },
    getCarouselObject(){
      console.log(this.$refs.myCarousel)
      console.log(this.$props.carouselRecords)
    },
    displayCard2(){
      let cardNumber = this.$refs.myCarousel.data.currentSlide.value;
      let records = this.$props.carouselRecords;
      let cardToDisplay = records[cardNumber];
      console.log(cardToDisplay);
      return cardToDisplay;
    }
  }
});

</script>

<style scoped>
.carouselSearchCard_container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "image year"
    "title title";
}

.carouselSearchCard_image {
  grid-area: image;
}

.carouselSearchCard_year {
  grid-area: year;
}

.carouselSearchCard_title {
  grid-area: title;
}

h3 {
  color: white;
}

p {
  color: white;
}

#Carousel {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 1500px;
  height: 500px;
}

.carousel__pagination {
  color: white
}

.carousel__pagination-button {
  color: white
}

.carousel__pagination-button--active {
  color: white
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
  opacity: 0.5;
  transform: rotateY(-18deg) scale(0.93);
}

.carousel__slide--next {
  opacity: 0.5;
  transform: rotateY(10deg) scale(.95);
}

.carousel__slide--active {
  opacity: 1;
  transform: rotateY(0) scale(1.1);
}

.stackOne {
  border: 6px solid #B452A8;
  float: left;
  height: 150px;
  width: 150px;
  position: relative;
  -webkit-box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3);
  -moz-box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3);
  box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3);
}

.stackOne:before {
  content: "";
  height: 150px;
  width: 150px;
  background: #EA5143;
  border: 6px solid #F5EF17;

  position: absolute;
  z-index: -1;
  top: 0px;
  left: -10px;

  -webkit-box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);
  -moz-box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);
  box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);

  -webkit-transform: rotate(-5deg);
  -moz-transform: rotate(-5deg);
  -o-transform: rotate(-5deg);
  -ms-transform: rotate(-5deg);
  transform: rotate(-5deg);
}

.stackOne:after {
  content: "";
  height: 150px;
  width: 150px;
  background: #05B49C;
  border: 6px solid #069FD6;
  position: absolute;
  z-index: -1;
  top: 5px;
  left: 0px;

  -webkit-box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);
  -moz-box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);
  box-shadow: 2px 2px 5px rgba(255, 255, 255, 0.76);

  -webkit-transform: rotate(4deg);
  -moz-transform: rotate(4deg);
  -o-transform: rotate(4deg);
  -ms-transform: rotate(4deg);
  transform: rotate(4deg);
}

.popup {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.2);
    display: flex;
    align-items: center;
    justify-content: center;
  }
  
  .popup-inner scoped {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    background: black;
    opacity: 0.75;
    padding: 10px;
    text-align: center;
    height: 100%;
    width: 100%; 
  }
  
  .popup-inner-inner {
    background: white;
    opacity: 200;
    z-index: 2;
    padding: 10px;
    text-align: center;
    height: 500px;
    width: 800px; 
  }
  
  .popup-inner button {
    margin-top: 16px;
    padding: 8px 16px;
    background-color: #3498db;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

</style>