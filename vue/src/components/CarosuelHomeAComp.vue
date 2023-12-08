<template>
    <div class="display" v-if="$store.state.searchResults.data">
        <!-- <div class="box" id="Carousel" v-for="item in $store.state.searchResults.data.results" :key="item.id">Carosuel 
            <div>{{ item.title }}</div>
            <img class="result-thumbnail" v-bind:src="item.thumb" />
        </div> -->
    </div>
</template>

<script>
import SearchResultComponentVue from "./SearchResultComponent.vue";

export default{
    SearchResultComponentVue
}
</script>

<style scoped>
#Carousel{
    display: flex;
  align-items: center;
  justify-content: center;
    width: 1500px;
    height: 500px;
}
</style>