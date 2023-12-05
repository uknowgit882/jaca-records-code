<template>
    <div class ="Record">
        <img class="record-image" v-bind:src="record.album_cover"/>
        <h2 class="record-title">{{ record.title }}</h2>
        <h3 class ="record-artist">{{ record.artist }}</h3>
    </div>
</template>

<script>

export default {
 props: ['record'],
}
</script>

<style scoped>
.record-image{
height: 250px;
width: 250px;
display: flex;
flex-wrap: wrap;

}
</style>
