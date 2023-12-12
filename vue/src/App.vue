<template>
  <div id="capstone-app">
    <!-- <NavigationBar/> -->
    <nav-bar-comp />
    <!-- <div id="nav"> -->
      <!-- <router-link v-bind:to="{ name: 'home' }">Home</router-link>&nbsp;|&nbsp; -->
      <!-- <router-link v-bind:to="{ name: 'logout' }" v-if="$store.state.token != ''">Logout</router-link>
    </div> -->

    <router-view />
  </div>
</template>

<script>
// import NavigationBar from '@/components/NavigationBar.vue';
import NavBarComp from './components/NavBarComp.vue';

export default {
  components: {
    // NavigationBar,
    NavBarComp
  },
}
</script>
