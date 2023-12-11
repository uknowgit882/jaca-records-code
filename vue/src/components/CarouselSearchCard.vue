<template>
<div class="carouselSearchCard_container">
  <img class="carouselSearchCard_image" :src="record.thumb"/>
  <h2 class="carouselSearchCard_year">Year: {{ record.year }}</h2>
  <h3 class="carouselSearchCard_title">Title and Artist:</h3>
  <h3 class="carouselSearchCard_title">{{ record.title }}</h3> 
</div>

</template>

<script>
export default {
    props: {
    carouselSearchCard: {
      type: Object,
      required: true
    }
  }
}
</script>

<style>
.carouselSearchCard_container{
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-areas: 
  "image year"
  "title title";
}
.carouselSearchCard_image{
  grid-area: image;
}
.carouselSearchCard_year{
  grid-area: year;
}
.carouselSearchCard_title{
  grid-area: title;
}
</style>