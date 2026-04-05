import { getGenres } from "@/service/api";
import { updateToast } from "../util";

export default {
  namespaced: true,
  state() {
    return {
      genres: [],
    }
  },
  mutations: {
    setGenres(state, payload) {
      state.genres = payload;
    },
  },
  actions: {
    async fetchGenres({ commit }) {
      try {
        let res = await getGenres();
        commit('setGenres', res.data);
      } catch (error) {
        updateToast(commit, error, `Failed to fetch genres, `);
      }
    },
  }
}