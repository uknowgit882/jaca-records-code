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
        <h3>Welcome, {{ this.$store.state.user.username }}!</h3>
        <!-- <h3> Your total records: {{ this.$store.state.StatsRecords }}</h3> -->

        <button class="button2" @click="openup">Upgrade Account</button>
        <div v-if="showPopup" class="popup">
          <div class="popup-inner">
          </div>
        </div>
        <div v-if="showPopup" class="popup">
          <div class="popup-inner">
            <div v-if="message == ''">
              <h2>You are about to upgrade your account.
                Click upgrade button if you want to proceed, else click exit button.</h2>
              <button class="button2"  @click="UpgradeUser()">
                Upgrade Account</button>
              <button class="ExitButton" @click="openup">Exit</button>
            </div>
            <div v-else>
              <h2 v-show="this.message"> {{ message }}</h2>
              <button class="ExitButton" @click="openup">Exit</button>
            </div>
          </div>
          </div>


          <button class="button2" @click="openup2">Downgrade Account</button>
        <div v-if="showPopup2" class="popup">
          <div class="popup-inner">
          </div>
        </div>
        <div v-if="showPopup2" class="popup">
          <div class="popup-inner">
            <div v-if="message2 == ''">
              <h2>You are about to downgrade your account.
                Click downgrade button if you want to proceed, else click exit button.</h2>
              <button class="button2" @click="DowngradeUser()">
                Downgrade Account</button>
              <button class="ExitButton" @click="openup2">Exit</button>
            </div>
            <div v-else>
              <h2 v-show="this.message2"> {{ message2 }}</h2>
              <button class="ExitButton" @click="openup2">Exit</button>
            </div>
          </div>
          </div>


          <button class="button2" @click="openup3">Deactivate Account</button>
        <div v-if="showPopup3" class="popup">
          <div class="popup-inner">
          </div>
        </div>
        <div v-if="showPopup3" class="popup">
          <div class="popup-inner">
            <div v-if="message3 == ''">
              <h2>You are about to deactivate your account.
                Click deactivate button if you want to proceed, else click exit button.</h2>
              <button class="button2" @click="DeactivateUser()">
                Deactivate Account</button>
              <button class="ExitButton" @click="openup3">Exit</button>
            </div>
            <div v-else>
              <h2  v-show="this.message3"> {{ message3 }}</h2>
              <button class="ExitButton" @click="openup3">Exit</button>
            </div>
          </div>
          </div>



          <button class="button2" @click="openup4">Logout</button>
        <div v-if="showPopup4" class="popup">
          <div class="popup-inner">
          </div>
        </div>
        <div v-if="showPopup4" class="popup">
          <div class="popup-inner">
            <div v-if="message4 == ''">
              <h2>You are about to logout of your account.
                Click logout button if you want to proceed, else click exit button.</h2>
              <button class="button2" @click="logout()">
                Logout</button>
              <button class="ExitButton" @click="openup4">Exit</button>
            </div>
            <div v-else>
              <h2  v-show="this.message4"> {{ message4 }}</h2>
              <button class="ExitButton" @click="openup4">Exit</button>
            </div>
          </div>
          </div>
          <!-- <button class="button2 is-warning" id="premiumButton" @click="DeactivateUser()">
            Deactivate Account
          </button> -->

          <!-- <button class="button2" v-on:click="logout()">Logout</button> -->
          <div class="latestLogin">Last Login: {{ this.$store.state.user.last_Login.substring(0, 19) }}</div>
        </div>
      </div>
    </div>
    <!-- </div> -->
</template>
  
<script>
import LoginComp from "@/components/LoginComp.vue";
import UserFunctionsService from "@/services/UserFunctionsService.js";
import StatisticsService from "../services/StatisticsService";


export default {
  components: {
    LoginComp,
  },
  data() {
    return {
      dropdownActive: false,
      showPopup: false,
      message: "",
      showPopup2: false,
      message2: "",
      showPopup3: false,
      message3: "",
      showPopup4: false,
      message4: "",
      totalRecords: []
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
      this.message == '';
      UserFunctionsService.upgradeUser()
        .then((response) => {
          if (response.status == 200) {
            this.message = "You have successfully upgraded your account."
            // this.$store.commit("CHANGE_USER", this.user);
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
            this.message2 = "You have successfully downgraded your account."
            // this.$store.commit("CHANGE_USER", this.user);
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
            // this.$store.commit("CHANGE_USER", this.user);
            this.message3 = "You have successfully deactivated your account."
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "User Deactivate");
        });
    },
    TotalRecords(){
      StatisticsService.getTotalRecords()
      .then((response) => {
        if (response.status == 200){
          this.$store.commit("GET_TOTAL_RECORDS",response.data)
        }
      })
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
    openup() {
      this.showPopup = !this.showPopup
    },
    openup2() {
      this.showPopup2 = !this.showPopup2
    },
    openup3() {
      this.showPopup3 = !this.showPopup3
    },
    openup4() {
      this.showPopup4 = !this.showPopup4
    },
  },
  created (){
    this.TotalRecords();
  }
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
  padding-top: 10px;
  padding-bottom: 10px;
}

.signup-text {
  color: white;
  text-align: center;
  padding-left: 0px;
}

.button {
  color: white;
  background-color: black;
}
.button2,.ExitButton {
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

.button2:before , .ExitButton:before {
  content: "";
  position: absolute;
  z-index: -1;
  background: var(--color);
  height: 115px;
  width: 500px;
  border-radius: 50%;
}

.button2:hover , .ExitButton:hover {
  color: black;
}

.button2:before, .ExitButton:before {
  top: 100%;
  left: 100%;
  transition: all 0.7s;
}

.button2:hover:before, .ExitButton:hover:before {
  top: -30px;
  left: -30px;
  background-color: #EEE810;
}

.button2:active:before, .ExitButton:active:before {
  background: #B856AB;
  transition: background 0s;
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
  left: -100%;
  transition: all 0.7s;
}

.button2:hover:before {
  top: -30px;
  left: -30px;
  background-color: #FF3938;
}

.button2:active:before {
  background: #B856AB;
  transition: background 0s;
}

h2{
  color:black
}

.button2, .ExitButton{
  justify-content: center;
  align-content: center;
}
.popup {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 99;
  background-color: rgba(0, 0, 0, 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
}

.popup-inner {
  background: #fff;
  padding: 32px;
  text-align: center;
}

.popup-inner button{
  margin-top: 16px;
  padding: 2px 16px;
  background-color: #000000;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

</style>