<template>
  <v-dialog v-model="visible" max-width="500">
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <slot></slot>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" @click="submit">Submit</v-btn>
        <v-btn text @click="close">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "AddPersonDialog",
  props: ["title", "value"],
  data() {
    return {
      visible: this.value,
      valid: true,
    };
  },
  watch: {
    value(val) {
      this.visible = val;
    },
    visible(val) {
      if(val == false){
        this.$refs.form.reset();
      }
      this.$emit("input", val);
    },
  },
  methods: {
    submit() {
      if (this.$refs.form.validate()) {
        this.$emit("submit");
        this.visible = false;
      }
    },
    close() {
      this.visible = false;      
    },
  },
};
</script>
