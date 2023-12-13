<template>
  <div class="dropdown" :class="{ 'is-active': dropdownActive }" v-if="this.$store.state.token == ''">
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
        <login-comp></login-comp>
        <h2 class="signup-text">&nbsp;&nbsp;Need an account? Sign up.</h2>
        <a class="button2" href="/register" id="premiumButton">
          Register
        </a>
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
    <!-- <div class="dropdown-menu" id="dropdown-menu" role="menu">
      <div class="dropdown-content">
        <button class="button2 is-warning" id="premiumButton" @click="UpgradeUser()">
          Upgrade Account
        </button>
        <button class="button2 is-warning" id="premiumButton" @click="DowngradeUser()">
          Downgrade Account
        </button>
        <button class="button2 is-warning" id="premiumButton" @click="DeactivateUser()">
          Deactivate Account
        </button>
      </div> -->
    <div class="dropdown-menu" id="dropdown-menu" role="menu">
      <div class="dropdown-content">
        <button class="button2 is-warning" id="premiumButton" @click="UpgradeUser()">
          Upgrade Account
        </button>
        <button class="button2 is-warning" id="premiumButton" @click="DowngradeUser()">
          Downgrade Account
        </button>
        <button class="button2 is-warning" id="premiumButton" @click="DeactivateUser()">
          Deactivate Account
        </button>
        <button class="button3" v-on:click="logout()">Logout</button>
        <div class="latestLogin">Last Login: {{ this.$store.state.user.last_Login }}</div>
      </div>
    </div>
  </div>
  <!-- </div> -->
</template>
  
<script>
import LoginComp from "@/components/LoginComp.vue";
import UserFunctionsService from "@/services/UserFunctionsService.js";


export default {
  components: {
    LoginComp,
  },
  data() {
    return {
      dropdownActive: false,
    };
  },
  // methods: {
  //   RegisterPagePush() {
  //     this.$router.push({ name: "register" });
  //   },
  //   UpgradeUser() {
  //     UserFunctionsService.upgradeUser()
  //       .then((response) => {
  //         if (response.status == 200) {
  //           this.$store.commit("CHANGE_USER", this.user);
  //         }
  //       })
  //       .catch((error) => {
  //         this.handleErrorResponse(error, "User Upgrade");
  //       });
  //   },
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
    logout() {
      this.$store.commit('LOGOUT');
      this.$router.push({ name: "home" });
    },
  },
  // DeactivateUser() {
  //   UserFunctionsService.deactivateUser()
  //     .then((response) => {
  //       if (response.status == 200) {
  //         this.$store.commit("CHANGE_USER", this.user);
  //       }
  //     })
  //     .catch((error) => {
  //       this.handleErrorResponse(error, "User Deactivate");
  //     });
  // },
  // handleErrorResponse(error, verb) {
  //   if (error.response) {
  //     console.log(
  //       `Error ${verb} topic. Response received was "${error.response.statusText}".`
  //     );
  //   } else if (error.request) {
  //     console.log(`Error ${verb} topic. Server could not be reached.`);
  //   } else {
  //     console.log(`Error ${verb} topic. Request could not be created.`);
  //   }
  // },

};
</script>
  
<style scoped>
.dropdown-content {
  background-color: rgba(0, 0, 0, 0.75);
  margin-top: 8px;
  padding-top: 1px;
  padding-bottom: 10px;
}

.signup-text {
  color: white;
  text-align: center;
  padding-left: 0px;
}

.button2 {
  --color: white;
  font-family: inherit;
  display: inline-block;
  width: 15.3em;
  height: 2.6em;
  line-height: 2.5em;
  margin: 10px;
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
  background-color: #EEE810;
}

.button2:active:before {
  background: #B856AB;
  transition: background 0s;
}

.button3 {
  --color: white;
  font-family: inherit;
  display: inline-block;
  width: 15.3em;
  height: 2.6em;
  line-height: 2.5em;
  margin: 10px;
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

.button3:before {
  content: "";
  position: absolute;
  z-index: -1;
  background: var(--color);
  height: 115px;
  width: 500px;
  border-radius: 50%;
}

.button3:hover {
  color: black;
}

.button3:before {
  top: 100%;
  left: -100%;
  transition: all 0.7s;
}

.button3:hover:before {
  top: -30px;
  left: -30px;
  background-color: #FF3938;
}

.button3:active:before {
  background: #B856AB;
  transition: background 0s;
}
</style>