<template>
  <div v-if="loaderDisplay" class="loader-container">
    <v-progress-circular
      size="60"
      width="6"
      indeterminate
    ></v-progress-circular>
    <p class="loader-message">Please Wait...</p>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "AppLoader",
  props: {
    message: {
      type: String,
      default: "",
    },
  },
  computed: {
    ...mapState("ui", ["loaderDisplay"]),
  },
};
</script>

<style scoped>
.loader-container {
  position: fixed;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: rgba(230, 230, 230, 0.8);
  backdrop-filter: blur(3px);
}

.loader-message {
  margin-top: 16px;
  font-size: 1.1em;
  color: #555;
}
</style>
