<template>
    <div class="AggData">
      <table class="table-Aggregate">
        <tr>
          <th>Categories</th>
          <th>Aggregate</th>
          <th>Users</th>
        </tr>
        <tr>
          <td>Total Artists</td>
          <td>{{ this.$store.state.StatsAggregate.totalArtists }}</td>
          <td>{{ this.$store.state.StatsUser.totalArtists }}</td>
        </tr>
        <tr>
          <td>Total Collections</td>
          <td>{{ this.$store.state.StatsAggregate.totalCollections }}</td>
          <td>{{ this.$store.state.StatsUser.totalCollections }}</td>
        </tr>
        <tr>
          <td>Total Formats</td>
          <td>{{ this.$store.state.StatsAggregate.totalFormats }}</td>
          <td>{{ this.$store.state.StatsUser.totalFormats}}</td>
        </tr>
        <tr>
          <td>Total Genres</td>
          <td>{{ this.$store.state.StatsAggregate.totalGenres }}</td>
          <td>{{ this.$store.state.StatsUser.totalGenres}}</td>
        </tr>
        <tr>
          <td>Total Images</td>
          <td>{{ this.$store.state.StatsAggregate.totalImages }}</td>
          <td>{{ this.$store.state.StatsUser.totalImages}}</td>
        </tr>
        <tr>
            <td>Total Labels</td>
            <td>{{ this.$store.state.StatsAggregate.totalLabels }}</td>
            <td>{{ this.$store.state.StatsUser.totalLabels}}</td>
        </tr>
        <tr>
          <td>Total Records In Collections</td>
          <td>{{ this.$store.state.StatsAggregate.totalCollections }}</td>
          <td>{{ this.$store.state.StatsUser.totalCollections}}</td>
        </tr>
        <tr>
          <td>Total Tracks</td>
          <td>{{ this.$store.state.StatsAggregate.totalTracks }}</td>
          <td>{{ this.$store.state.StatsUser.totalTracks}}</td>
        </tr>
        <tr>
          <td>Total Users</td>
          <td>{{ this.$store.state.StatsAggregate.totalUsers }}</td>
          <td>{{ this.$store.state.StatsUser.totalUsers }}</td>
        </tr>
        <tr>
          <td>Total Records</td>
          <td>{{ this.$store.state.StatsAggregate.totalRecords }}</td>
          <td>{{ this.$store.state.StatsUser.totalRecords}}</td>
        </tr>
        <tr>
          <td>Number of Records By Artists</td>
          <td v-for="(artist, numRecords) in $store.state.StatsUser.numRecordsByArtist" :key="artist">
               ({{ artist }}): {{ numRecords }}</td>
          <td></td>
        </tr>
      </table>
    </div>
  </template>
  
  <script>
  export default {};
  </script>
  
  <style scoped>
  .table-Aggregate{}

  h2 {
    color: white;
  }
  
  .AggData {
    background: black;
    display: flex;
  }
  
  table {
    width: 80%;
    border-collapse: collapse;
    margin-top: 20px;
    margin-left: 250px;
    color: white
  }
  
  th,
  td {
    border: 1px solid white;
    padding: 8px;
    text-align: left;
    
  }
  
  th {
    background-color: rgb(72, 41, 82);
    color: white;
  }
  </style>

<!-- <template>
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
            <h2 v-for="(year, numYears) in $store.state.StatsUser.numRecordsByYear" :key="year">
                Number of Records By Label ({{ year }}): {{ numYears }}
            </h2>
            <h2 v-for="(country, numCountry) in $store.state.StatsUser.numRecordsByCountry" :key="country">
                Number of Records By Label ({{ country }}): {{ numCountry }}
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
</style> -->
