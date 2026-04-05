<template>
  <v-container>
    <v-row>
      <v-col
        v-for="person in target"
        :key="person.id"
        cols="12"
        sm="6"
        md="4"
        lg="3"
        class="d-flex justify-center"
      >
        <v-card
          class="pa-4 d-flex flex-column align-center"
          style="width: 1000px"
          outlined
        >
          <v-avatar size="100">
            <img src="../assets/profile.jpg" />
          </v-avatar>

          <div class="mt-2 text-center">
            <strong>{{ person.name }}</strong>
          </div>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
  name: "PersonCard",
  computed: {
    ...mapState("actors", ["actors"]),
    ...mapState("producers", ["producers"]),
    target() {
      if (this.$route.path == "/actors") {
        return this.actors;
      } else if (this.$route.path == "/producers") {
        return this.producers;
      }
      return [];
    },
  },
  methods: {
    ...mapActions("actors", ["fetchActors"]),
    ...mapActions("producers", ["fetchProducers"]),
  },
  created() {
    this.fetchActors();
    this.fetchProducers();
  },
};
</script>

<style scoped></style>
