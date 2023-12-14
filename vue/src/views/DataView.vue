<template>
    <aggregate-data-table-components/>
   </template>
   
   <script>
   
   import StatisticsService from '@/services/StatisticsService.js'
   import AggregateDataTableComponents from '../components/AggregateDataTableComponents.vue'
   
   export default {
       components: {
           AggregateDataTableComponents
       },
       methods: {
           getAggregateForEveryone() {
               StatisticsService.getAggregateStats()
                   .then(response => {
                       this.$store.commit('ADD_AGGREGATE_STATS', response.data)
                   })
           },
           getAggregateForUsers(){
               StatisticsService.getUserStats()
               .then(response =>{
                   this.$store.commit('ADD_USER_STATS', response.data)
               })
           }   
       },
       created() {
           this.getAggregateForEveryone(),
           this.getAggregateForUsers()
       },
   }
   </script>
   
   
   <style scoped>
   </style>