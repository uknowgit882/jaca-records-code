<template>
    <div v-if="showRecordOptionsPopup" class="library-popup-RecordOptions-popup">
        <div class="library-popup-RecordOptions-popup-inner">
            <button class="popup-exit-button" @click.prevent="showRecordOptionsPopup = !showRecordOptionsPopup">X</button>
            <div class="library-popup-RecordOptions-popup-inner-inner"
                :class="optionsToggle != 0 ? 'library-popup-RecordOptions-bigBox' : ''">
                <div class="library-popup-RecordOptions-buttons" style="margin-left: 8px;">
                    <div>
                        <button class="library-popup-RecordOptions-popup-inner-inner-buttons"
                            style="background-color: #09A3DA;" @click="optionsToggle = 1">Add to Collection</button>
                    </div>
                    <div>
                        <button class="library-popup-RecordOptions-popup-inner-inner-buttons"
                            style="background-color: #09A3DA;" @click="optionsToggle = 2">Update Quantity</button>
                    </div>
                    <div>
                        <button class="library-popup-RecordOptions-popup-inner-inner-buttons"
                            style="background-color: #09A3DA;" @click="optionsToggle = 3">Update Notes</button>
                    </div>
                    <div>
                        <button style="width: 150px;" @click="areYouSurePopup = true">Delete Record</button>
                    </div>
                </div>
                <div style="flex-grow: 2; align-items: stretch; margin: 12px;">
                    <div :class="optionsToggle == 1 ? 'makeVisible' : 'notVisible'" style="border-radius: 4px; ">
                        <p>Add to Collection</p>
                        <div class="scrollableBox" style="height: 100px; max-height: 110px">
                            <button v-for="eligibleCollection in collectionsYouCanAddTo" :key="eligibleCollection.name"
                                style="background-color: #17B39F; width: 100%; margin: 10px;"
                                @click="addRecordToCollection(eligibleCollection.name)">Add to: {{ eligibleCollection.name
                                }}</button>
                        </div>

                    </div>
                    <div :class="optionsToggle == 2 ? 'makeVisible' : 'notVisible'" style="border-radius: 4px;">
                        <div style="display: flex; flex-direction: column; margin-top: 4px;">
                            <p>Update Quantity</p>
                            <input type="number" id="updateQuantity" name="updateQuantity" placeholder="1" min="1"
                                v-model="updateLibrary.quantity"
                                style="align-items: left; margin: 12px; border-radius: 4px; color: white;">
                            <button style="margin: 12px; background-color: #17B39F;" @click="updateQuantity">Update</button>
                        </div>
                    </div>
                    <div :class="optionsToggle == 3 ? 'makeVisible' : 'notVisible'" style="border-radius: 4px; ">
                        <div style="display: flex; flex-direction: column; margin-top: 4px;">
                        <p>Update Notes</p>
                        <input type="textarea" id="updateNotes" name="updateNotes" placeholder="Updated note:" 
                                v-model="updateLibrary.notes"
                                style="align-items: left; margin: 12px; border-radius: 4px; color: white;">
                            <button style="margin: 12px; background-color: #17B39F;" @click="updateNotes">Update</button>
                        </div>
                    </div>
                    <p v-if="weAreOnIt">We're on it!</p>
                    <p v-if="actionSuccessful">Success!</p>
                    <button v-if="optionsToggle != 0" @click="optionsToggle = 0">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div v-if="errorPopup">
        <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>

    <div v-if="areYouSurePopup" class="library-popup-AreYouSure-popup">
        <div class="library-popup-AreYouSure-popup-inner">
            <button class="popup-exit-button" @click.prevent="areYouSurePopup = false">X</button>
            <div class="library-popup-AreYouSure-popup-inner-inner" style="background-color: ">
                <p style="margin-top: 20px; font-weight: bold; padding-left: 0;">Are you sure?</p>
                <button class="library-popup-RecordOptions-popup-inner-inner-buttons"
                    style="background-color: black; color: white; font-size: 14px; height: 30px; border-radius: 4px; margin: 12px; margin-top: 20px; border-color: white;"
                    @click="deleteRecord">Yes</button>
                <button class="library-popup-RecordOptions-popup-inner-inner-buttons"
                    style="background-color: #09A3DA; color: white; font-size: 14px; height: 30px; border-radius: 4px; margin: 12px; margin-bottom: 20px; border-color: white;"
                    @click="areYouSurePopup = false">Cancel</button>
            </div>
        </div>
    </div>

    <div class="home" v-if="!isLoading">
        <img src="../../../img/Logogif.gif" alt="">
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

            <div class="library-popup-notes scrollableBox" style="text-align: left; padding: 8px; max-height: 150px;">
                <span style="color: white; font-weight: bold;">My Notes: </span>
                <span>{{ activeCard.notes }}</span>
            </div>

            <div class="library-popup-table">
                <!-- implementing this tab thing... -->
                <div>
                    <div class="tab" style="height: 40px; display: flex; align-items: center; border-radius: 4px;">
                        <button class="tablinks" @click="tabToggle = 1" style="color: white;">Tracks</button>
                        <button class="tablinks" @click="tabToggle = 2" style="color: white;">Formats</button>
                        <button class="tablinks" @click="tabToggle = 3" style="color: white;">Labels</button>
                        <button class="tablinks" @click="tabToggle = 4" style="color: white;">Identifiers</button>
                        <button class="tablinks" @click="tabToggle = 5" style="color: white;">Record Notes</button>
                    </div>

                    <div class="scrollableBox" style="max-height: 110px">

                        <div :class="tabToggle == 1 ? 'makeVisible' : 'notVisible'" class="tabcontent"
                            style="border-radius: 4px;">
                            <div>
                                <div class="library-popup-tracks-table">
                                    <span class="library-popup-tracks-table-title"
                                        style="text-align: left; color: white;">Track</span>
                                    <span class="library-popup-tracks-table-position" style="color: white; ">Pos.</span>
                                    <span class="library-popup-tracks-table-duration" style="color: white; ">Length</span>
                                    <table class="library-popup-tracks-table-row">
                                        <tr>
                                            <track-component v-for="track in activeCard.record.tracklist" :key="track.title"
                                                :track="track"></track-component>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div :class="tabToggle == 2 ? 'makeVisible' : 'notVisible'" class="tabcontent"
                        style="border-radius: 4px; ">
                        <p class="smallFont" v-for="format in activeCard.record.formats" :key="format.name"
                            style="color: #183E50">{{ format.name }}</p>
                    </div>
                    <div :class="tabToggle == 3 ? 'makeVisible' : 'notVisible'" class="tabcontent"
                        style="border-radius: 4px; ">
                        <p class="smallFont" v-for="label in activeCard.record.labels" :key="label.name"
                            style="color: #183E50">{{ label.name }}</p>
                    </div>
                    <div :class="tabToggle == 4 ? 'makeVisible' : 'notVisible'" class="tabcontent"
                        style="border-radius: 4px;">
                        <p class="smallFont" v-for="identifier in activeCard.record.identifiers" :key="identifier.value"
                            style="color: #183E50">{{ identifier.type }}, {{ identifier.value }}</p>
                    </div>
                    <div :class="tabToggle == 5 ? 'makeVisible' : 'notVisible'" class="tabcontent"
                        style="border-radius: 4px;">
                        <p class="smallFont scrollableBox" style="color: #183E50">{{ activeCard.record.notes }}</p>
                    </div>
                </div>
            </div>

        </div>
        <button class="library-popup-button" @click="showRecordOptionsPopup = !showRecordOptionsPopup">Show Options</button>
    </div>
