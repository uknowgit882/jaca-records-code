<template>
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
        <login-component></login-component>
        <h2 class="signup-text">&nbsp;&nbsp;Need an account? Sign up.</h2>
        <button
          class="button2 is-warning"
          id="premiumButton"
          @click="RegisterPagePush()"
        >
          Register
        </button>
        <!-- <img class="image is-50x50" src="img/logo2.png" style="max-height: none;"/> -->
      </div>
    </div>
  </div>
</template>

<script>
import LoginComponent from "./LoginComponent.vue";

export default {
  components: {
    LoginComponent,
  },
  data() {
        return {
            dropdownActive: false
        }
    },
  methods: {
    RegisterPagePush() {
      this.$router.push({ name: "register" });
    },
  },
};
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