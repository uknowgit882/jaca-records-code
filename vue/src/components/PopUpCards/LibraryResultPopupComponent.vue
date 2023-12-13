<template>
    <div v-if="showAddRecordPopup" class="search-popup-addRecord-popup">
        <div class="search-popup-addRecord-popup-inner">
            <button class="popup-exit-button" @click.prevent="showAddRecordPopup = !showAddRecordPopup">X</button>
            <div class="search-popup-addRecord-popup-inner-inner">

            </div>
        </div>
    </div>

    <div v-if="errorPopup">
        <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>

    <div class="home" v-if="!isLoading">
        <img src="../../img/Logogif.gif" alt="">
    </div>
    <div v-else>
        <div class="library-popup-container">
            <div class="library-popup-image">
                <Carousel>
                    <Slide v-for="image in activeCard.record.images" :key="image.uri">
                        <div class="carousel__item">
                            <img :src="image.uri" alt="">
                        </div>
                    </Slide>

                    <template #addons>
                        <Pagination />
                    </template>
                </Carousel>
            </div>

            <div class="library-popup-artist" style="text-align: left; padding: 8px;">
                <h2 v-for="artist in activeCard.record.artists" :key="artist.name">{{ artist.name }}</h2>
                <br>
                <span style="color: white; font-weight: bold;">Title: </span>
                <h3>{{ activeCard.record.title }}</h3>
            </div>


            <div class="library-popup-yearCountry" style="text-align: left; padding: 8px;">
                <span style="color: white; font-weight: bold;">Year: </span>
                <br>
                <span>{{ activeCard.record.released.substring(0, 4) }}</span>
                <br>
                <span style="color: white; font-weight: bold;">Country: </span>
                <br>
                <span>{{ activeCard.record.country }}</span>
            </div>

            <div class="library-popup-extra-artist" style="text-align: left; padding: 8px;">
                <span style="color: white; font-weight: bold;">Extra Artists: </span>
                <div class="ticker-tape-container">
                    <div class="ticker-tape">
                        <span v-for="extraArtist in activeCard.record.extraArtists" :key="extraArtist.name">{{
                            extraArtist.name }}</span>
                    </div>
                    <div class="ticker-tape" aria-hidden="true">
                        <span v-for="extraArtist in activeCard.record.extraArtists" :key="extraArtist.name">{{
                            extraArtist.name }}</span>
                    </div>
                </div>
                <span style="font-weight: bold; color: white;">Genre:</span>
                <div class="ticker-tape-container">
                    <div class="ticker-tape">
                        <span v-for="genre in activeCard.record.genres" :key="genre">{{ genre }}</span>
                    </div>
                    <div class="ticker-tape" aria-hidden="true">
                        <span v-for="genre in activeCard.record.genres" :key="genre">{{ genre }}</span>
                    </div>
                </div>
            </div>

            <div class="library-popup-quantity">
                <span style="color: white; font-weight: bold;">Quantity: </span>
                <span style="text-align: right;">{{ activeCard.quantity }}</span>
            </div>

            <div class="library-popup-notes" style="text-align: left; padding: 8px;">
                <span style="color: white; font-weight: bold;">Notes: </span>
                <span>{{ activeCard.notes }}</span>
            </div>

            <div class="library-popup-table">
            </div>

        </div>
        <div v-if="isInLibrary">
            <p class="library-popup-alreadyOwn">You already have this in your library</p>
        </div>
        <button v-else class="library-popup-button" @click="showAddRecordPopup = !showAddRecordPopup">Add To
            Library</button>
    </div>
</template>

<script>
import ErrorPopup from './ErrorPopup.vue'
import LibraryService from '../../services/LibraryService'
import { defineComponent } from 'vue'
import { Carousel, Navigation, Pagination, Slide } from 'vue3-carousel'

export default {
    props: ['activeCard'],
    components: {
        ErrorPopup,
        Carousel,
        Slide,
        Pagination,
    },
    data() {
        return {
            isInLibrary: false,
            isLoading: false,
            showAddRecordPopup: false,
            errorPopup: false,
            addRecord: {
                discogsId: this.activeCard.id,
                notes: "",
                quantity: 1
            },
            errorMessage: ""
        }
    },
    methods: {
        getRecordInLibrary() {
            LibraryService.GetRecordInLibrary(this.activeCard.id)
                .then(response => {
                    this.isInLibrary = true;
                    this.isLoading = true;
                })
                .catch(error => {
                    this.isInLibrary = false;
                    this.isLoading = true;
                })
        },
        addRecordToLibrary() {
            this.isLoading = false;
            this.showAddRecordPopup = false;

            LibraryService.AddToLibrary(this.addRecord)
                .then(response => {
                    this.$store.commit('PUSH_RECORDS_TO_LIBRARY', response.data)
                    this.isInLibrary = true;
                    this.isLoading = true;
                })
                .catch(error => {
                    this.isLoading = true;
                    this.errorMessage = "add this record to the library";
                    this.errorPopup = true;
                })
        }
    },
    created() {
        this.getRecordInLibrary();
    }


}

