<template>
    
    <div class = "Library-container">
        <record-in-library v-for="record in $store.state.records.data" v-bind:record="record" v-bind:key="record.id"/>
    
    </div>
    
</template>

<script>
import RecordInLibrary from '@/components/RecordInLibraryComponent.vue'; 
import LibraryService from '../services/LibraryService';


export default{
    components: {
        RecordInLibrary, 
    },
    methods: { 
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