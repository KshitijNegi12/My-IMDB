<template>
  <v-row justify="center">
    <v-dialog v-model="flag" max-width="290">
      <v-card>
        <v-card-title class="text-h5"> Delete Movie </v-card-title>

        <v-card-text>
          Are you sure you want to delete
          <strong>{{ selectedMovie?.name }}</strong
          >?
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn
            color="blue white--text"
            text
            @click="setDeleteDialogDisplay({ status: false })"
          >
            No
          </v-btn>

          <v-btn color="red" text @click="deleteSelectedMovie()"> Yes </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
import { mapActions, mapMutations, mapState } from "vuex";

export default {
  name: "DeleteDialog",
  computed: {
    ...mapState("movies", ["selectedMovie", "deleteDialogDisplay"]),
    flag: {
      get() {
        return this.deleteDialogDisplay;
      },
      set(value) {
        this.setDeleteDialogDisplay({ status: value });
      },
    }
  },
  methods: {
    ...mapMutations("movies", ["setDeleteDialogDisplay"]),
    ...mapActions("movies", ["deleteMovie"]),
    deleteSelectedMovie() {
      let id = this.selectedMovie.id;
      this.deleteMovie(id);
      this.setDeleteDialogDisplay({ status: false });
    },
  },
};
</script>

<style scoped></style>
