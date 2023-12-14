<template>
  <div>
    <div>
      <div class="tab">
        <button class="tablinks" @click="tabToggle = 1">London</button>
        <button class="tablinks" @click="tabToggle = 2">Paris</button>
        <button class="tablinks" @click="tabToggle = 3">Tokyo</button>
      </div>

    </div>
    <div :class="tabToggle == 1 ? 'makeVisible' : 'notVisible' "  class="tabcontent">
      <h3>London</h3>
      <p>London is the capital city of England.</p>
    </div>

    <div :class="tabToggle == 2 ? 'makeVisible' : 'notVisible' "  class="tabcontent">
      <h3>Paris</h3>
      <p>Paris is the capital of France.</p>
    </div>

    <div :class="tabToggle == 3 ? 'makeVisible' : 'notVisible' " class="tabcontent">
      <h3>Tokyo</h3>
      <p>Tokyo is the capital of Japan.</p>
    </div>
  </div>
</template>

<script>
export default {
  data(){
    return{
      tabToggle: 1
    }
  }
}
</script>

<style>
.makeVisible{
  display: block;
}
.notVisible{
  display: none;
}
</style>