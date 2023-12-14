<template>
    <div v-if="errorPopup">
        <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>

    <div v-if="showRecordOptionsPopup">
        <LibrarySearchResultPopupComponent :activeCard="record" :isVisibleLibrary="showRecordOptionsPopup" v-if="showRecordOptionsPopup" @showCollectionOptionsToParent="i => showRecordOptionsPopup = i"></LibrarySearchResultPopupComponent>

    </div>

    <div style="display: flex; flex-direction: column; align-items: center;">
        <div class="CollectionsResults-row-table">
            <div style="display: flex; flex-direction: column; justify-content: center; align-items: center;" v-if="this.$store.state.token != ''">
                <!-- <button class="CollectionsResults-row-button" @click="showRecordOptionsPopup = !showRecordOptionsPopup">Show
                    Options</button> -->
                    <div class="CollectionsResults-row-button">
                        <button class="button2" style="margin: 0; width: 200px;" @click="showRecordOptionsPopup = !showRecordOptionsPopup;">{{ showRecordOptionsPopup ? 'Hide Card' : 'Show Card'}}</button>
                    </div>
            </div>
            
            <div class="CollectionsResults-row-image" v-show="!showRecordOptionsPopup">
                <Carousel>
                    <Slide v-for="image in record.images" :key="image.uri">
                        <div class="carousel__item">
                            <img :src="image.uri" alt="">
                        </div>
                    </Slide>

                    <template #addons>
                        <Pagination />
                    </template>
                </Carousel>
            </div>

            <div style="display: flex; flex-direction: column; justify-content: center; align-items: center;" v-show="!showRecordOptionsPopup">
                <div class="CollectionsResults-row-artist" style="text-align: left; padding: 8px;">
                    <h2 v-for="artist in record.artists" :key="artist.name">{{ artist.name }}</h2>
                    <h3 style="color: white;">{{ record.title }}</h3>
                    <p style="color: white; font-weight:normal">Year: {{ record.released.substring(0, 4) }}, Country: {{ record.country }}</p>
                </div>
            </div>
            
        </div>
    </div>
</template>

<script>
import ErrorPopup from './ErrorPopup.vue';
import { Carousel, Navigation, Pagination, Slide } from 'vue3-carousel'
import LibrarySearchResultPopupComponent from './LibrarySearchResultPopupComponent.vue';

export default {
    data() {
        return {
            errorPopup: false,
            errorMessage: "",
            canSee: this.isVisible,
            showRecordOptionsPopup: false,
            showCollectionOptions: false
        }
    },
    components: {
        ErrorPopup,
        Carousel,
        Slide,
        Pagination,
        LibrarySearchResultPopupComponent
    },
    props: {
        isVisible: {
            type: Boolean,
            required: true
        },
        record: {
            type: Object,
            required: true
        }

    },
    methods: {
        showCollectionOptionsToParent(response) {
            this.canSee = false;
            this.$emit('showCollectionOptionsToParent', false)
        },
    }

}
</script>


<style>
.CollectionsResults-row-table {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    grid-template-areas:
        " button image artist  ";
    /* "button button button"; */
    gap: 5px;
    width: 750px;
    padding: 12px;
    font-weight: bold;
}

.CollectionsResults-row-image {
    grid-area: image;
    padding: 5px;
    max-width: 200px;
}

.CollectionsResults-row-artist {
    grid-area: artist;
    justify-content: center;
    ;
    text-align: center;
}


.CollectionsResults-row-country {
    grid-area: yearCountry;
}

.CollectionsResults-row-button {
    grid-area: button;
    height: 35px;
    background-color: #F89590;
    color: #fff;
    text-align: center;
    text-justify: middle;
    border-radius: 4px;
    cursor: pointer;
}

.carousel__item {
    min-height: 200px;
    width: 100%;
    color: var(--vc-clr-white);
    font-size: 20px;
    border-radius: 8px;
}

.carousel__slide {
    /* padding: 10px; */
}

.carousel__prev,
.carousel__next {
    box-sizing: content-box;
    border: 5px solid white;
}


.scrollableBox {
    overflow-y: scroll;
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
}

.CollectionsResults-popup {
    border-radius: 5px;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.75);
    display: flex;
    align-items: center;
    justify-content: center;
    word-wrap: break-word;
}

.CollectionsResults-popup-inner scoped {
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    background: black;
    opacity: 1.00;
    padding: 10px;
    text-align: center;
    height: 100%;
    width: 100%;
}

.CollectionsResults-popup-inner-inner {
    border-radius: 5px;
    background: black;
    text-align: center;
    height: 800px;
    width: 800px;
    display: flex;
    align-items: top;
    justify-content: stretch;
    margin: 12px;
}

.CollectionsResults-bigBox {
    height: 220px;
    width: 530px;
}

.CollectionsResults-buttons {
    display: flex;
    flex-direction: column;
    justify-content: stretch;
}

.CollectionsResults-popup-inner-inner-buttons {
    background-color: #09A3DA;
    width: 150px;
}

.CollectionsResults-popup-inner button {
    border-radius: 5px;
    margin-top: 16px;
    padding: 8px 12px;
    background-color: #EA5143;
    color: #fff;
    border: none;
    text-align: center;
    border-radius: 4px;
    cursor: pointer;
    float: right;
}

.CollectionsResults-popup-inner-inputs {
    border-radius: 5px;
    box-sizing: border-box;
    width: 100%;
    background-color: black;
    color: white;
}

.makeVisible {
     display: block;
 }

 .notVisible {
     display: none;
 }
</style>