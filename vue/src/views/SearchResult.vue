<template>
    <button v-on:click="searchRecord">Test</button>
</template>

<script>
import AuthService from '../services/AuthService'

export default{
    data(){
        return {
            search: {
                general: 'queen'
            }
        }
    },
    methods: {
        searchRecord(){
            AuthService.search(this.search)
            .then ( response => {
                this.$store.commit('ADD_SEARCH_RESULT', response)
            })
            .catch( error => {
                
            })
        }
    }
}
</script>
