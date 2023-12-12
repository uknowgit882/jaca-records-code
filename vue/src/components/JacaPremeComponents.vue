<template>
  <input class="input-users" type="text" placeholder="Enter Username" v-model="User.username">

  <button @click="openup">Reactivate</button>
  <div v-if="showPopup" class="popup">
    <div class="popup-inner">
    </div>
  </div>
  <div v-if="showPopup" class="popup">
    <div class="popup-inner">
      <h2>You are about to reactivate user : {{ User.username }} </h2>
      <button class="button-reactivate" @click="reactivateUser()">Reactivate</button>
      <button class="ExitButton">Exit</button>
    </div>
  </div>
  <button class="button-upgrade" @click="upgradeUser()">Upgrade</button>
  <button class="button-downgrade" @click="downgradeUser()">Downgrade</button>
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
      showPopup: false
    }
  },
  methods: {
    openup(){
      this.showPopup = !this.showPopup
    },
    reactivateUser() {
      JacapremeService.reactivateUser(this.User.username)
        .then(response => {
          if (response.status == 200) {
            console.log(`${this.User.username} was reactivated.`)
           
          }

        })
    },
    upgradeUser() {
      JacapremeService.upgradeUserToAdmin(this.User.username)
        .then(response => {
          if (response.status == 200) {
            console.log(`${this.User.username} was upgraded.`)
          }
        })
    },
    downgradeUser() {
      JacapremeService.downgradeUserFromAdmin(this.User.username)
        .then(response => {
          if (response.status == 200) {
            console.log(`${this.User.username} was upgraded.`)
          }
        })
    },
    alert() {
      this.alert('Your message')
        .then(() => {
          console.log('Alert');
        })
    }


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
  background-color: #3498db;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
h2{
  color: black
}
</style>