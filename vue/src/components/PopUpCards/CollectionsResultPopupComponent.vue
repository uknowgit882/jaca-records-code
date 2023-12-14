<template>
    <div v-if="errorPopup">
        <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>
    <div v-if="showCollectionOptions">
        <CollectionsOptionsComponent :collection="activeCollection" :isVisible="showCollectionOptions" v-if="showCollectionOptions" @showCollectionOptionsToParent="i => showCollectionOptions = i"></CollectionsOptionsComponent>
    </div>
    <div>
        <div class="scrollableBox"
            style="margin: 20px; height: 550px; width: 750px; display: flex; flex-direction: column; justify-items: center;">
            <div>
                <h2 style="text-align: center;">{{ collection.name }}</h2>
                <button class="button2" @click="showCollectionOptions = !showCollectionOptions; activeCollection = collection" v-if="this.$store.state.token != ''">Collection Options</button>
            </div>
            <div class="CollectionsResults-table">
                <!-- <span class="CollectionsResults-image" style="text-align: left; color: white;">Image</span>
                <span class="CollectionsResults-artist" style="text-align: left; color: white; padding: 8px;">Artist & Title</span>
                <span class="CollectionsResults-country" style="text-align: left; color: white; padding: 8px;"></span> -->
                <table class="CollectionsResults-trackRow">
                    <tr>
                        <collections-row-component v-for="record in collection.records" :key="record.id"
                            :record="record"></collections-row-component>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</template>

<script>


import ErrorPopup from './ErrorPopup.vue'
import CollectionsRowComponent from './CollectionsRowComponent.vue';
import CollectionsOptionsComponent from './CollectionsOptionsComponent.vue';


export default {
    props: {
        collection: {
            type: Object,
            required: true
        },
        
    },
    components: {
        ErrorPopup,
        CollectionsRowComponent,
        CollectionsOptionsComponent
    },
    data() {
        return {
            errorPopup: false,
            errorMessage: "",
            showCollectionOptions: false,
            activeCollection: null
        }
    },

    methods: {

    },
}

</script>

<style> .CollectionsResults-table {
     display: grid;
     grid-template-columns: 16% 16% 16% 16% 16%;
     grid-template-areas:
         " trackRow trackRow trackRow trackRow trackRow";
     /* "button button button"; */
     gap: 5px;
     font-weight: bold;
 }

 .CollectionsResults-image { 
     grid-area: imageHeader;
     padding: 5px;
 }

 .CollectionsResults-artist { 
     grid-area: artistHeader;
     text-align: center;
 }


 .CollectionsResults-country { 
     grid-area: yearCountryHeader;
 }

 .CollectionsResults-trackRow {
     grid-area: trackRow;
 }
/* 
 .CollectionsResults-button {
     grid-area: button;
     margin-top: 8px;
     padding: 8px;
     background-color: #09A3DA; 
     color: #fff;
     text-align: center;
     border-radius: 4px;
     cursor: pointer;
 } */

 .scrollableBox {
     overflow-y: scroll;
     overflow-x:hidden;
 }

 /* Works on Chrome, Edge, and Safari */
 .scrollableBox::-webkit-scrollbar {
     width: 8px;
 }

 .scrollableBox::-webkit-scrollbar-track {
     background: white;
 }

 .scrollableBox::-webkit-scrollbar-thumb {
     background-color: #F89590;
     border-radius: 20px;
     border: 3px solid white;
 }</style>