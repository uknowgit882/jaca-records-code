<template>
  <div
    class="dropdown"
    :class="{ 'is-active': dropdownActive }"
    v-if="this.$store.state.token == ''"
  >
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
      </div>
    </div>
  </div>

  <div class="dropdown" :class="{ 'is-active': dropdownActive }" v-else>
    <div class="dropdown-trigger" @click="dropdownActive = !dropdownActive">
      <button class="button" aria-haspopup="true" aria-controls="dropdown-menu">
        <span>
          <strong><i class="fa-solid fa-circle-user"></i></strong>
        </span>
        <span class="icon is-small">
          <i class="fas fa-angle-down" aria-hidden="true"></i>
        </span>
      </button>
    </div>
    <div class="dropdown-menu" id="dropdown-menu" role="menu">
      <div class="dropdown-content">
        <button
          class="button2 is-warning"
          id="premiumButton"
          @click="UpgradeUser()"
        >
          Upgrade Account
        </button>
        <button
          class="button2 is-warning"
          id="premiumButton"
          @click="DowngradeUser()"
        >
          Downgrade Account
        </button>
        <button
          class="button2 is-warning"
          id="premiumButton"
          @click="DeactivateUser()"
        >
          Deactivate Account
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import LoginComponent from "./LoginComponent.vue";
import UserFunctionsService from "@/services/UserFunctionsService.js";

export default {
  components: {
    LoginComponent,
  },
  data() {
    return {
      dropdownActive: false,
    };
  },
  methods: {
    RegisterPagePush() {
      this.$router.push({ name: "register" });
    },
    UpgradeUser() {
      UserFunctionsService.upgradeUser()
        .then((response) => {
          if (response.status == 200) {
            this.$store.commit("CHANGE_USER", this.user);
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "User Upgrade");
        });
    },
    DowngradeUser() {
      UserFunctionsService.downgradeUser()
        .then((response) => {
          if (response.status == 200) {
            this.$store.commit("CHANGE_USER", this.user);
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "User Downgrade");
        });
    },
    DeactivateUser() {
      UserFunctionsService.deactivateUser()
        .then((response) => {
          if (response.status == 200) {
            this.$store.commit("CHANGE_USER", this.user);
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "User Deactivate");
        });
    },
    handleErrorResponse(error, verb) {
      if (error.response) {
        console.log(
          `Error ${verb} topic. Response received was "${error.response.statusText}".`
        );
      } else if (error.request) {
        console.log(`Error ${verb} topic. Server could not be reached.`);
      } else {
        console.log(`Error ${verb} topic. Request could not be created.`);
      }
    },
  },
};
</script>

<style scoped>
.dropdown-content {
  background-color: #4d437f;
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
  transition: color 0.5s;
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
  transition: all 0.7s;
}

.button2:hover:before {
  top: -30px;
  left: -30px;
  background-color: #d1d301;
}

.button2:active:before {
  background: #4d437f;
  transition: background 0s;
}
</style>