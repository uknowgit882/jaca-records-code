<template>
   <!-- <button v-on:click="searchRecord">Test</button> -->

    <div class= "tableHead" >
        <span class="thumb">Thumb</span>
        <span class="artist">Artist & Title</span>
        <span class="year">Year</span>
        <span class="genre">Genre</span>
        <span class="country">Country</span>
        <span class="table-head-label">Label</span>
        <span class="barcode">Barcode</span>


    </div>
    <table  >
        <tr>
            <search-result-component v-for="result in this.$store.state.searchResults.data.results" 
            v-bind:key="result.id"
                v-bind:result="result" />

        </tr>

    </table>

</template>

<script>

import SearchResultComponent from '@/components/SearchResultComponent.vue'


export default {
    components: {
        SearchResultComponent
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
            searchResults: [],
        }
    },
    // methods: {
    //     searchRecord() {
    //         AuthService.search(this.Search)
    //             .then(response => {
    //                 this.$store.commit('ADD_SEARCH_RESULT', response);
    //                 //this.isVisible = true;
    //             })
    //             .catch(error => {

    //             })
    //     }
    // }
}
</script>
<style scoped>


.tableHead{
    padding: 20px;
    display: grid;
    grid-area: tableHeader;
    grid-template-columns:9% 14% 10% 10% 10% 10%;
    grid-template-areas: 
    "thumb artist-title year genre country label barcode";
    gap: 10px;
    align-items: center;
    color: white;

}

.thumb{
    grid-template-areas: Thumb
}
.artist{
    grid-template-areas: Artist;
}
.year{
    grid-template-areas: Year;
}
.genre{
    grid-template-areas: Genre;
}
.country{
    grid-template-areas: Country;
}
.table-head-label{
    grid-template-areas: Label;
}
.barcode{
    grid-template-areas: Barcode;
}
</style>

