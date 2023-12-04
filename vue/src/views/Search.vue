<template>
  <div>
    <div class="field has-addons">
      <div class="control">
        <input class="input is-large is-hovered" type="text" placeholder="Find a Record, Artist etc."
          v-model="Search.General">
      </div>
      <div class="control">
        <a class="button is-info is-large" v-on:click="sendSearch()">
          Search
        </a>
      </div>
    </div>
    <button class="button" v-on:click="showForm = !showForm">Advanced Search</button>

    <form v-show="showForm">
      <div class="field">
        <label class="label">Artist</label>
        <div class="control">
          <input class="input" type="text" placeholder="Nirvana" v-model="Search.Artist">
        </div>
        <div class="field">
          <label class="label">Title</label>
          <div class="control">
            <input class="input" type="text" placeholder="Nirvana - nevermind" v-model="Search.Title">
          </div>
        </div>
      </div>
      <div class="field">
        <label class="label">Genre</label>
        <div class="control">
          <input class="input" type="text" placeholder="Rock" v-model="Search.Genre">
        </div>
      </div>
      <div class="field">
        <label class="label">Year</label>
        <div class="control">
          <input class="input" type="text" placeholder="1991" v-model="Search.Year">
        </div>
      </div>
      <div class="field">
        <label class="label">Country</label>
        <div class="control">
          <input class="input" type="text" placeholder="Canada" v-model="Search.Country">
        </div>
      </div>
      <div class="field">
        <label class="label">Label</label>
        <div class="control">
          <input class="input" type="text" placeholder="Dgc" v-model="Search.Label">
        </div>
      </div>
    </form>


  </div>
</template>

<script>
import AuthService from '../services/AuthService';
export default {
  data() {
    return {

      Search: {
        General: "",
        Artist: "",
        Title: "",
        Genre: "",
        Year: "",
        Country: "",
        Label: ""

      },
      showForm: false,
      searchResults: []


    }
  },
  methods: {
    sendSearch(){
      AuthService.search(this.Search)
      .then( (response) => {
        this.searchResults = response.data;
      })
      .catch( (error) => {
        this.handleErrorResponse(error, 'Search Query')
      })
    },
    handleErrorResponse(error, verb) {
      if (error.response) {
        console.log(`Error ${verb} topic. Response received was "${error.response.statusText}".`);
        }
       else if (error.request) {
        console.log(`Error ${verb} topic. Server could not be reached.`)
      } else {
        console.log(`Error ${verb} topic. Request could not be created.`)
      } 
  }
  // created() {
  //   AuthService.search(this.$route.params.Search).then((dataBack) => {
  //     this.Search = dataBack.data;
  //     this.isLoading = false;

  //   });
   },
}
</script>
