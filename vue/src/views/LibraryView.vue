<template>
    <div class="home" v-if="!isLoading" style="display: flex; justify-content: center;">
      <img src="../../img/Logogif.gif" alt="">
    </div>
    <div v-else>
      <div v-if="hasCollectionResults">
      <CarouselComponent v-bind:carouselRecords="$store.state.library" v-bind:carouselChooser="'library'" :autoplay="true" >
      </CarouselComponent>
    </div>
    <div v-else>
          <!-- remove the v-else if you want this to display -->
          <br />
          <p>You don't have any records in your library</p>
          <br />
        </div>
    </div>
  </template>
  
  <script>
  
  import LibraryService from '../services/LibraryService';
  import CarouselComponent from '../components/CarouselComponent.vue';
  
  export default {
    data() {
      return {
        isLoading: false
      }
    },
    components: {
      CarouselComponent
    }, 
    methods: {
      getLibrary() {
        LibraryService.GetLibrary()
          .then(response => {
            this.$store.commit('ADD_RECORDS_TO_LIBRARY', response.data) 
            this.isLoading = true;
          })
      } 
    },
    created() {
      this.getLibrary();
    }
  };
  </script>

  <style>

</style>
  