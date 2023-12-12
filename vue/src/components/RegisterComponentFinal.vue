<template>
    <div id="register" class="text-center">
        <form v-on:submit.prevent="register">
            <h1>Create Account</h1>
            <div role="alert" v-if="registrationErrors">
                {{ registrationErrorMsg }}
            </div>
            <div class="form-input-group">
                <label for="firstName">First Name</label>
                <input type="text" id="firstName" v-model="user.firstName" required autofocus />
            </div>
            <div class="form-input-group">
                <label for="lastName">Last Name</label>
                <input type="text" id="lastName" v-model="user.lastName" required />
            </div>
            <div class="form-input-group">
                <label for="email">Email</label>
                <input type="text" id="email" v-model="user.email" required />
            </div>
            <div class="form-input-group">
                <label for="username">Username</label>
                <input type="text" id="username" v-model="user.username" required />
            </div>
            <div class="form-input-group">
                <label for="password">Password</label>
                <input type="password" id="password" v-model="user.password" required />
            </div>
            <div class="form-input-group">
                <label for="confirmPassword">Confirm Password</label>
                <input type="password" id="confirmPassword" v-model="user.confirmPassword" required />
            </div>
            <div class="form-input-group2">
                <label for="userRole">Account Type</label>
                <select v-model="user.role">
                    <option disabled value="">Please select one</option>
                    <option>Free</option>
                    <option>Premium</option>
                </select>
            </div>
            <button class="button2" type="submit">Create Account</button>
            <!-- <p><router-link v-bind:to="{ name: 'login' }">Already have an account? Log in.</router-link></p> -->
        </form>
    </div>
</template>

<script>
import authService from '@/services/AuthService';

export default {
    data() {
        return {
            user: {
                firstName: '',
                lastName: '',
                email: '',
                username: '',
                password: '',
                confirmPassword: '',
                role: '',
            },
            registrationErrors: false,
            registrationErrorMsg: 'There were problems registering this user.',
        };
    },
    methods: {
        register() {
            if (this.user.password != this.user.confirmPassword) {
                this.registrationErrors = true;
                this.registrationErrorMsg = 'Password & Confirm Password do not match.';
            } else {
                authService
                    .register(this.user)
                    .then((response) => {
                        if (response.status == 201) {
                            this.$router.push({
                                path: '/login',
                                query: { registration: 'success' },
                            });
                        }
                    })
                    .catch((error) => {
                        const response = error.response;
                        this.registrationErrors = true;
                        if (response.status === 400) {
                            this.registrationErrorMsg = 'Bad Request: Validation Errors';
                        }
                    });
            }
        },
        clearErrors() {
            this.registrationErrors = false;
            this.registrationErrorMsg = 'There were problems registering this user.';
        },
    },
};

</script>

<style scoped>
.form-input-group {
    margin-bottom: 1rem;
    color: white;
}

.form-input-group2 {
    color: white;
}

#email {
    padding-left: 39px;
}

#username {
    padding-left: 5px;
}
#password {
    padding-right: 10px;
}
label {
    margin-right: 0.5rem;
}
.button2 {
    --color: white;
    font-family: inherit;
    display: inline-block;
    width: 15.3em;
    height: 2.6em;
    line-height: 1.5em;
    margin: 10px;
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
    background-color: #05B5A1;
}

.button2:active:before {
    background: #B856AB;
    transition: background 0s;
}
</style>
