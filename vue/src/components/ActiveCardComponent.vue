<template> 
<div v-if="cardType == 'searchAPI'">
    <search-result-popup v-bind:activeCard="this.activeCard"></search-result-popup>
</div>

</template>

<script>
import SearchResultPopup from './PopUpCards/SearchResultPopupComponent.vue'
export default{
    props: ['activeCard', 'cardType'],
    components: {
        SearchResultPopup
    }


}

</script>

<style>  

</style>