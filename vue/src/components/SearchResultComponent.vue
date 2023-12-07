<template>
    <div class="Result-table">
        <img class="result-thumbnail" v-bind:src="result.thumb" />
        <span class="result-artist">{{ result.title }}</span>
        <span class="result-year">{{ result.year }}</span>
        <!-- First dropdown for genres -->
        <div class="dropdown" :class="{ 'is-active': labelClick }">
            <button class="button" @click="labelClick = !labelClick" aria-haspopup="true" aria-controls="dropdown-menu2">
                <span>{{ result.genre[0] }}</span>
                <span class="icon is-small">
                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                </span>
            </button>

            <div class="dropdown-menu" id="dropdown-menu2" role="menu">
                <div class="dropdown-content">
                    <ul class="result-genre">
                        <li v-for="item in result.genre" :key="item">
                            <button class="dropdown-item">{{ item }}</button>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- First dropdown ends -->

        <!-- <ul class="result-genre">
            <li v-for="item in result.genre" v-bind:key="item">{{ item }}</li>
        </ul> -->

        <span class="result-country">{{ result.country }}</span>

        <!-- Second dropdown starts: -->

        <div class="dropdown" :class="{ 'is-active': labelClick2 }">
            <button class="button" @click="labelClick2 = !labelClick2" aria-haspopup="true" aria-controls="dropdown-menu2">
                <span>{{ result.label[0] }}</span>
                <span class="icon is-small">
                    <i class="fas fa-angle-down" aria-hidden="true"></i>
                </span>
            </button>

            <div class="dropdown-menu" id="dropdown-menu2" role="menu">
                <div class="dropdown-content">
                    <ul class="result-label">
                        <li v-for="item in result.label" v-bind:key="item">{{ item }}</li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- Second dropdown stops here -->

        <!-- <ul class="result-label">
            <li v-for="item in result.label" v-bind:key="item">{{ item }}</li>
        </ul> -->

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
    data() {
        return {
            labelClick: false,
            labelClick2: false

        }
    },
    computed: {
        selectedGenre() {
            return this.labelClick ? this.result.genre[0] : '';
        },
        selectedLabel() {
            return this.labelClick2 ? this.result.label[0] : '';
        }


    }
}
</script>

<style scoped>
.Result-table {
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

.result-thumbnail {
    grid-template-areas: Thumb;
}

.result-artist {
    grid-template-areas: title;
}

.result-year {
    grid-template-areas: year;
}

.result-genre {
    grid-template-areas: genre;
}

.result-country {
    grid-template-areas: country;
}

.result-label {
    grid-template-areas: label;
}
#dropdown-menu2{
    color:black
}
</style>