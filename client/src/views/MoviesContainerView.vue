<template>
  <div>
    <v-container class="grey lighten-5">
      <v-btn
        elevation="3"
        rounded
        color="green white--text px-8"
        :to="{ name: 'create' }"
      >
        Add Movie</v-btn
      >
      <v-row no-gutters>
        <v-col
          v-for="movie in movies"
          :key="movie.id"
          cols="12"
          lg="3"
          md="4"
          sm="6"
        >
          <movie-card :movie="movie"></movie-card>
        </v-col>
      </v-row>
    </v-container>
    <movie-modal></movie-modal>
    <delete-dialoge></delete-dialoge>
  </div>
</template>

<script>
import MovieCard from "@/components/MovieCard.vue";
import { mapActions, mapState } from "vuex";
import MovieModal from "@/components/MovieModal.vue";
import DeleteDialog from "@/components/DeleteDialog.vue";

export default {
  name: "MoviesContainerView",
  components: {
    "movie-card": MovieCard,
    "movie-modal": MovieModal,
    "delete-dialoge": DeleteDialog,
  },
  computed: {
    ...mapState('movies', ["movies"]),
  },
  methods: {
    ...mapActions('movies', ["fetchMovies"]),
  },
  created() {
    this.fetchMovies();
  },
};
</script>

<style scoped></style>
