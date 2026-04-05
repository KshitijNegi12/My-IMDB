<template>
  <div v-if="toastDisplay" class="toast">
    <p>{{ toastMessage }}</p>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "AppToast",
  computed: {
    ...mapState("ui", ["toastDisplay", "toastMessage"]),
  },
};
</script>

<style scoped>
.toast {
  border-radius: 20px;
  padding: 2px 10px;
  position: absolute;
  right: 10px;
  top: 10px;
  width: 30%;
  background-color: red;
  color: white;
}
p {
  white-space: pre-line;
  margin-top: 8px;
  margin-left: 10px;
}
</style>
