import Vue from "vue";
import Vuex from "vuex";
import actors from "./modules/actors";
import producers from "./modules/producers";
import movies from "./modules/movies";
import genres from "./modules/genres";
import ui from "./modules/ui";

Vue.use(Vuex);

export default new Vuex.Store({
  modules:{
    actors,
    producers,
    movies,
    genres,
    ui
  }
});
