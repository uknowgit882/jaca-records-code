<template>
    <div class="AggData">
        <div class="AggDataLeft">
            <h2> Total Artists: {{ this.$store.state.StatsAggregate.totalArtists }}</h2>
            <h2> Total Collections: {{ this.$store.state.StatsAggregate.totalCollections }}</h2>
            <h2> Total Formats: {{ this.$store.state.StatsAggregate.totalFormats }}</h2>
            <h2> Total Genres: {{ this.$store.state.StatsAggregate.totalGenres }}</h2>
            <h2> Total Images: {{ this.$store.state.StatsAggregate.totalImages }}</h2>
            <h2> Total Labels: {{ this.$store.state.StatsAggregate.totalLabels }}</h2>
            <h2> Total Records in Collections: {{ this.$store.state.StatsAggregate.totalRecordsInCollections }}</h2>
            <h2> Total Tracks: {{ this.$store.state.StatsAggregate.totalTracks }}</h2>
            <h2 v-for="(artist, numRecords) in $store.state.StatsAggregate.numRecordsByArtist" :key="artist">
                Number of Records By Artist ({{ artist }}): {{ numRecords }}
            </h2>
            <h2 v-for="(genre, numRecords) in $store.state.StatsAggregate.numRecordsByGenre" :key="genre">
                Number of Records By Genre ({{ genre }}): {{ numRecords }}
            </h2>
            <h2 v-for="(label, numRecords) in $store.state.StatsAggregate.numRecordsByLabel" :key="label">
                Number of Records By Label ({{ label }}): {{ numRecords }}
            </h2>
            <h2> Total Users: {{ this.$store.state.StatsAggregate.totalUsers }}</h2>
            <h2> Total Records: {{ this.$store.state.StatsAggregate.totalRecords }}</h2>
            <h2 v-for="(year, numLabels) in $store.state.StatsAggregate.numRecordsByYear" :key="year">
                Number of Records By Label ({{ year }}): {{ numLabels }}
            </h2>
            <h2 v-for="(country, numLabels) in $store.state.StatsAggregate.numRecordsByCountry" :key="country">
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
.AggDataLeft{
    width: 50vw;
    background: red;
}
</style>
