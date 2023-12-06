<template>
    
    <div class = "Library-container">
        <record-in-library v-for="record in $store.state.records" v-bind:record="record" v-bind:key="record.id"/>
    
    </div>
    
</template>

<script>
import RecordInLibrary from '@/components/RecordInLibraryComponent.vue';


export default{
    components: {
        RecordInLibrary
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