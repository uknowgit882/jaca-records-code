<template>
    <section class="section" id="app">
        <div class="columns">
            <div class="column is-4">
                <div class="card">
                    <div class="card-content">
                        <div class="card-carousel">
                            <div class="card-img">
                                <img src="img/logo2.png" alt="Some image">
                                <div class="actions">
                                    <span @click="prevImage" class="prev">
                                        <i class="fas fa-chevron-left"></i>
                                    </span>
                                    <span @click="nextImage" class="next">
                                        <i class="fas fa-chevron-right"></i>
                                    </span>
                                </div>
                            </div>
                            <div class="thumbnails">
                                <div class="thumbnail-img">
                                    <img src="img/background.jpg" alt="Some thumbnail">
                                </div>
                                <div class="thumbnail-img active">
                                    <img src="/some-thumbnail.jpg" alt="Some thumbnail">
                                </div>
                                <div class="thumbnail-img">
                                    <img src="/some-thumbnail.jpg" alt="Some thumbnail">
                                </div>

                                <p>I suck.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
export default {

    data() {
        return {
            //Array to hold all carousel images
            images: [
              
            ],
            //Index of the active image on the images array
            activeImage: 0
        }
    },
    computed: {
        // currentImage gets called whenever activeImage changes
        // and is the reason why we don't have to worry about the 
        // big image getting updated
        currentImage() {
            return this.images[this.activeImage].big;
        }
    },
    methods: {
        // Go forward on the images array 
        // or go at the first image if you can't go forward :/ 
        nextImage() {
            let active = this.activeImage + 1;
            if (active >= this.images.length) {
                active = 0;
            }
            this.activateImage(active);
        },
        // Go backwards on the images array 
        // or go at the last image
        prevImage() {
            let active = this.activeImage - 1;
            if (active < 0) {
                active = this.images.length - 1;
            }
            this.activateImage(active);
        },
        activateImage(imageIndex) {
            this.activeImage = imageIndex;
        }
    }
};

</script>

<style scoped>
p {
    color: black;
}
</style>