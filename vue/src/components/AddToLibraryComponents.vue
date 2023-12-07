<template>
<button class="button is-rounded" v-on:click="clickedAdd()">Add to Library</button>
</template>

<script>

import AddToLibraryService from '@/services/AddToLibraryService.js'

export default {
    props:{
        id: {
            type: Number,
            required: true,
        }
    },
    data(){
        return{
            Records: {
                General: "",
                Artist: "",
                Title: "",
                Genre: "",
                Year: "",
                Country: "",
                Label: "",
                Barcode: ""
            }
        }
    },
    methods: {
        clickedAdd() {
           AddToLibraryService.addToLibrary(this.id)
           .then((response) => {
            if(response.status == 201){
                this.$store.commit('ADD_RECORDS_TO_LIBRARY', response);
                this.$router.push({name: "Library"})

            }

           })
        //    .catch((error) => {
        //             this.handleErrorResponse(error, 'Search Query')
        //         })
        }
    }
}
</script>
