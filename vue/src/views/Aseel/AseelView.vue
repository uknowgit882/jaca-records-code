<template>
    <CarouselComponent v-bind:carouselRecords="$store.state.library" v-bind:carouselChooser="'library'"></CarouselComponent>

    <div class="carouselSearchCard_container">
        <p class="carouselSearchCard_image">Img</p>
        <p class="carouselSearchCard_year">Year</p>
        <p class="carouselSearchCard_title">Title and Artist</p>
    </div>
</template>

<script>
import LibraryService from '../../services/LibraryService';
import CarouselComponent from '../../components/CarouselComponent.vue';

export default {
    computed: {
    },
    components: {
        CarouselComponent
    },
    methods: {
        getLibrary() {
            LibraryService.GetLibrary()
                .then(response => {
                    this.$store.commit('ADD_RECORDS_TO_LIBRARY', response.data)
                })
        }
    },
    created() {
        this.getLibrary();
    }
}
</script>

<style scoped>
.Library-container {
    display: flex;
    justify-content: space-evenly;
    flex-wrap: wrap;
    /* background-color: black; */

}

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
</style>