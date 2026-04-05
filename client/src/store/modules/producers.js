import { getProducers, addProducer } from "@/service/api";
import { updateToast } from "../util";

export default {
  namespaced: true,
  state() {
    return {
      producers: [],
    }
  },
  mutations: {
    setProducers(state, payload) {
      state.producers = payload;
    },
  },
  actions: {
    async fetchProducers({ commit }) {
      try {
        let res = await getProducers();
        commit('setProducers', res.data);
      } catch (error) {
        updateToast(commit, error, `Failed to fetch producers, `);
      }
    },
    async addProducer({ state, commit }, person) {
      try {
        let res = await addProducer(person);
        person.id = res.data.id;
        commit('movies/setProducerToSelected', person, { root: true });
        state.producers.push(person);
      } catch (error) {
        updateToast(commit, error, `Failed to add producer, `);
      }
    }
  }
}