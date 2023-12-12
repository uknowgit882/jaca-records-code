<template>
    <!-- <login-component /> -->
    <register-component />
    <navbar-comp />
</template>

<script>
// import LoginComponent from "./LoginComponent.vue";
import RegisterComponent from './RegisterComponent.vue';
import NavbarComp from "./NavbarComp.vue";

export default{
    components: {
        // LoginComponent,
        RegisterComponent,
        NavbarComp
    }
}

</script>