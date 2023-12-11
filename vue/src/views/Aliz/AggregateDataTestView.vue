<template>
 <aggregate-data-left-components></aggregate-data-left-components>
 <aggregate-data-right-components v-if="$store.state.token != ''"/>
</template>

<script>

import StatisticsService from '@/services/StatisticsService.js'
import AggregateDataLeftComponents from '@/components/AggregateDataLeftComponents.vue'
import AggregateDataRightComponents from '../../components/AggregateDataRightComponents.vue'

export default {
    components: {
        AggregateDataLeftComponents,
        AggregateDataRightComponents
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
