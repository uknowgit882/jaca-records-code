<template>
    <div class="home" v-if="!isLoading">
        <img src="../../img/Logogif.gif" alt="">
    </div>

    <div v-else>
        <div>
            <div>
                <div class="collectionPage ">
                    <div style="display: flex; justify-content: center; ">
                        <div style="display: flex; flex-direction: column; ">
                            <div style="display flex; flex-direction: row; align-items: center; flex-grow: 1 ;">
                                <button class="button2" style="margin: 6px" @click="filterAll">All Collections</button>
                                <button class="button2" style="margin: 6px" @click="filterPrivate">Private
                                    Collections</button>
                                <button class="button2" style="margin: 6px" @click="filterPublic">Public
                                    Collections</button>
                            </div>
                            <div style="display: flex; flex-direction: row; justify-content: center;">
                                <button class="button2" @click="showAddCollection = !showAddCollection">Add
                                    Collection</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <AddCollectionComponent :isVisible="showAddCollection" v-if="showAddCollection" @showAddCollectionToParent="i => showAddCollection = i"></AddCollectionComponent>
            
            <CollectionsOptionsComponent :collection="activeCollection" :isVisible="showCollectionOptions" v-if="showCollectionOptions" @showCollectionOptionsToParent="i => showCollectionOptions = i"></CollectionsOptionsComponent>
            
            <div v-for="collection in displayedCollection" v-bind:key="collection.name" class="collectionsPage">
                <div style="display: flex; flex-direction: row; justify-content: space-between; margin: 20px;">
                    <h2
                        style="margin-left: 20px; display: inline; align-items: left; margin-top: auto; margin-bottom: auto ;">
                        {{ collection.name }}</h2>
                    <div style="align-items: right;">
                        <!-- <button class="button2" @click="getVBindKey">Options</button> -->
                        <button class="button2" @click="showCollectionOptions = !showCollectionOptions; activeCollection = collection">Options</button>
                    </div>
                </div>
                <div>

                    <div v-if="collection.records.length == 0">
                        <p style="margin-left: 20px">You have no records in this collection</p>
                    </div>
                    <div v-else>
                        <CarouselComponent v-bind:carouselRecords="collection.records" v-bind:carouselChooser="'collection'"
                            :autoplay="false"></CarouselComponent>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>

import CollectionsService from '../services/CollectionsService';
import CarouselComponent from '../components/CarouselComponent.vue';
import AddCollectionComponent from '../components/PopUpCards/AddCollectionComponent.vue';
import CollectionsOptionsComponent from '../components/PopUpCards/CollectionsOptionsComponent.vue';
import {getCurrentInstance} from 'vue'

export default {
    data() {
        return {
            isLoading: false,
            displayedCollection: [],
            showAddCollection: false,
            showCollectionOptions: false,
            currentKey: '',
            activeCollection: null
        }
    },
    components: {
        CarouselComponent,
        AddCollectionComponent,
        CollectionsOptionsComponent
    },
    methods: {
        filterAll() {
            this.displayedCollection = this.$store.state.collections;
        },
        filterPrivate() {
            this.displayedCollection = this.$store.state.collections.filter(collection => {
                return collection.is_Private == true;
            });
        },
        filterPublic() {
            this.displayedCollection = this.$store.state.collections.filter(collection => {
                return collection.is_Private == false;
            });
        },
        getCollections() {
            CollectionsService.GetAllCollections()
                .then(response => {
                    this.$store.commit('ADD_COLLECTIONS_TO_LIBRARY', response.data)
                    this.displayedCollection = this.$store.state.collections;
                    this.isLoading = true;
                })
        },
    },
    created() {
        this.getCollections();
    }
};
</script>

<style> h2 {
     font-size: 1.5rem;
     font-weight: bolder;
     color: white;
     text-align: left;
 }

 p {
     font-weight: bolder;
     color: white;
     text-align: left;
 }

 .collectionsPage {
     display: flex;
     flex-direction: column;
     margin-bottom: 10px;
 }

 .collectionsHeaderFixed {
     position: fixed;
 }

 .collectionButton {
     display: inline;
 }

 .button2 {
     --color: white;
     font-family: inherit;
     display: inline-block;
     width: 15.3em;
     height: 2.6em;
     line-height: 1.5em;
     margin: 20px;
     position: relative;
     overflow: hidden;
     border: 2px solid var(--color);
     transition: color 0.5s;
     z-index: 1;
     font-size: 17px;
     border-radius: 8px;
     font-weight: 500;
     color: var(--color);
     background-color: black;
 }

 .button2:before {
     content: "";
     position: absolute;
     z-index: -1;
     background: var(--color);
     height: 115px;
     width: 500px;
     border-radius: 50%;
 }

 .button2:hover {
     color: black;
 }

 .button2:before {
     top: 100%;
     left: 100%;
     transition: all 0.7s;
 }

 .button2:hover:before {
     top: -30px;
     left: -30px;
     background-color: #d1d301;
 }

 .button2:active:before {
     background: #4d437f;
     transition: background 0s;
 }</style>