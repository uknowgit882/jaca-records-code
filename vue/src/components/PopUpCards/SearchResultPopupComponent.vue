<template>
    <div v-if="showAddRecordPopup" class="search-popup-addRecord-popup">
        <div class="search-popup-addRecord-popup-inner">
            <button class="popup-exit-button" @click.prevent="showAddRecordPopup = !showAddRecordPopup">X</button>
            <div class="search-popup-addRecord-popup-inner-inner">
                <p
                    style="color: white; font-weight: bold; text-align: center; text-decoration: underline; margin: 12px; margin-top: 10px;">
                    Add to Your Library</p>
                <form style="text-align: left; margin: 12px;">
                    <div>
                        <label for="notes" style="color: white; font-weight: bold; margin: 12px;">Notes:</label>
                        <input class="search-popup-addRecord-popup-inner-inputs" id="notes" type="textarea" name="notes"
                            rows="3" columns="40" placeholder="Any notes?" v-model="addRecord.notes" />
                    </div>
                    <div>
                        <label for="quantity" style="color: white; font-weight: bold; margin: 12px;">Quantity:</label>
                        <input class="search-popup-addRecord-popup-inner-inputs" id="quantity" type="number" name="quantity"
                            placeholder="1" v-model="addRecord.quantity" min="1" />
                    </div>
                    <div>
                        <button class="search-popup-button" @click.prevent="addRecordToLibrary">Add Record</button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <div v-if="errorPopup" >
         <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>

    <div class="home" v-if="!isLoading">
        <img src="../../img/Logogif.gif" alt="">
    </div>
    <div v-else>
        <div class="search-popup-container">
            <img class="search-popup-image" :src="activeCard.thumb" alt="">
            <div class="search-popup-artist-title">
                <h2>{{ activeCard.title }}</h2>
            </div>
            <div class="search-popup-year" style="text-align: left; padding: 8px;">
                <span style="color: white; font-weight: bold;">Year: </span>
                <h3>{{ activeCard.year }}</h3>
            </div>
            <div class="search-popup-country" style="text-align: left; padding: 8px;">
                <span style="color: white; font-weight: bold;">Country: </span>
                <h3>{{ activeCard.country }}</h3>
            </div>

            <div class="search-popup-genre">
                <p style="font-weight: bold; color: white;">Genre:</p>
                <div class="ticker-tape-container">
                    <div class="ticker-tape">
                        <span v-for="genre in activeCard.genre" :key="genre">{{ genre }}</span>
                    </div>
                    <div class="ticker-tape" aria-hidden="true">
                        <span v-for="genre in activeCard.genre" :key="genre">{{ genre }}</span>
                    </div>
                </div>
            </div>
            <div class="search-popup-label">
                <p style="font-weight: bold; color: white;">Label(s): </p>
                <div class="ticker-tape-container">
                    <div class="ticker-tape">
                        <span v-for="label in activeCard.label" :key="label">{{ label }}</span>
                    </div>
                    <div class="ticker-tape" aria-hidden="true">
                        <span v-for="label in activeCard.label" :key="label">{{ label }}</span>
                    </div>
                </div>
            </div>
            <div class="search-popup-identifier">
                <p style="font-weight: bold; color: white;">Identifier(s): </p>
                <div class="ticker-tape-container ">
                    <div class="ticker-tape">
                        <span v-for="identifier in activeCard.identifier" :key="identifier">{{ identifier }}</span>
                    </div>
                    <div class="ticker-tape" aria-hidden="true">
                        <span v-for="identifier in activeCard.identifier" :key="identifier">{{ identifier }}</span>
                    </div>
                </div>
            </div>

        </div>
        <div v-if="isInLibrary">
            <p class="search-popup-alreadyOwn">You already have this in your library</p>
        </div>
        <button v-else class="search-popup-button" @click="showAddRecordPopup = !showAddRecordPopup">Add To Library</button>
    </div>
</template>

<script>
import ErrorPopup from './ErrorPopup.vue'
import LibraryService from '../../services/LibraryService'
export default {
    props: ['activeCard'],
    components: {
        ErrorPopup
    },
    data() {
        return {
            isInLibrary: false,
            isLoading: false,
            showAddRecordPopup: false,
            errorPopup: false,
            addRecord: {
                discogs_Id: this.activeCard.id,
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

<style> .search-popup-container {
     display: grid;
     grid-template-columns: 1fr 1fr 1fr;
     grid-template-areas:
         "img artist-title artist-title"
         "img year country"
         "genre label identifier";
     /* "button button button"; */
     gap: 5px;
     font-weight: bold;
 }

 .search-popup-image {
     grid-area: img;
     margin: auto;
     padding: 5px;
     max-width: 200px;
 }

 .search-popup-artist-title {
     grid-area: artist-title;
     text-align: center;
     max-width: 400px;
 }

 .search-popup-year {
     grid-area: year;
     max-width: 200px;
 }

 .search-popup-country {
     grid-area: country;
     max-width: 200px;
 }

 .search-popup-genre {
     grid-area: genre;
     max-width: 200px;
 }

 .search-popup-label {
     grid-area: label;
     max-width: 200px;
 }

 .search-popup-identifier {
     grid-area: identifier;
     max-width: 200px;
 }

 .search-popup-notes {
     grid-area: notes;
 }

 .search-popup-button {
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

 .search-popup-alreadyOwn {
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

 .search-popup-addRecord-popup {
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

 .search-popup-addRecord-popup-inner scoped {
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

 .search-popup-addRecord-popup-inner-inner {
     border-radius: 5px;
     background: black;
     text-align: center;
     height: 250px;
     width: 300px;
 }

 .search-popup-addRecord-popup-inner button {
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

 .search-popup-addRecord-popup-inner-inputs {
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