</template>

<script>


import ErrorPopup from './ErrorPopup.vue'
import LibraryService from '../../services/LibraryService'
import CollectionService from '../../services/CollectionsService'
import { defineComponent } from 'vue'
import { Carousel, Navigation, Pagination, Slide } from 'vue3-carousel'
import TrackComponent from './TrackComponent.vue'

export default {
    props: ['activeCard'],
    components: {
        ErrorPopup,
        Carousel,
        Slide,
        Pagination,
        TrackComponent
    },
    data() {
        return {
            isLoading: false,
            showRecordOptionsPopup: false,
            errorPopup: false,
            errorMessage: "",
            areYouSurePopup: false,
            weAreOnIt: false,
            actionSuccessful: false,
            tabToggle: 1,
            optionsToggle: 0,
            recordToAddToCollection: {
                discogs_Id: this.activeCard.record.id
            },
            updateLibrary: {
                discogs_Id: this.activeCard.record.id,
                notes: "",
                quantity: 0,
            },
            eligibleCollections: []
        }
    },
    computed: {
        collectionsYouCanAddTo() {
            return this.eligibleCollections;
        }
    },
    methods: {
        getRecordInLibrary() {
            LibraryService.GetRecordInLibrary(this.activeCard.record.id)
                .then(response => {
                    this.isInLibrary = true;
                    this.isLoading = true;
                })
                .catch(error => {
                    this.isInLibrary = false;
                    this.isLoading = true;
                })
        },
        successDisappearer() {
            this.actionSuccessful = false;
            return this.actionSuccessful;
        },
        displaySuccess() {
            this.actionSuccessful = true;
            setTimeout(this.successDisappearer, 2000)
        },
        getCollectionsYouCanAddThisRecordTo() {
            CollectionService.GetAllCollections()
                .then(response => {
                    const collections = response.data;
                    this.eligibleCollections = collections.filter((collection) => {
                        const collectionWithoutThisRecord = collection.records.filter((record) => {
                            return record.id == this.activeCard.record.id
                        })
                        if (collectionWithoutThisRecord.length == 0) {
                            return collection
                        }
                    })
                    this.isInLibrary = true;
                    this.isLoading = true;
                })
                .catch(error => {
                    this.isInLibrary = false;
                    this.isLoading = true;
                })
        },
        updateQuantity(){
            this.isLoading = false;
            this.weAreOnIt - true;
            LibraryService.ChangeQuantityForRecordInLibrary(this.updateLibrary)
            .then( response => {
                this.isLoading = true;
                this.weAreOnIt = false;
                this.displaySuccess();
            })
            .catch(error => {
                    this.isInLibrary = false;
                    this.weAreOnIt = false;
                    this.optionsToggle = 0;
                    this.errorMessage = `update the quantity of this record`;
                    this.isLoading = true;
                    this.errorPopup = true;
                })
        },
        updateNotes(){
            this.isLoading = false;
            this.weAreOnIt - true;
            LibraryService.ChangeNoteForRecordInLibrary(this.updateLibrary)
            .then( response => {
                this.isLoading = true;
                this.weAreOnIt = false;
                this.displaySuccess();
            })
            .catch(error => {
                    this.isInLibrary = false;
                    this.weAreOnIt = false;
                    this.optionsToggle = 0;
                    this.errorMessage = `update the notes of this record`;
                    this.isLoading = true;
                    this.errorPopup = true;
                })
        },
        addRecordToCollection(collectionName) {
            this.isLoading = false;
            this.weAreOnIt = true;
            CollectionService.AddRecordToCollection(collectionName, this.recordToAddToCollection)
                .then(response => {
                    this.isInLibrary = true;
                    this.isLoading = true;
                    this.weAreOnIt = false;
                    this.getCollectionsYouCanAddThisRecordTo();
                    this.displaySuccess();
                })
                .catch(error => {
                    this.isInLibrary = false;
                    this.weAreOnIt = false;
                    this.optionsToggle = 0;
                    this.errorMessage = `add this record to ${collectionName}`;
                    this.isLoading = true;
                    this.errorPopup = true;
                })
        },
        deleteRecord() {
            this.areYouSurePopup = false;
            this.showRecordOptionsPopup = false;
            LibraryService.DeleteRecordInLibrary(this.activeCard.record.id)
                .then(response => {
                    this.isLoading = true;
                    this.$router.go()
                })
                .catch(error => {
                    this.isLoading = true;
                    this.errorMessage = "deleting this record in your library";
                    this.errorPopup = true;
                })
        }
    },
    created() {
        this.getRecordInLibrary();
        this.getCollectionsYouCanAddThisRecordTo();
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
     font-weight: normal;
     font-size: 12px;
 }

 .library-popup-table {
     grid-area: table;
 }

 .library-popup-tracks-table {
     display: grid;
     grid-template-columns: 3fr 1fr 1fr;
     grid-template-areas:
         "trackTitle trackPosition trackDuration"
         "trackRow trackRow trackRow";
     gap: 2px;
     font-weight: normal;
     font-size: 10px;

 }


 .library-popup-tracks-table-title {
     grid-area: trackTitle;
     text-align: left;
 }

 .library-popup-tracks-table-position {
     grid-area: trackPosition;
 }

 .library-popup-tracks-table-duration {
     grid-area: trackDuration;
 }

 .library-popup-tracks-table-row {
     grid-area: trackRow;
 }

 .smallFont {
     font-weight: normal;
     font-size: 10px;
 }

 .library-popup-button {
     grid-area: button;
     margin-top: 8px;
     padding: 8px;
     background-color: #09A3DA;
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

 .library-popup-RecordOptions-popup {
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

 .library-popup-RecordOptions-popup-inner scoped {
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

 .library-popup-RecordOptions-popup-inner-inner {
     border-radius: 5px;
     background: black;
     text-align: center;
     height: 220px;
     width: 180px;
     display: flex;
     align-items: top;
     justify-content: stretch;
     margin: 12px;
 }

 .library-popup-RecordOptions-bigBox {
     height: 220px;
     width: 530px;
 }

 .library-popup-RecordOptions-buttons {
     display: flex;
     flex-direction: column;
     justify-content: stretch;
 }

 .library-popup-RecordOptions-popup-inner-inner-buttons {
     background-color: #09A3DA;
     width: 150px;
 }

 .library-popup-RecordOptions-popup-inner button {
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

 .library-popup-RecordOptions-popup-inner-inputs {
     border-radius: 5px;
     box-sizing: border-box;
     width: 100%;
     background-color: black;
     color: white;
 }

 .library-popup-AreYouSure-popup {
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

 .library-popup-AreYouSure-popup-inner scoped {
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

 .library-popup-AreYouSure-popup-inner button {
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

 .library-popup-AreYouSure-popup-inner-inner {
     border-radius: 5px;
     background: #EA5143;
     ;
     text-align: center;
     height: 200;
     width: 200px;
     display: flex;
     flex-direction: column;
     align-items: center;
     margin: 12px;
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


 /* tab styling */
 .tab {
     overflow: hidden;
     border: 1px solid #F89590;
     background-color: black;
 }

 /* Style the buttons that are used to open the tab content */
 .tab button {
     background-color: inherit;
     float: left;
     border: none;
     outline: none;
     cursor: pointer;
     padding: 14px 16px;
     transition: 0.3s;
 }

 /* Change background color of buttons on hover */
 .tab button:hover {
     background-color: #F89590;
 }

 /* Create an active/current tablink class */
 .tab button.active {
     background-color: white;
     color: black;
 }

 /* Style the tab content */
 .tabcontent {
     display: none;
     padding: 6px 12px;
     border: 1px solid #F89590;
     border-top: none;
 }

 .tabcontent {
     animation: fadeEffect 1s;
     /* Fading effect takes 1 second */
 }

 /* Go from zero to full opacity */
 @keyframes fadeEffect {
     from {
         opacity: 0;
     }

     to {
         opacity: 1;
     }
 }

 .makeVisible {
     display: block;
 }

 .notVisible {
     display: none;
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
 }</style>