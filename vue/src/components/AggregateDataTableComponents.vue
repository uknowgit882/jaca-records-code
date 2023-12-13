<template>
    <div class="AggData">
        <table class="table-Aggregate">
            <tr>
                <th>Categories</th>
                <th>Aggregate</th>
                <th  v-if="$store.state.token != ''">Users</th>
            </tr>
            <tr>
                <td>Total Artists</td>
                <td>{{ this.$store.state.StatsAggregate.totalArtists }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalArtists }}</td>
            </tr>
            <tr>
                <td>Total Collections</td>
                <td>{{ this.$store.state.StatsAggregate.totalCollections }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalCollections }}</td>
            </tr>
            <tr>
                <td>Total Formats</td>
                <td>{{ this.$store.state.StatsAggregate.totalFormats }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalFormats }}</td>
            </tr>
            <tr>
                <td>Total Genres</td>
                <td>{{ this.$store.state.StatsAggregate.totalGenres }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalGenres }}</td>
            </tr>
            <tr>
                <td>Total Images</td>
                <td>{{ this.$store.state.StatsAggregate.totalImages }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalImages }}</td>
            </tr>
            <tr>
                <td>Total Labels</td>
                <td>{{ this.$store.state.StatsAggregate.totalLabels }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalLabels }}</td>
            </tr>
            <tr>
                <td>Total Records In Collections</td>
                <td>{{ this.$store.state.StatsAggregate.totalCollections }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalCollections }}</td>
            </tr>
            <tr>
                <td>Total Tracks</td>
                <td>{{ this.$store.state.StatsAggregate.totalTracks }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalTracks }}</td>
            </tr>
            <tr>
                <td>Total Users</td>
                <td>{{ this.$store.state.StatsAggregate.totalUsers }}</td>
                <td  v-if="$store.state.token != ''">1</td>
            </tr>
            <tr>
                <td>Total Records</td>
                <td>{{ this.$store.state.StatsAggregate.totalRecords }}</td>
                <td  v-if="$store.state.token != ''">{{ this.$store.state.StatsUser.totalRecords }}</td>
            </tr>
            <tr>
                <td>Number of Records By Artists</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown">
                                <span>Records By Artists </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(artist, numRecords) in $store.state.StatsAggregate.numRecordsByArtist"
                                    :key="artist">
                                    {{ numRecords }} : {{ artist }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive2 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown2">
                                <span>Records By Artists </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(artist, numRecords) in $store.state.StatsUser.numRecordsByArtist" :key="artist">
                                    {{ numRecords }} : {{ artist }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Number of Records By Format</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive3 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown3">
                                <span>Records By Format </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(format, numRecords) in $store.state.StatsAggregate.numRecordsByFormat"
                                    :key="format">
                                    {{ numRecords }} : {{ format }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive4 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown4">
                                <span>Records By Format </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(format, numRecords) in $store.state.StatsUser.numRecordsByFormat"
                                    :key="format">
                                    {{ numRecords }} : {{ format }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Number of Records By Genre</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive5 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown5">
                                <span>Records By Genre </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(genre, numRecords) in $store.state.StatsAggregate.numRecordsByGenre"
                                    :key="genre">
                                    {{ numRecords }} : {{ genre }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive6 }">
                        <div class="dropdown-trigger">
                            
                            <button class="button" @click="toggleDropdown6">
                                <span>Records By Genre </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(genre, numRecords) in $store.state.StatsUser.numRecordsByGenre"
                                    :key="genre">
                                    {{ numRecords }} : {{ genre }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Number of Records By Label</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive7 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown7">
                                <span>Records By Label </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(label, numRecords) in $store.state.StatsAggregate.numRecordsByLabel"
                                    :key="label">
                                    {{ numRecords }} : {{ label }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive8 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown8">
                                <span>Records By Label </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(label, numRecords) in $store.state.StatsUser.numRecordsByLabel"
                                    :key="label">
                                    {{ numRecords }} : {{ label }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td>Number of Records By Year</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive9 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown9">
                                <span>Records By Year </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(year, numRecords) in $store.state.StatsAggregate.numRecordsByYear"
                                    :key="year">
                                    {{ numRecords }} : {{ year }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive10 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown10">
                                <span>Records By Year </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(year, numRecords) in $store.state.StatsUser.numRecordsByYear"
                                    :key="year">
                                    {{ numRecords }} : {{ year }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Number of Records By Country</td>
                <td>
                    <div class="dropdown" :class="{ 'is-active': dropdownActive11 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown11">
                                <span>Records By Country </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                    v-for="(country, numRecords) in $store.state.StatsAggregate.numRecordsByCountry"
                                    :key="country">
                                    {{ numRecords }} : {{ country }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
                <td  v-if="$store.state.token != ''">
                    <div class="dropdown" :class="{ 'is-active': dropdownActive12 }">
                        <div class="dropdown-trigger">
                            <button class="button" @click="toggleDropdown12">
                                <span>Records By Country </span>
                                <span class="icon is-small">
                                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </button>
                        </div>
                        <div class="dropdown-menu" role="menu">
                            <div class="dropdown-content">
                                <div class="dropdown-item"
                                v-for="(country, numRecords) in $store.state.StatsUser.numRecordsByCountry"
                                    :key="country">
                                    {{ numRecords }} : {{ country }}

                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>






        </table>
    </div>
</template>
  
<script>
export default {
    data() {
        return {
            dropdownActive: false,
            dropdownActive2: false,
            dropdownActive3: false,
            dropdownActive4: false,
            dropdownActive5: false,
            dropdownActive6: false,
            dropdownActive7: false,
            dropdownActive8: false,
            dropdownActive9: false,
            dropdownActive10: false,
            dropdownActive11: false,
            dropdownActive12: false,


        };
    },
    methods: {
        toggleDropdown() {
            this.dropdownActive = !this.dropdownActive;
        },
        toggleDropdown2() {
            this.dropdownActive2 = !this.dropdownActive2;
        },
        toggleDropdown3() {
            this.dropdownActive3 = !this.dropdownActive3;
        },
        toggleDropdown4() {
            this.dropdownActive4 = !this.dropdownActive4;
        },
        toggleDropdown5() {
            this.dropdownActive5 = !this.dropdownActive5;
        },
        toggleDropdown6() {
            this.dropdownActive6 = !this.dropdownActive6;
        },
        toggleDropdown7() {
            this.dropdownActive7 = !this.dropdownActive7;
        },
        toggleDropdown8() {
            this.dropdownActive8 = !this.dropdownActive8;
        },
        toggleDropdown9() {
            this.dropdownActive9 = !this.dropdownActive9;
        },
        toggleDropdown10() {
            this.dropdownActive10 = !this.dropdownActive10;
        },
        toggleDropdown11() {
            this.dropdownActive11 = !this.dropdownActive11;
        },
        toggleDropdown12() {
            this.dropdownActive12 = !this.dropdownActive12;
        },
    },
};
</script>
  
<style scoped>
.table-Aggregate {}

h2 {
    color: white;
}

.AggData {
    background: rgba(0, 0, 0, 0.75);
    display: flex;
    padding-bottom: 32px;
}

table {
    width: 80%;
    border-collapse: collapse;
    margin-top: 30px;
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
    background-color: #b856abcb;
    color: white;
}

.button {
    color: white;
    background-color: black;
}

/* .button {
    --color: white;
    font-family: inherit;
    display: inline-block;
    width: 15.3em;
    height: 2.6em;
    line-height: 1.5em;
    margin: 20px;
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

} */

/* .button:before {
    content: "";
    position: absolute;
    z-index: -1;
    background: var(--color);
    height: 115px;
    width: 500px;
    border-radius: 50%;
}

.button:hover {
    color: black;
}

.button:before {
    top: 100%;
    left: 100%;
    transition: all .7s;
}

.button:hover:before {
    top: -30px;
    left: -30px;
    background-color: #D1D301;
}

.button:active:before {
    background: #4D437F;
    transition: background 0s;
} */
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
              <h2 v-for="(format, numRecords) in $store.state.StatsUser.numRecordsByFormat" :key="format">
                Number of Records By Format ({{ format }}): {{ numRecords }}
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
