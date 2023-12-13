<template>
    <div class="library-popup-tracks-table-row-header">
        <span class="library-popup-tracks-table-title-row">{{ track.title }}</span>
        <span class="library-popup-tracks-table-position-row">{{ track.position }}</span>
        <span class="library-popup-tracks-table-duration-row">{{ track.duration }}</span>
        <p></p>
    </div>

</template>

<script>
export default {
    props:{
        track: {
            type: Object,
            required: true
        }
    }
}
</script>

<style>
.library-popup-tracks-table-row-header{
    display: grid;
    grid-template-columns: 3fr 1fr 1fr;
    grid-template-areas: 
    "trackTitleRow trackPositionRow trackDurationRow";
    gap: 2px;
    font-weight: normal; 
    font-size: 10px;

 }
 .library-popup-tracks-table-title-row{
    grid-area: trackTitleRow;
    text-align: left;
 }
 .library-popup-tracks-table-position-row{
    grid-area: trackPositionRow;
 }
 .library-popup-tracks-table-duration-row{
    grid-area: trackDurationRow;
 }
</style>