<template>
    <div class="Result-table" >
        <img class="result-thumbnail" v-bind:src="result.thumb"  /> 
        <span class="result-artist">{{ result.title }}</span>
        <span class="result-year">{{ result.year }}</span>
        <ul class ="result-genre">
            <li v-for="item in result.genre" v-bind:key="item">{{ item }}</li>
        </ul>   
        <span class ="result-country">{{ result.country }}</span>
        <ul class ="result-label">
            <li v-for="item in result.label" v-bind:key="item">{{ item }}</li>
        </ul>   
        
        <!-- <span class="result-barcode">{{ result.barcode }}</span> -->

        </div>

</template>

<script>

export default {
    props: {
        result: {
            type: Object,
            required: true
        }

    },
    data(){
        return {
            labelClick: false
        }
    }

}
</script>

<style scoped>



.Result-table{
    padding: 50px;
    display: grid;
    padding-left: 35%;
    grid-template-columns: 45% 43% 30% 42% 40% 50%;
    grid-template-areas: 
    "thumb artist year genre country label";
    gap: 10px;
    align-items: center;
    color: white;
    background: rgb(7, 7, 7);


}

.result-thumbnail{
    grid-template-areas: Thumb;
}
.result-artist{
    grid-template-areas: title;
}
.result-year{
    grid-template-areas: year;
}
.result-genre{
    grid-template-areas: genre;
}
.result-country{
    grid-template-areas: country;
}
.result-label{
    grid-template-areas: label;
}


</style>