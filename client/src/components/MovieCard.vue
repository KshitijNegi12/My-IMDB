<template>
  <v-card class="pa-3 mx-2 my-4" height="380" max-height="400">
    <v-img height="180" v-bind:src="movie.coverImage" cover></v-img>

    <v-card-title class="movie-name font-weight-bold">{{
      movie.name
    }}</v-card-title>

    <v-card-text>
      <div class="movie-plot">
        {{ shortPlot(movie.plot) }}
      </div>
    </v-card-text>

    <v-card-actions>
      <v-btn text @click="explore(movie.id)">
        Explore
        <v-icon>mdi-arrow-right</v-icon>
      </v-btn>

      <v-spacer></v-spacer>

      <router-link :to="{ name: 'edit', params: { id: movie.id } }">
        <v-btn icon>
          <v-icon color="blue">mdi-square-edit-outline</v-icon>
        </v-btn>
      </router-link>

      <v-btn
        icon
        @click="
          setDeleteDialogDisplay({
            status: true,
            id: movie.id,
            name: movie.name,
          })
        "
      >
        <v-icon color="red">mdi-trash-can</v-icon>
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { mapActions, mapMutations } from "vuex";

export default {
  name: "MovieCard",
  props: ["movie"],
  methods: {
    ...mapMutations("movies", [
      "setMovieModelDisplay",
      "setDeleteDialogDisplay",
    ]),
    ...mapActions("movies", ["fetchMovieById"]),
    explore(id) {
      this.fetchMovieById(id);
      this.setMovieModelDisplay({ status: true });
    },
    shortPlot(plot) {
      if (plot.length > 60) {
        return plot.substr(0, 60) + " ...";
      }
      return plot;
    },
  },
};
</script>

<style scoped>
.movie-name {
  font-size: 1em;
}
.movie-plot {
  height: 48px;
}
</style>
