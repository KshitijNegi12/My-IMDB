<template>
  <v-dialog v-model="flag" max-width="700px">
    <v-card>
      <div class="w-100 d-flex">
        <v-card-title class="text-h5 font-weight-bold pb-0">{{
          selectedMovie?.name
        }}</v-card-title>
        <v-icon
          style="height: fit-content"
          color="red"
          class="me-3 mt-3"
          @click="setMovieModelDisplay({ status: false })"
          >mdi-close-thick</v-icon
        >
      </div>
      <v-divider class="mx-4 mb-3"></v-divider>
      <v-card-text>
        <v-row
          class="flex-column flex-md-row flex-lg-row align-content-center justify-center"
        >
          <v-col cols="5" class="ms-sm-14 ms-lg-0 ms-md-0">
            <v-img
              :src="selectedMovie?.coverImage"
              height="250"
              contain
            ></v-img>
          </v-col>

          <v-col cols="7" class="d-flex flex-column justify-center">
            <p><strong>Plot:</strong> {{ selectedMovie?.plot }}</p>

            <p>
              <strong>Genres:</strong>
              <span
                class="box mx-1 pa-1 rounded"
                v-for="genre in selectedMovie?.genresName"
                :key="genre"
              >
                {{ genre }}
              </span>
            </p>

            <p>
              <strong>Actors:</strong>
              <span
                class="box mx-1 pa-1 rounded"
                v-for="actor in selectedMovie?.actorsName"
                :key="actor"
              >
                {{ actor }}
              </span>
            </p>

            <p>
              <strong>Producer:</strong>
              <span class="box mx-1 pa-1 rounded">
                {{ selectedMovie?.producerName }}
              </span>
            </p>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapMutations, mapState } from "vuex";

export default {
  name: "MovieModal",
  computed: {
    ...mapState("movies", ["selectedMovie", "movieModelDisplay"]),
    flag: {
      get() {
        return this.movieModelDisplay;
      },
      set(value) {
        if(!value){
          this.setSelectedMovie(null);
        }
        this.setMovieModelDisplay({ status: value });
      },
    },
  },
  methods: {
    ...mapMutations("movies", ["setSelectedMovie", "setMovieModelDisplay"]),
  },
};
</script>

<style scoped>
.box {
  background-color: rgb(229, 236, 235);
  line-height: 2em;
}
.d-flex {
  justify-content: space-between;
}
</style>
