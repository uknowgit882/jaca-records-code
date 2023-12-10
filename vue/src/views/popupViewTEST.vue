<template>
    <div>
      <button @click="returnCollection">Toggle Popup</button>
      
      <!-- <img src="https://i.ebayimg.com/images/g/CFwAAOSwTW1dgjwJ/s-l640.jpg" alt="Click me"/> -->
      <div v-if="showPopup" class="popup">
        <button @click="showPopup =! showPopup">Exit</button>
      <pop-up-component @click="showPopup = !showPopup" v-for="record in collection.records" v-bind:record="record" v-bind:key="record.id"></pop-up-component>
    </div>
      <!-- <div v-if="showPopup" class="popup">
        <div class="popup-inner">
          <h2>More information:</h2>
          <h2>{{ collection[0].name }}</h2>
          <p>
            
          </p>
          
          <button @click="showPopup = !showPopup">Close Information</button>
          
        </div>
        <record-in-collection v-for="record in $store.state.Collections.records" v-bind:record="record" v-bind:key="record.id"/>
      </div> -->
    </div>

    <div class="collections-components">
    </div>
  </template>
  
  <script>
  import PopUpComponent from '../components/PopUpComponent.vue';
  export default {
    components: {
      PopUpComponent
    },
    data() {
      return {
        showPopup: false,
        collection: {}
      };
    },
    methods: {
      returnCollection() {
        this.showPopup = !this.showPopup
        this.collection = this.$store.state.Collections;
      }
    }
  };
  </script>
  
  <style>
  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
  }
  
  .popup {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.2);
    display: flex;
    align-items: center;
    justify-content: center;
  }
  
  .popup-inner {
    background: #fff;
    padding: 32px;
    text-align: center;
  }
  
  .popup-inner button {
    margin-top: 16px;
    padding: 8px 16px;
    background-color: #3498db;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }
  </style>