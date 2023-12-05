<template>
  <div id="capstone-app">
    <NavigationBar/>
 
    <!-- <div id="nav"> -->
      <!-- <router-link v-bind:to="{ name: 'home' }">Home</router-link>&nbsp;|&nbsp; -->
      <!-- <router-link v-bind:to="{ name: 'logout' }" v-if="$store.state.token != ''">Logout</router-link>
    </div> -->

    <router-view />
  </div>
</template>

<script>
import NavigationBar from '@/components/NavigationBar.vue';


export default {
  components: {
    NavigationBar
    
  },
}
</script>
