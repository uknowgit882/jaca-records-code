<template>
  <div class="home" v-if="isLoading">
    <img src="../../img/Logogif.gif" alt="">
  </div>
  <div v-else>
    <h1>Home</h1>
    <!-- <button v-on:click="logout">Logout</button> -->
    <CarouselComponent v-bind:carouselRecords="$store.state.library" v-bind:carouselChooser="'library'" :autoplay="true">
    </CarouselComponent>
  </div>
</template>

<script>

import AuthService from '../services/AuthService';
import LibraryService from '../services/LibraryService';
import CarouselComponent from '../components/CarouselComponent.vue';

export default {
  data() {
    return {
      Search: {
        General: "",
        Artist: "queen",
        Title: "",
        Genre: "",
        Year: "",
        Country: "",
        Label: ""

      },
      isLoading: true
    }
  },
  components: {
    CarouselComponent
  },
  methods: {
    searchRecord() {
      AuthService.search(this.Search)
        .then(response => {
          this.$store.commit('ADD_SEARCH_RESULT', response);
          this.isVisible = true;
        })
        .catch(error => {

        })
    },
    getLibrary() {
      LibraryService.GetLibrary()
        .then(response => {
          this.$store.commit('ADD_RECORDS_TO_LIBRARY', response.data)
          this.isLoading = false;
        })
    },
    logout() {
      this.$store.commit('LOGOUT');
    }
  },
  created() {
    this.getLibrary();
  }
};
</script>