</script>

<style> .library-popup-container {
     display: grid;
     grid-template-columns: 16% 16% 16% 16% 16% 16%;
     grid-template-areas:
         "image image image artist artist artist"
         "image image image artist artist artist"
         "image image image extraArtist extraArtist yearCountry"
         "quantity blank table table table table"
         "notes notes table table table table"
         "notes notes table table table table";
     /* "button button button"; */
     gap: 5px;
     font-weight: bold;
 }

 .library-popup-image {
     grid-area: image;
     margin: auto;
     padding: 5px;
     max-width: 200px;
 }

 .library-popup-artist {
     margin-top: 12px;
     grid-area: artist;
     text-align: center;
 }

 .library-popup-year-country {
     grid-area: yearCountry;
     width: 100px;
 }

 .library-popup-extra-artist {
     grid-area: extraArtist;
     max-width: 300px;
 }

 .library-popup-genre {
     grid-area: genre;
     max-width: 300px;
 }

 .library-popup-quantity {
     grid-area: quantity;
     max-width: 100px;
 }


 .library-popup-notes {
     grid-area: notes;
     max-width: 200px;
 }

 .library-popup-table {
     grid-area: table;
 }

 .library-popup-button {
     grid-area: button;
     margin-top: 8px;
     padding: 8px;
     background-color: #EA5143;
     min-width: 100%;
     color: #fff;
     text-align: center;
     border-radius: 4px;
     cursor: pointer;
 }

 .library-popup-alreadyOwn {
     grid-area: button;
     margin-top: 8px;
     padding: 8px;
     background-color: #03B8A0;
     min-width: 100%;
     color: #fff;
     text-align: center;
     border-radius: 4px;
     cursor: pointer;
 }

 .ticker-tape-container {
     overflow-x: hidden;
     width: 100%;
     display: flex;
 }

 .ticker-tape {
     display: flex;
     align-items: center;
     flex: 0 0 auto;
     gap: 1rem;
     margin-right: 1rem;
     min-width: 100%;
     word-wrap: normal;
     animation-name: marquee;
     animation-duration: 60s;
     animation-timing-function: linear;
     animation-delay: 0s;
     animation-iteration-count: infinite;
     animation-play-state: running;
     animation-direction: normal;
 }

 @keyframes marquee {
     0% {
         transform: translateX(0%);
     }

     100% {
         transform: translateX(-100%);
     }
 }

 .library-popup-addRecord-popup {
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

 .library-popup-addRecord-popup-inner scoped {
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

 .library-popup-addRecord-popup-inner-inner {
     border-radius: 5px;
     background: black;
     text-align: center;
     height: 250px;
     width: 300px;
 }

 .library-popup-addRecord-popup-inner button {
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

 .library-popup-addRecord-popup-inner-inputs {
     border-radius: 5px;
     box-sizing: border-box;
     width: 100%;
     background-color: black;
     color: white;
 }

 input[type=textarea] {
     height: 50px;
     line-height: 25px;
     font-size: 12px;
     padding: 0 8px;
     margin-top: 6px;
     margin-bottom: 6px;
     background-color: black;
     color: white;
 }

 .carousel__item {
     min-height: 200px;
     width: 100%;
     background-color: var(--vc-clr-primary);
     color: var(--vc-clr-white);
     font-size: 20px;
     border-radius: 8px;
     display: flex;
     justify-content: center;
     align-items: center;
 }

 .carousel__slide {
     /* padding: 10px; */
 }

 .carousel__prev,
 .carousel__next {
     box-sizing: content-box;
     border: 5px solid white;
 }



 input[type=number] {
     height: 30px;
     line-height: 30px;
     font-size: 12px;
     padding: 0 8px;
     margin-top: 6px;
     margin-bottom: 6px;
     background-color: black;
 }

 input[type=number]::-webkit-inner-spin-button {
     -webkit-appearance: none;
     cursor: pointer;
     display: block;
     width: 8px;
     color: black;
     text-align: center;
     position: relative;
 }

 input[type=number]:hover::-webkit-inner-spin-button {
     background: black url('https://i.stack.imgur.com/YYySO.png') no-repeat 50% 50%;
     width: 14px;
     height: 14px;
     padding: 4px;
     position: relative;
     right: 4px;
     border-radius: 28px;
 }
</style>