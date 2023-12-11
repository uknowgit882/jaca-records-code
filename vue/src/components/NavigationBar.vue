<template>
    <nav class="navbar is-dark" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
            <!-- <p class="profile">
                <button class="button" href="/Profile">
                    <strong><i class="fa-solid fa-circle-user" ></i></strong>
                </button>
            </p> -->
            <a role="button" class="navbar-burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
                <span aria-hidden="true"></span>
                <span aria-hidden="true"></span>
                <span aria-hidden="true"></span>
            </a>
        </div>
        <!-- puts items in the start of the navbar -->
        <div id="navbarBasicExample" class="navbar-menu">
            <div class="navbar-start">
                <div class="navbar-item">
                    <!-- <div class="buttons">
                        <a class="button is-primary" href="/register">
                            <strong>Sign up</strong>
                        </a> -->
                    <div class="dropdown" :class="{ 'is-active': dropdownActive }">
                        <div class="dropdown-trigger" @click="dropdownActive = !dropdownActive">
                            <button class="button" aria-haspopup="true" aria-controls="dropdown-menu">
                                <span>Log in</span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" id="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <login-view></login-view>
                                <h2 class="signup-text">&nbsp;&nbsp;Need an account? Sign up. </h2>
                                <button class="button2 is-warning" id="premiumButton">Register</button>
                                <!-- <img class="image is-50x50" src="img/logo2.png" style="max-height: none;"/> -->
                            </div>
                        </div>
                    </div>

                    <!-- <a class="button is-light" href="/login">
                            Log in
                        </a> -->
                    <!-- </div> -->
                </div>
                <div class="navbar-start">
                    <router-link class="navbar-item" :to="{ name: 'home' }">
                        Home
                    </router-link>

                    <a class="navbar-item" href="/library">
                        Library
                    </a>

                    <a class="navbar-item" href="/collections">
                        Collections
                    </a>
                    <a class="navbar-item" href="/data">
                        Data
                    </a>

                </div>
            </div>

            <SearchBoxVue v-if="this.$store.state.token != ''" />
        </div>
    </nav>
</template>

<script>
import SearchBoxVue from '@/components/SearchBox.vue'
import LoginView from '../views/LoginView.vue';
export default {
    components: {
        SearchBoxVue,
        LoginView
    },
    data() {
        return {
            dropdownActive: false
        }
    }
}
</script>


<style scoped>

.dropdown-content {
    background-color: #4D437F;
    margin-top: 7px;
    padding-top: 1px;
    padding-bottom: 22px;
}

.signup-text {
    color: white;
    text-align: center;
}

.navbar {
    position: fixed;
    top: 0px;
    width: 100%;
    padding-right: 1%;
    background-color: #4D437F;
}

.search {

    width: 200%;
}

.input {
    /* TODO: how to make search bar longer without using pxs ( want to adjust correctly to every screen) */
}

.profile {
    margin-top: 15%;
    margin-left: 10%;
}

h2 {
    color: black;
}


.button2 {
    --color: white;
    font-family: inherit;
    display: inline-block;
    width: 15.3em;
    height: 2.6em;
    line-height: 1.5em;
    margin: 20px;
    position: relative;
    overflow: hidden;
    border: 2px solid var(--color);
    transition: color .5s;
    z-index: 1;
    font-size: 17px;
    border-radius: 8px;
    font-weight: 500;
    color: var(--color);
    background-color: black;

}

.button2:before {
    content: "";
    position: absolute;
    z-index: -1;
    background: var(--color);
    height: 115px;
    width: 500px;
    border-radius: 50%;
}

.button2:hover {
    color: black;
}

.button2:before {
    top: 100%;
    left: 100%;
    transition: all .7s;
}

.button2:hover:before {
    top: -30px;
    left: -30px;
    background-color: #D1D301;
}

.button2:active:before {
    background: #4D437F;
    transition: background 0s;
}
</style>