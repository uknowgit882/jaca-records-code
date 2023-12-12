<template>
  <input class="input-users" type="text" placeholder="Enter Username"
  v-model="User.username">
  <button class="button-reactivate" @click="reactivateUser()">Reactivate</button>
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
    }
  },
  methods: {
    reactivateUser() {
      JacapremeService.reactivateUser(this.User.username)
        .then(response => {
          if (response.status == 200) {
            console.log(`${this.User.username} was reactivated.`)
          }

        })
    },
    upgradeUser(){
      JacapremeService.upgradeUserToAdmin(this.User.username)
      .then(response => {
        if (response.status == 200){
          console.log(`${this.User.username} was upgraded.`)
        }
      })
    },
    downgradeUser(){
      JacapremeService.downgradeUserFromAdmin(this.User.username)
      .then(response => {
        if (response.status == 200){
          console.log(`${this.User.username} was upgraded.`)
        }
      })
    },
    alert(){
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
</style>