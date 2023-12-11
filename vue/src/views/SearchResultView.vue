<template>
    <div class="searchResultsGrid" >
        <div class="apiSearchResults">
            <h2>Search Results:</h2>
            <CarouselComponent v-bind:carouselRecords="$store.state.searchResults.results"
                v-bind:carouselChooser="'searchAPI'"></CarouselComponent>
        </div>
        <div class="librarySearchResults">
            <h2>Library Results:</h2>
            <p>Records you own</p>
            <CarouselComponent v-bind:carouselRecords="$store.state.searchLibraryResults"
                v-bind:carouselChooser="'searchLibrary'"></CarouselComponent>
        </div>
        <div class="collectionsSearchResults">
            <h2>Collection Results:</h2>
            <p>Collections where you have saved this record</p>
            <CarouselComponent v-bind:carouselRecords="$store.state.searchCollectionsResults"
                v-bind:carouselChooser="'searchCollections'"></CarouselComponent>
        </div>
    </div>
</template>

<script>
import SearchService from '../services/SearchService'
import SearchResultComponent from '@/components/SearchResultComponent.vue'
import CarouselComponent from '../components/CarouselComponent.vue';

export default {
    components: {
        CarouselComponent
    },
    data() {
        return {
            //isVisible: false,
            // This is where user search item
            Search: {
                General: "",
                Artist: "",
                Title: "",
                Genre: "",
                Year: "",
                Country: "",
                Label: ""

            },
            searchResults: []
        }
    },
    methods: {
        searchRecord() {
            SearchService.search(this.Search)
                .then(response => {
                    this.$store.commit('ADD_SEARCH_RESULT', response);
                    //this.isVisible = true;
                })
                .catch(error => {

                })
        }
    }
}
</script>

<style scoped>
.searchResultsGrid{
    display: grid;
    grid-template-areas:
    "API"
    "LIBRARY"
    "COLLECTIONS";
    height: 1vh;
    margin-top: 20px
}
.apiSearchResults{
    grid-template-areas: "API";
}
.librarySearchResults{
    grid-template-areas: "LIBRARY";
}
.collectionsSearchResults{
    grid-template-areas: "COLLECTIONS";
}

h2{
    font-size: 2rem;
    font-weight: bolder;
    color: #D1D301;
    text-align: left;
    padding-left: 20px;
}
p{
    font-weight: bolder;
    color: #D1D301;
    text-align: left;
    padding-left: 20px;
} 

.tableHead {
    padding: 20px;
    display: grid;
    grid-area: tableHeader;
    padding-left: 15%;
    grid-template-columns: 9% 10% 8% 10% 10% 20%;
    grid-template-areas:
        "thumb artist-title year genre country label library";
    gap: 10px;
    align-items: center;
    color: white;

}

.table-table {
    background: rgba(0, 0, 0, 0.587);
}

.thumb {
    grid-template-areas: Thumb;
}

.artist {
    grid-template-areas: Artist;
}

.year {
    grid-template-areas: Year;
}

.genre {
    grid-template-areas: Genre;
}

.country {
    grid-template-areas: Country;
}

.table-head-label {
    grid-template-areas: Label;
}

.barcode {
    grid-template-areas: Barcode;
}
</style>

