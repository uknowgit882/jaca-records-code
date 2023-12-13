<template>

  <div class="box container is-fullheight is-flex is-align-items-center">
    <img src="img/logo2.png"/>
    <box class="box-jaca">
      <input class="input-users" type="text" placeholder="Enter Username" v-model="User.username">

      <button class="commands" @click="openup">Reactivate</button>
      <div v-if="showPopup" class="popup">
        <div class="popup-inner">
        </div>
      </div>
      <div v-if="showPopup" class="popup">
        <div class="popup-inner">
          <h2>You are about to reactivate user : {{ User.username }} </h2>
          <button class="button" @click="reactivateUser()">Reactivate</button>
          <h2 @click="reactivateUser()" v-if="message"> {{ message }}</h2>
          <button class="ExitButton" @click="openup">Exit</button>
        </div>
      </div>

      <button class="commands" @click="openup2">Upgrade</button>
      <div v-if="showPopup2" class="popup">
        <div class="popup-inner">
        </div>
      </div>
      <div v-if="showPopup2" class="popup">
        <div class="popup-inner">
          <h2>You are about to upgrade user : {{ User.username }} </h2>
          <button class="button" @click="upgradeUser()">Upgrade</button>
          <h2 @click="upgradeUser()" v-if="message2"> {{ message2 }}</h2>
          <button class="ExitButton" @click="openup2">Exit</button>
        </div>
      </div>

      <button class="commands" @click="openup3">Downgrade</button>
      <div v-if="showPopup3" class="popup">
        <div class="popup-inner">
        </div>
      </div>
      <div v-if="showPopup3" class="popup">
        <div class="popup-inner">
          <h2>You are about to downgrade user : {{ User.username }} </h2>
          <button class="button" @click="downgradeUser()">Downgrade</button>
          <h2 @click="downgradeUser()" v-if="message3"> {{ message3 }}</h2>
          <button class="ExitButton" @click="openup3">Exit</button>
        </div>
      </div>
    </box>
  </div>
</template>

<script>
import JacapremeService from '@/services/JacapremeService.js'
import PopUpComponent from '@/components/PopUpComponent.vue';

export default {
  components: {

  },
  data() {
    return {
      User: {
        username: "",
      },
      showPopup: false,
      showPopup2: false,
      showPopup3: false,
      message: "",
      message2: "",
      message3: "",
    }
  },
  methods: {
    openup() {
      this.showPopup = !this.showPopup
    },
    openup2() {
      this.showPopup2 = !this.showPopup2
    },
    openup3() {
      this.showPopup3 = !this.showPopup3
    },
    reactivateUser() {
      JacapremeService.reactivateUser(this.User.username)
        .then(response => {
          if (response.status == 200) {
            this.message = `You have successfully reactivated ${this.User.username}`;

          }

        })
        .catch((error) => {
          this.handleErrorResponse(error, "Reactivate User")
        })
    },
    upgradeUser() {
      JacapremeService.upgradeUserToAdmin(this.User.username)
        .then(response => {
          if (response.status == 200) {
            this.message2 = `You have successfully upgraded ${this.User.username}`;
          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Upgrade User")
        })
    },
    downgradeUser() {
      JacapremeService.downgradeUserFromAdmin(this.User.username)
        .then(response => {
          if (response.status == 200) {
            this.message3 = `You have successfully downgraded ${this.User.username}`;

          }
        })
        .catch((error) => {
          this.handleErrorResponse(error, "Downgrade User")
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


  }
}

</script>

<style scoped>
.input-users {
  width: 60%;
  height: 40px;
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

.popup-inner button {
  margin-top: 16px;
  padding: 8px 16px;
  background-color: #000000;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

h2 {
  color: black
}

.button,
.ExitButton,
.commands {
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

.button:before,
.ExitButton:before,
.commands:before {
  content: "";
  position: absolute;
  z-index: -1;
  background: var(--color);
  height: 115px;
  width: 500px;
  border-radius: 50%;
}

.button:hover,
.ExitButton:hover,
.commands:hover {
  color: black;
}

.button:before,
.ExitButton:before,
.commands:before {
  top: 100%;
  left: 100%;
  transition: all .7s;
}

.button:hover:before,
.ExitButton:hover:before,
.commands:hover:before {
  top: -30px;
  left: -30px;
  background-color: #EEE810;
}

.button:active:before,
.ExitButton:active:before,
.commands:active:before {
  background: #B856AB;
  transition: background 0s;
}


.box{
  display: flex;
    width: 750px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(#07B6A0, #c5e4dd);
    border-radius: 10px;
    box-shadow: rgba(0, 0, 0, 0.35) 0px -50px 36px -28px inset;
    transition: transform 0.3s ease;
    top: 300px;
    
}


.input-users{
  height: 40px;
  width: 300px;
  background: black;
  color: white
}

img {
  width: 300px;
  height: 300px;
}

</style>