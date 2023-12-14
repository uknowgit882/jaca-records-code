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
<div v-if="cardType == 'searchCollections'">
    <collections-result-popup-component  v-bind:collection="this.activeCard"></collections-result-popup-component>
</div>

</template>

<script>
import SearchResultPopup from './PopUpCards/SearchResultPopupComponent.vue'
import LibraryResultPopup from './PopUpCards/LibraryResultPopupComponent.vue';
import LibrarySearchResultPopup from './PopUpCards/LibrarySearchResultPopupComponent.vue';
import CollectionsResultPopupComponent from './PopUpCards/CollectionsResultPopupComponent.vue';
export default{
    props: ['activeCard', 'cardType'],
    components: {
        SearchResultPopup,
        LibraryResultPopup,
        LibrarySearchResultPopup,
        CollectionsResultPopupComponent
    }


}

</script>

<style>  
</style>