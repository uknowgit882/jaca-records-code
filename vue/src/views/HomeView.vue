<template>
  <div class="home">
    <h1>Home</h1>
    <p>You must be authenticated to see this</p>
    <button v-on:click="searchRecord">Test</button>
    <button v-on:click="logout">Logout</button>
  </div>
</template>

<script>

import AuthService from '../services/AuthService';
export default {
  data() {
        return {
            Search: {
                General: "",
                Artist: "queen",
                Title: "",
                Genre: "",
                Year: "",
                Country: "",
                Label: ""

            },
        }
    },
    methods: {
        searchRecord() {
            AuthService.search(this.Search)
                .then(response => {
                    this.$store.commit('ADD_SEARCH_RESULT', response);
                    this.isVisible = true;
                })
                .catch(error => {

                })
        },
        logout(){
          this.$store.commit('LOGOUT');
        }
    }
};
</script>
