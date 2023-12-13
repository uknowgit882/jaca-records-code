<template>
  <div class="SearchBar" @keyup.enter="sendSearch()">
    <div class="level-item is-flex is-flex-direction-column is-justify-content-end">
      <div class="field has-addons">
        <p class="control">
          <input class="input" type="text" placeholder="Find a Record, Artist, etc." v-model="Search.General" />
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
        <!-- <div class="dropdown-trigger">xxx</div> -->
        <div class="dropdown-menu" id="advanced-search-dropdown" role="menu">
          <div class="dropdown-content">

            <form class="form" v-show="showForm">
              <div class="field">
                <label class="label">Artist</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. Nirvana" v-model="Search.Artist" />
                </div>
                <div class="field">
                  <label class="label">Title</label>
                  <div class="control">
                    <input class="searchresultype" type="text" placeholder="e.g. Nevermind" v-model="Search.Title" />
                  </div>
                </div>
              </div>
              <div class="field">
                <label class="label">Genre</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. Rock" v-model="Search.Genre" />
                </div>
              </div>
              <div class="field">
                <label class="label">Year</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. 1991" v-model="Search.Year" />
                </div>
              </div>
              <div class="field">
                <label class="label">Country</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. Canada" v-model="Search.Country" />
                </div>
              </div>
              <div class="field">
                <label class="label">Label</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. DGC" v-model="Search.Label" />
                </div>
              </div>
              <div class="field">
                <label class="label">Barcode</label>
                <div class="control">
                  <input class="searchresultype" type="text" placeholder="e.g. 7 20642-44251 7"
                    v-model="Search.Barcode" />
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
    // sendSearch(){
    //   this.$store.commit("SET_SEARCH_REQUEST", this.Search);
    //   this.$router.push({ name: "SearchResult"});
    // },
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
            this.$store.commit("ADD_SEARCH_LIBRARY_RESULT", response2.data);
            //this.$router.push({ name: "SearchResult" });
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Search Query");
        });
      SearchService.searchCollections(this.Search)
        .then((response3) => {
          if (response3.status == 200) {
            this.$store.commit("ADD_SEARCH_COLLECTIONS_RESULT", response3.data);
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
  },

  // created() {
  //     this.searchDiscogs();
  //     this.searchLibrary();
  //     this.searchCollections();
  //   }
};
</script>

<style scoped>
.input {
  flex-grow: 1;
  margin-top: 11px;
  width: 800px;
  background-color: black;
  color: white;
  right: 2%;
}

.button {
  margin-top: 11px;
  right: 18px;
}

.dropdown.is-active {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
}

.dropdown-menu {
  scale: 108%;
  /* <div class="level-item is-flex is-flex-direction-column is-justify-content-end"> */
  /* background-color: green; */

}

.dropdown-content {
  width: 114%;
  scale: 97%;
  padding-bottom: 50px;
  position: relative;
  left: 101%;
  background-color: rgba(0, 0, 0, 0.75);
  margin-top: 7px;
  padding: 12px;
  padding-top: 10px;
  text-align: center;
}

.label {
  color: white;
  text-align: left;

}

.searchresultype {
  background-color: #000000;
  color: white;
}

::placeholder {
  color: gray;
}
</style>