<template>
    
    <div class = "Library-container">
        <record-in-library v-for="record in $store.state.records.data" v-bind:record="record" v-bind:key="record.id"/>
    
    </div>
    
</template>

<script>
import RecordInLibrary from '@/components/RecordInLibraryComponent.vue';
import AddToLibraryService from '../services/AddToLibraryService';
import LibraryService from '../services/LibraryService';


export default{
    components: {
        RecordInLibrary,
        // AddToLibraryService
    },
    methods: {
        // DisplayingLibrary() {
        //     AddToLibraryService.displayRecordsInLibrary()
        //         .then((response) => {
        //             if (response.status == 200) {
        //                 this.$store.commit('SHOW_RECORDS_IN_LIBRARY', response)
        //                 this.$router.push({ name: "Library" });
        //             }
        //         })
        // }
        getLibrary() {
            LibraryService.GetLibrary()
            .then( response => {
                this.$store.commit('ADD_RECORDS_TO_LIBRARY', response.data)
            })
        }
    },
    created(){
        this.getLibrary();
    }
}
</script>

<style scoped>
.Library-container{
    display: flex;
  justify-content: space-evenly;
  flex-wrap: wrap;
  /* background-color: black; */

}

</style>