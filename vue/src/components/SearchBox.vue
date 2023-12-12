<template>
  <div class="SearchBar" @keyup.enter="sendSearch()">
    <div class="level-item is-flex is-flex-direction-column is-justify-content-end">
      <div class="field has-addons">
        <p class="control">
          <input class="input" type="text" placeholder="Find a Record, Artist etc." v-model="Search.General" />
        </p>
        <p class="control">
          <button class="button" v-on:click="sendSearch()"><i class="fa-solid fa-magnifying-glass"></i></button>
        </p>
        <p class="control-adv">
          <button class="button" v-on:click="showForm = !showForm">
            <strong>. . .</strong>
          </button>
        </p>
      </div>

      <div class="dropdown is-active" v-show="showForm">
        <div class="dropdown-trigger"></div>
        <div class="dropdown-menu" id="advanced-search-dropdown" role="menu">
          <div class="dropdown-content">
            <form class="form" v-show="showForm">
              <div class="field">
                <label class="label">Artist</label>
                <div class="control">
                  <input class="input" type="text" placeholder="Nirvana" v-model="Search.Artist" />
                </div>
                <div class="field">
                  <label class="label">Title</label>
                  <div class="control">
                    <input class="input" type="text" placeholder="Nirvana - nevermind" v-model="Search.Title" />
                  </div>
                </div>
              </div>
              <div class="field">
                <label class="label">Genre</label>
                <div class="control">
                  <input class="input" type="text" placeholder="Rock" v-model="Search.Genre" />
                </div>
              </div>
              <div class="field">
                <label class="label">Year</label>
                <div class="control">
                  <input class="input" type="text" placeholder="1991" v-model="Search.Year" />
                </div>
              </div>
              <div class="field">
                <label class="label">Country</label>
                <div class="control">
                  <input class="input" type="text" placeholder="Canada" v-model="Search.Country" />
                </div>
              </div>
              <div class="field">
                <label class="label">Label</label>
                <div class="control">
                  <input class="input" type="text" placeholder="Dgc" v-model="Search.Label" />
                </div>
              </div>
              <div class="field">
                <label class="label">Barcode</label>
                <div class="control">
                  <input class="input" type="text" placeholder="123aAs3445" v-model="Search.Barcode" />
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import SearchService from "../services/SearchService";

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
        Label: "",
        Barcode: "",
      },
      showForm: false,
    };
  },
  methods: {
    // C# sends found search results back to front end
    sendSearch() {
      SearchService.searchDiscogs(this.Search)
        .then((response1) => {
          if (response1.status == 200) {
            this.$store.commit("ADD_SEARCH_RESULT", response1.data);
            this.$router.push({ name: "SearchResult" });
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
      SearchService.searchLibrary(this.Search)
        .then((response2) => {
          if (response2.status == 200) {
            this.$store.commit("ADD_SEARCH_LIBRARY_RESULT", response2.data)
            //this.$router.push({ name: "SearchResult" });
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
      SearchService.searchCollections(this.Search)
        .then((response3) => {
          if (response3.status == 200) {
            this.$store.commit("ADD_SEARCH_COLLECTIONS_RESULT", response3.data)
            //this.$router.push({ name: "SearchResult" });
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
    // created() {
    //   AuthService.search(this.$route.params.Search).then((dataBack) => {
    //     this.Search = dataBack.data;
    //     this.isLoading = false;

    //   });
  },
};
</script>

<style scoped>
.input {
  margin-top: 11px;
  width: 800px;
}

.button {
  margin-top: 11px;
}

.dropdown-content {
  width: 38%;
  padding-bottom: 50px;
  scale: 97%;
  position: relative;
  left: 19%;
  margin-top: -3.4%;
}

</style>