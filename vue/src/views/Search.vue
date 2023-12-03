<template>

  <div>
  <div class="field has-addons">
  <div class="control">
    <input class="input is-large is-hovered" type="text" placeholder="Find a Record, Artist etc." v-model="Record.General">
  </div>
  <div class="control">
    <a class="button is-info is-large">
      Search
    </a>
  </div>
</div>
<button class="button" v-on:click="showForm = !showForm" >Advanced Search</button>

<form v-show="showForm">
<div class="field">
  <label class="label">Artist</label>
  <div class="control">
    <input class="input" type="text" placeholder="Nirvana" v-model="Record.Artist" >
  </div>
  <div class="field">
  <label class="label">Title</label>
  <div class="control">
    <input class="input" type="text" placeholder="Nirvana - nevermind" v-model="Record.Title">
  </div>
</div>
</div>
<div class="field">
  <label class="label">Genre</label>
  <div class="control">
    <input class="input" type="text" placeholder="Rock" v-model="Record.Genre">
  </div>
</div>
<div class="field">
  <label class="label">Year</label>
  <div class="control">
    <input class="input" type="text" placeholder="1991" v-model="Record.Year">
  </div>
</div>
<div class="field">
  <label class="label">Country</label>
  <div class="control">
    <input class="input" type="text" placeholder="Canada" v-model="Record.Country">
  </div>
</div>
<div class="field">
  <label class="label">Label</label>
  <div class="control">
    <input class="input" type="text" placeholder="Dgc" v-model="Record.Label">
  </div>
</div>
</form>


</div>
</template>

<script>
export default {
  data(){
    return{

       Record: {
        General: "",
        Artist:"",
        Title: "",
        Genre: "",
        Year: "",
        Country: "",
        Label: ""
        
       },
       showForm: false


    }
}
};
</script>
