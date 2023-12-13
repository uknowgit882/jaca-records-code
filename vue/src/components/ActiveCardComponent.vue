<template> 
<div v-if="cardType == 'searchAPI'">
    <search-result-popup v-bind:activeCard="this.activeCard"></search-result-popup>
</div>
<div v-if="cardType == 'library'">
    <library-result-popup  v-bind:activeCard="this.activeCard"></library-result-popup>
</div>
<div v-if="cardType == 'searchLibrary' || cardType == 'collection'">
    <library-search-result-popup  v-bind:activeCard="this.activeCard"></library-search-result-popup>
</div>
</template>

<script>
import SearchResultPopup from './PopUpCards/SearchResultPopupComponent.vue'
import LibraryResultPopup from './PopUpCards/LibraryResultPopupComponent.vue';
import LibrarySearchResultPopup from './PopUpCards/LibrarySearchResultPopupComponent.vue';
export default{
    props: ['activeCard', 'cardType'],
    components: {
        SearchResultPopup,
        LibraryResultPopup,
        LibrarySearchResultPopup
    }


}

</script>

<style>  
</style>