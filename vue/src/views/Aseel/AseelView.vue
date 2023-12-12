<template>
 
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