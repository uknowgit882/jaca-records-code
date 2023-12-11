<template>
    <div>
        <carosuel-home-a-comp></carosuel-home-a-comp>

    </div>
    <div>
        <register-suggestion-comp></register-suggestion-comp>

    </div>

</template>

<script>
import RegisterSuggestionComp from '../../components/RegisterSuggestionComp.vue';
import CarosuelHomeAComp from '../../components/CarosuelHomeAComp.vue';

export default {
    components: {
        RegisterSuggestionComp,
        CarosuelHomeAComp
    }
}
</script>

<style scoped>


</style>