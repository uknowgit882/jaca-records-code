<template>
<img src="image()"/>

</template>

<script>
export default {
    props: {
    carouselCard: {
      type: Object,
      required: true
    },
    computed: {
        image() {
            return this.carouselCard.record.images[0].uri
        }
    }
  }
}
</script>

<style>

</style>