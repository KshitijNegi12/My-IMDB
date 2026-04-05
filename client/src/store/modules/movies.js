import { getMovies, getMovieById, createMovie, updateMovieCover, editMovie, deleteMovie } from "@/service/api";
import { updateToast } from "../util";

export default {
  namespaced: true,
  state() {
    return {
      movies: [],
      movieModelDisplay: false,
      deleteDialogDisplay: false,
      selectedMovie: {
        id: 0,
        name: '',
        yearOfRelease: null,
        plot: '',
        actorIds: [],
        actorsName: [],
        genreIds: [],
        genresName: [],
        producerId: null,
        producerName: null,
        coverImage: null,
        newCoverImage: null
      },
    }
  },

  mutations: {
    setMovies(state, payload) {
      state.movies = payload;
    },
    setMovieModelDisplay(state, payload) {
      state.movieModelDisplay = payload.status;
    },
    setDeleteDialogDisplay(state, payload) {
      state.selectedMovie.id = payload.id;
      if (payload) {
        state.selectedMovie.name = payload.name;
        state.deleteDialogDisplay = payload.status;
      }
    },
    setSelectedMovie(state, payload = null) {
      if (payload == null) {
        state.selectedMovie = {
          id: 0,
          name: '',
          yearOfRelease: null,
          plot: '',
          actorIds: [],
          actorsName: [],
          genreIds: [],
          genresName: [],
          producerId: null,
          producerName: null,
          coverImage: null,
          newCoverImage: null
        }
        return;
      }

      state.selectedMovie = {
        id: payload.id,
        name: payload.name,
        yearOfRelease: payload.yearOfRelease,
        plot: payload.plot,
        actorIds: payload.actors.map(a => a.id),
        actorsName: payload.actors.map(a => a.name),
        genreIds: payload.genres.map(g => g.id),
        genresName: payload.genres.map(g => g.name),
        producerId: payload.producer.id,
        producerName: payload.producer.name,
        coverImage: payload.coverImage,
        newCoverImage: null
      }
    },
    setActorToSelected(state, person) {
      state.selectedMovie.actorIds.push(person.id);
      state.selectedMovie.actorsName.push(person.Name);
    },
    setProducerToSelected(state, person) {
      state.selectedMovie.producerId = person.id;
      state.selectedMovie.producerName = person.Name;
    }
  },

  actions: {
    async fetchMovies({ commit }) {
      try {
        let res = await getMovies();
        commit('setMovies', res.data);
      } catch (error) {        
        updateToast(commit, error, `Failed to fetch movies, `);
      }
    },
    async fetchMovieById({ commit }, id) {
      try {
        let res = await getMovieById(id);
        commit('setSelectedMovie', res.data);
      } catch (error) {
        updateToast(commit, error, `Failed to fetch movie data, `);
        throw error;
      }
    },
    async createMovie({ state, commit }) {
      let res;
      try {
        res = await createMovie({
          name: state.selectedMovie.name,
          yearOfRelease: state.selectedMovie.yearOfRelease,
          plot: state.selectedMovie.plot,
          actorIds: state.selectedMovie.actorIds,
          producer: state.selectedMovie.producerId,
          genreIds: state.selectedMovie.genreIds,
          coverImage: 'None',
        });
      } catch (error) {        
        updateToast(commit, error, `Failed to create movie, `);
      }
      if(res){
        try {
          await updateMovieCover(res.data?.id, state.selectedMovie.newCoverImage);
        } catch (error) {
          updateToast(commit, error, `Failed to add movie cover, `);
        }
      }
    },
    async editMovie({ state, commit }) {
      try {
        let res = null;
        if (state.selectedMovie.newCoverImage != null) {
          res = await updateMovieCover(state.selectedMovie.id, state.selectedMovie.newCoverImage);
        }
        await editMovie(state.selectedMovie.id, {
          name: state.selectedMovie.name,
          yearOfRelease: state.selectedMovie.yearOfRelease,
          plot: state.selectedMovie.plot,
          actorIds: state.selectedMovie.actorIds,
          producer: state.selectedMovie.producerId,
          genreIds: state.selectedMovie.genreIds,
          coverImage: res == null ? state.selectedMovie.coverImage : res.data.url,
        });
      } catch (error) {
        updateToast(commit, error, `Failed to edit movie, `);
      }
    },
    async deleteMovie({ state, commit }, id) {
      try {
        await deleteMovie(id);
        let filterMovie = state.movies.filter(movie => movie.id !== id);
        commit('setMovies', filterMovie);
      } catch (error) {
        updateToast(commit, error, `Failed to delete movie, `);
      }
    },
  },
}