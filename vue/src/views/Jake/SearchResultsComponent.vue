<template>
  <div></div>
</template>

<script>
import SearchService from '../../services/SearchService';

export default {
  data() {
    return{
        discogsResults: [],
        libraryResults: [],
        collectionsResults: [],
        request: this.$store.state.searchRequest
    }
  },
  methods: {
    searchRecord(request) {
      SearchService.searchDiscogs(request)
        .then((response1) => {
          if (response1.status == 200) {
            this.$store.commit("ADD_SEARCH_RESULT", response1.data);
            this.isLoadingDiscogs = true;
            //this.$router.push({ name: "SearchResult" });
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
      SearchService.searchLibrary(request)
        .then((response2) => {
          if (response2.status == 200) {
            this.$store.commit("ADD_SEARCH_LIBRARY_RESULT", response2.data);
            //this.isLoadingLibrary = true;
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
        SearchService.searchCollections(request)
        .then((response3) => {
          if(response3.status == 200) {
            this.$store.commit("ADD_SEARCH_COLLECTIONS_RESULT", response3.data);
            //this.isLoadingCollections = true;
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
    },
    handleErrorResponse(error, verb) {
      if (error.response) {
        console.log(
          `Error ${verb} topic. Response received was "${error.response.statusText}".`
        );
      } else if (error.request) {
        console.log(`Error ${verb} topic. Server could not be reached.`);
      } else {
        console.log(`Error ${verb} topic. Request could not be created.`);
      }
    },
  },

};
</script>