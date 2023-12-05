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
.Record{

  width: 250px;
  height: 550px;
  margin: 20px;
  position: relative;
}
</style>
