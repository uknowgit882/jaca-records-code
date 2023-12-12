<template>
    
    <div class="home" v-if="!isLoading">
        <img src="../../img/Logogif.gif" alt="">
    </div>
    <div v-else>
        <div v-for="collection in $store.state.collections" v-bind:key="collection.name">
            <h2>{{ collection.name }}</h2>
            <button>Change Collection Name</button>
            <button>Delete</button>
            <div v-if="collection.records.length == 0">
                <p>You have no records in this collection</p>
            </div>
            <div v-else>
                <CarouselComponent v-bind:carouselRecords="collection.records" v-bind:carouselChooser="'collection'"
                    :autoplay="false"></CarouselComponent>
            </div>
        </div>
    </div>
</template>
  
<script>

import CollectionsService from '../services/CollectionsService';
import CarouselComponent from '../components/CarouselComponent.vue';

export default {
    data() {
        return {

            isLoading: false
        }
    },
    components: {
        CarouselComponent,
    },
    methods: {

        getCollections() {
            CollectionsService.GetAllCollections()
                .then(response => {
                    this.$store.commit('ADD_COLLECTIONS_TO_LIBRARY', response.data)
                    this.isLoading = true;
                })
        }
    },
    created() {
        this.getCollections();
    }
};
</script>

<style>   h2 {
       font-size: 1.5rem;
       font-weight: bolder;
       color: white;
       text-align: left;
       padding-left: 20px;
   }

   p {
       font-weight: bolder;
       color: white;
       text-align: left;
       padding-left: 20px;
   }
</style>