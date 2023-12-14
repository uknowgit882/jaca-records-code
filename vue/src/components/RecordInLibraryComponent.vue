<template>
    <div class="Record">
        <!-- <img class="image" v-bind:src="record.album_cover" /> -->
        <h2 class="title">{{ records.title }}</h2>
        <h3 class="artist">{{ records.artist }}</h3>
    </div>
</template>

<script>
import AddToLibraryService from '../services/AddToLibraryService';
import AddToLibraryComponents from './AddToLibraryComponents.vue';
export default {
    components: {
        AddToLibraryComponents
    },
    props: ['records'],
    methods: {
        // DisplayingLibrary() {
        //     AddToLibraryService.displayRecordsInLibrary()
        //         .then((response) => {
        //             if (response.status == 200) {
        //                 this.$store.commit('SHOW_RECORDS_IN_LIBRARY', response)
        //             }
        //         })
        // }

    }
}
</script>

<style scoped>
.record-image {
    height: 250px;
    width: 250px;
    display: flex;
    flex-wrap: wrap;

}
</style>
