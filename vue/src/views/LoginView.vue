<template>
  <div class="divClass" id="login">
    <img class="image" src="img/logo2.png" />
    <form class="log-in" v-on:submit.prevent="login">
      <h1>Please Sign In</h1>
      <div role="alert" v-if="invalidCredentials">
        Invalid username and password!
      </div>
      <div role="alert" v-if="this.$route.query.registration">
        Thank you for registering, please sign in.
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input type="text" id="username" v-model="user.username" required autofocus />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="user.password" required />
      </div>
      <button type="submit">Sign in</button>
      <p>
        <router-link v-bind:to="{ name: 'register' }">Need an account? Sign up.</router-link>
      </p>
    </form>
    <register-suggestion-Comp></register-suggestion-Comp>
  </div>
  
</template>

<script>
import authService from "../services/AuthService";
import RegisterSuggestionComp from "../components/RegisterSuggestionComp.vue";


export default {
  components: { 
    RegisterSuggestionComp

   },
  data() {
    return {
      user: {
        username: "",
        password: ""
      },
      invalidCredentials: false
    };
  },
  methods: {
    login() {
      authService
        .login(this.user)
        .then(response => {
          if (response.status == 200) {
            this.$store.commit("SET_AUTH_TOKEN", response.data.token);
            this.$store.commit("SET_USER", response.data.user);
            this.$router.push("/");
          }
        })
        .catch(error => {
          const response = error.response;

          if (response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    }
  }
};
</script>

<style scoped>
.form-input-group {
  margin-bottom: 1rem;
}

label {
  margin-right: 0.5rem;
}

.image {
  left: 0%;
  width: 350px;
  height: 350px;
  margin-left: 0%;

}

.log-in {
  margin-left: 1%;
}
.divClass{
  display: flex;
  align-items: center;
  justify-content: start;
}

</style>