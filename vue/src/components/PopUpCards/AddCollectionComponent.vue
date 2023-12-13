<template>
    <div v-if="errorPopup">
        <errorPopup v-bind:errorMessage="this.errorMessage" @click="errorPopup = !errorPopup"></errorPopup>
    </div>
    <div v-if="canSee" class="addCollection-popup">
        <div class="addCollection-popup-inner">
            <button class="popup-exit-button" @click="showAddCollectionToParent">X</button>
            <div class="addCollection-popup-inner-inner">
                <p
                    style="color: white; font-weight: bold; text-align: center; text-decoration: underline; margin: 12px; margin-top: 12px;">
                    Create a collection</p>
                <form style="text-align: left; margin: 12px;">
                    <div>
                        <label for="newCollection" style="color: white; font-weight: bold; margin: 12px;">Name: </label>
                        <input class="addCollection-popup-inner-inputs" id="newCollection" type="text" name="newCollection"
                            placeholder="Your new collection name..." v-model="newCollectionToAdd.name" style="margin-top: 12px; margin-bottom: 12px; width: 100%; height: 30px;" />
                    </div>
                    <div>
                        <label for="isPrivate" style="color: white; font-weight: bold; margin: 12px; display: inline;">Make Private: </label>
                        <input class="addCollection-popup-inner-inputs" id="isPrivate" type="checkbox" name="isPrivate"
                            v-model="newCollectionToAdd.is_Private" style="display: inline;"/>
                    </div>
                    <div>
                        <button class="addCollection-popup-button" @click.prevent="createCollection">Create</button>
                    </div>
                    <p v-if="weAreOnIt">We're on it!</p>
                    <p v-if="actionSuccessful">Success! Refreshing the page...</p>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
import ErrorPopup from './ErrorPopup.vue';
import CollectionsService from '../../services/CollectionsService';
export default {
    data() {
        return {
            newCollectionToAdd: {
                name: "",
                is_Private: false
            },
            errorPopup: false,
            errorMessage: "",
            weAreOnIt: false,
            canSee: this.isVisible
        }
    },
    components: {
        ErrorPopup
    },
    props: ['isVisible'],
    methods: {
        successDisappearer() {
            this.actionSuccessful = false;
            setTimeout(this.$router.go(), 2000)
            return this.actionSuccessful;
        },
        displaySuccess() {
            this.actionSuccessful = true;
            setTimeout(this.successDisappearer, 2000)
        },
        showAddCollectionToParent(response){
            this.canSee = false;
            this.$emit('showAddCollectionToParent', false)
        },
        createCollection() {
            this.weAreOnIt = true;
            CollectionsService.AddNewCollection(this.newCollectionToAdd)
                .then(response => {
                    this.weAreOnIt = false;
                    this.displaySuccess();
                })
                .catch(error => {
                    this.weAreOnIt = false;
                    this.errorMessage = `create ${this.newCollectionToAdd.name}`;
                    this.errorPopup = true;
                })
        },
    }

}
</script>


<style>
.addCollection-popup {
    border-radius: 5px;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.75);
    display: flex;
    align-items: center;
    justify-content: center;
    word-wrap: break-word;
}

.addCollection-popup-inner scoped {
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    background: black;
    opacity: 1.00;
    padding: 10px;
    text-align: center;
    height: 100%;
    width: 100%;
}

.addCollection-popup-inner-inner {
    border-radius: 5px;
    background: black;
    text-align: center;
    height: 250px;
    width: 300px;
}

.addCollection-popup-inner button {
    border-radius: 5px;
    margin-top: 16px;
    padding: 8px 12px;
    background-color: #EA5143;
    color: #fff;
    border: none;
    text-align: center;
    border-radius: 4px;
    cursor: pointer;
    float: right;
}

.addCollection-popup-inner-inputs {
    border-radius: 5px;
    box-sizing: border-box;
    background-color: black;
    color: white;
}

.addCollection-popup-button {
    grid-area: button;
    margin-top: 8px;
    padding: 8px;
    background-color: #EA5143;
    min-width: 100%;
    color: #fff;
    text-align: center;
    border-radius: 4px;
    cursor: pointer;
}
</style>