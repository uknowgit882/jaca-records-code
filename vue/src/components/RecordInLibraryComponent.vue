<template>
    <div class="Record">
        <img class="record-image" v-bind:src="record.album_cover" />
        <h2 class="record-title">{{ record.title }}</h2>
        <h3 class="record-artist">{{ record.artist }}</h3>
    </div>
</template>

<script>
import AddToLibraryService from '../services/AddToLibraryService';
export default {
    props: ['record'],
    methods: {
        DisplayingLibrary() {
            AddToLibraryService.displayRecordsInLibrary()
                .then((response) => {
                    if (response.status == 200) {
                        this.$store.commit('SHOW_RECORDS_IN_LIBRARY', response)
                    }
                })
        }

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
