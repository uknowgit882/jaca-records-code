<template>
    <div class="AggData">
        <div class="AggDataRight">
            <h2> Total Artists: {{ this.$store.state.StatsUser.totalArtists }}</h2>
            <h2> Total Collections: {{ this.$store.state.StatsUser.totalCollections }}</h2>
            <h2> Total Formats: {{ this.$store.state.StatsUser.totalFormats }}</h2>
            <h2> Total Genres: {{ this.$store.state.StatsUser.totalGenres }}</h2>
            <h2> Total Images: {{ this.$store.state.StatsUser.totalImages }}</h2>
            <h2> Total Labels: {{ this.$store.state.StatsUser.totalLabels }}</h2>
            <h2> Total Records in Collections: {{ this.$store.state.StatsUser.totalRecordsInCollections }}</h2>
            <h2> Total Tracks: {{ this.$store.state.StatsUser.totalTracks }}</h2>
            <h2 v-for="(artist, numRecords) in $store.state.StatsUser.numRecordsByArtist" :key="artist">
                Number of Records By Artist ({{ artist }}): {{ numRecords }}
            </h2>
            <h2 v-for="(genre, numRecords) in $store.state.StatsUser.numRecordsByGenre" :key="genre">
                Number of Records By Genre ({{ genre }}): {{ numRecords }}
            </h2>
            <h2 v-for="(label, numRecords) in $store.state.StatsUser.numRecordsByLabel" :key="label">
                Number of Records By Label ({{ label }}): {{ numRecords }}
            </h2>
            <h2> Total Users: {{ this.$store.state.StatsUser.totalUsers }}</h2>
            <h2> Total Records: {{ this.$store.state.StatsUser.totalRecords }}</h2>
            <h2 v-for="(year, numLabels) in $store.state.StatsUser.numRecordsByYear" :key="year">
                Number of Records By Label ({{ year }}): {{ numLabels }}
            </h2>
            <h2 v-for="(country, numLabels) in $store.state.StatsUser.numRecordsByCountry" :key="country">
                Number of Records By Label ({{ country }}): {{ numLabels }}
            </h2>
        </div>
    </div>
</template>

<script>

export default {

}
</script>

<style scoped>

h2 {
    color: white;
}

.AggData {
    background: black;
    display: flex;
}
.AggDataRight{
    width: 50vw;
    background: green;
}
</style>
