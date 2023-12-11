<template>
     <CarouselComponent v-bind:carouselRecords="$store.state.library"></CarouselComponent>
  
</template>

<script>
import LibraryService from '../../services/LibraryService';
import CarouselComponent from '../../components/CarouselComponent.vue';

export default{
  computed:{
    records() {
      return this.$store.library;
    }
  },
  components: {
      CarouselComponent
  },
  methods: { 
      getLibrary() {
          LibraryService.GetLibrary()
          .then( response => {
              this.$store.commit('ADD_RECORDS_TO_LIBRARY', response.data)
          })
      }
  },
  created(){
      this.getLibrary();
  }
}
</script>

<style scoped>
.Library-container{
  display: flex;
justify-content: space-evenly;
flex-wrap: wrap;
/* background-color: black; */

}

</style>