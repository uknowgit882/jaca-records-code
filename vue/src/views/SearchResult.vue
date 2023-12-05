<template>
    <button v-on:click="searchRecord">Test</button>
</template>

<script>
import AuthService from '../services/AuthService'

export default{
    data(){
        return {
            Search: {
                General: "",
                Artist: "queen",
                Title: "",
                Genre: "",
                Year: "",
                Country: "",
                Label: ""

            }
        }
    },
    methods: {
        searchRecord(){
            AuthService.search(this.Search)
            .then ( response => {
                this.$store.commit('ADD_SEARCH_RESULT', response)
            })
            .catch( error => {
                
            })
        }
    }
}
</script>
