<template>
    <div class="error-popup">
        <div class="error-popup-inner">
            <div class="error-popup-inner-inner">
                <div>
                    <p style="color: white; font-weight: bold; text-align: center;">Something went wrong when you tried to {{ errorMessage }}</p>
                    <br>
                    <p style="color: white; font-weight: bold; text-align: center;">Click anywhere to close</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: ['errorMessage'],
    data() {
        return {
            }
        },
    methods: {
    }
}

</script>

<style> 

 .error-popup {
     border-radius: 5px;
     position: fixed;
     top: 0;
     left: 0;
     right: 0;
     bottom: 0;
     z-index: 99;
     background-color: rgba(0, 0, 0, 0.75);
     display: flex;
     align-items: center;
     justify-content: center;
     word-wrap: break-word;
 }

 .error-popup-inner scoped {
     border-radius: 5px;
     display: flex;
     flex-direction: column;
     align-items: center;
     justify-content: center;
     background: #EA5143;
     opacity: 1.00;
     padding: 10px;
     text-align: center;
     height: 100%;
     width: 100%;
 }

 .error-popup-inner-inner {
    display: flex;
    flex-direction: column;
    justify-content: center;
    border-radius: 5px;
     background: #EA5143;
     text-align: center;
     height: 100px;
     width: 700px;
 }
</style>