<template> 
<div v-if="cardType == 'searchAPI'">
    <search-result-popup v-bind:activeCard="this.activeCard"></search-result-popup>
</div>
<div v-if="cardType == 'library' || cardType == 'searchLibrary'">
    <library-result-popup  v-bind:activeCard="this.activeCard"></library-result-popup>
</div>
</template>

<script>
import SearchResultPopup from './PopUpCards/SearchResultPopupComponent.vue'
import LibraryResultPopup from './PopUpCards/LibraryResultPopupComponent.vue';
export default{
    props: ['activeCard', 'cardType'],
    components: {
        SearchResultPopup,
        LibraryResultPopup
    }


}

</script>

<style>  
</style>