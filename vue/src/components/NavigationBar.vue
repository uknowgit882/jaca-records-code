<template>
    <nav class="navbar is-dark" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
            <!-- <a class="navbar-item" href="">
                <img src="" width="112" height="28">
            </a> -->
            <p class="profile">
                        <button class="button" href="/Profile">
                            <strong><i class="fa-solid fa-circle-user"></i></strong>
                        </button>
                    </p>
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
                    <div class="buttons">
                        <a class="button is-primary" href="/register">
                            <strong>Sign up</strong>
                        </a>
                        <a class="button is-light" href="/login">
                            Log in
                        </a>
                    </div>
                </div>
                <div class="navbar-start">
                    <a class="navbar-item" href="/home">
                        Home
                    </a>

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

          
        </div>

    </nav>
</template>

<script>
import SearchBoxVue from '@/components/SearchBox.vue'
export default {
    components: {
        SearchBoxVue
    }
}
</script>


<style scoped>
.navbar {
    position: fixed;
    top: 0px;
    width: 100%;
    padding-right: 1%;
}

.search {

    width: 200%;
}
.input{
    /* TODO: how to make search bar longer without using pxs ( want to adjust correctly to every screen) */
}
.profile{
margin-top: 15%;
margin-left: 10%;
}
</style>