import { getActors, addActor } from "@/service/api"
import { updateToast } from "../util";

export default {
  namespaced: true,
  state() {
    return {
      actors: [],
    }
  },
  mutations: {
    setActors(state, payload) {
      state.actors = payload;
    },
  },
  actions: {
    async fetchActors({ commit }) {
      try {
        let res = await getActors();
        commit('setActors', res.data);
      } catch (error) {
        updateToast(commit, error, `Failed to fetch actors, `);
      }
    },
    async addActor({ state, commit }, person) {
      try {
        let res = await addActor(person);
        person.id = res.data.id;
        commit('movies/setActorToSelected', person, { root: true });
        state.actors.push(person);
      } catch (error) {
        updateToast(commit, error, `Failed to add actor, `);
      }
    },
  }
}