<template>
  <div class="home" v-if="!isLoading">
    <img src="../../img/Logogif.gif" alt="">
  </div>
  <div v-else>
    <h1>Home</h1>
    <!-- <button v-on:click="logout">Logout</button> -->
    <CarouselComponent v-bind:carouselRecords="filteredCollections" v-bind:carouselChooser="'searchCollections'" :autoplay="true">
      </CarouselComponent>
  </div>
</template>

<script>

import AuthService from '../services/AuthService';
import CollectionsService from '../services/CollectionsService';
import AnonymousService from '../services/AnonymousService'
import CarouselComponent from '../components/CarouselComponent.vue';

export default {
  data() { return {
    isLoading: false
  }
  },
  computed:{
    filteredCollections(){
      let filteredCollections = this.$store.state.publicCollections.filter( 
        collection => {
          return collection.records.length > 0;
        })
        return filteredCollections;
      }
  },
  components: {
        CarouselComponent
    },
  methods: {
    // logout() {
    //   this.$store.commit('LOGOUT');
    // },
    getPublicCollections() {
        AnonymousService.getPublicCollections()
          .then(response => {
            this.$store.commit('ADD_PUBLIC_COLLECTIONS_TO_LIBRARY', response.data)
            this.isLoading = true;
          })
      } 
  },
  created() {
    this.getPublicCollections();
  }
};
</script>
