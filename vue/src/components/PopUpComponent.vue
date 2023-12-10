<template>
    <div v-if="showCard" class="popup">
      <div class="popup-inner">
        <h2>My Popup</h2>
        <p> {{ record.title }}        </p>
        <p v-for="artist in record.artists" v-bind:key="artist">{{ artist.name }}</p>

        <button @click="showCard = !showCard">Exit</button>
      </div>
    </div>
  </template>
  
  <script>
  import Collections from '../services/Collections';
  export default {
    data(){
        return {
            showCard: true
        }
    },
    props: ['record'],
    methods: {
        getCollection(){
            Collections.getInfoForCard()
            .then((response) => {
                if(response.status == 200){
                    this.$store.commit('SHOW_INFO_FROM_COLLECTIONS',response)
                    this.$router.push({name: "Collections"})
                }
            })

        }
    }
  };
  </script>
  
  <style scoped>
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