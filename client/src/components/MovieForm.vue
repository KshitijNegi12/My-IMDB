<template>
  <v-container>
    <v-card class="pa-6" outlined>
      <v-card-title class="text-h5">{{ title }}</v-card-title>
      <v-card-text>
        <!-- form -->
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="selectedMovie.name"
            label="Movie Name"
            placeholder="Ex. Spiderman"
            outlined
            :rules="[rules.required]"
          />

          <v-text-field
            v-model="selectedMovie.yearOfRelease"
            label="Year of Release"
            placeholder="20XX"
            outlined
            type="number"
            :rules="[rules.required, rules.validYear]"
          />

          <div class="d-flex align-baseline drop-down">
            <v-select
              class="me-2"
              v-model="selectedMovie.actorIds"
              :items="actors"
              item-text="name"
              item-value="id"
              label="Actors"
              multiple
              outlined
              @blur="actorTouched = true"
              :rules="[rules.required, rules.requiredArray('Actor')]"
            />
            <v-btn class="modal-btn" @click="openAddPerson('Actor')" outlined
              >Add Actor</v-btn
            >
          </div>

          <div class="d-flex align-baseline drop-down">
            <v-select
              class="me-2"
              v-model="selectedMovie.producerId"
              :items="producers"
              item-text="name"
              item-value="id"
              label="Producer"
              outlined
              :rules="[rules.required]"
            />
            <v-btn class="modal-btn" @click="openAddPerson('Producer')" outlined
              >Add Producer</v-btn
            >
          </div>

          <v-select
            v-model="selectedMovie.genreIds"
            :items="genres"
            item-text="name"
            item-value="id"
            label="Genres"
            multiple
            outlined
            @blur="genreTouched = true"
            :rules="[rules.required, rules.requiredArray('Genre')]"
          />

          <v-textarea
            v-model="selectedMovie.plot"
            label="Plot"
            placeholder="Describe the beautiful story"
            outlined
            :rules="[rules.required]"
          />

          <div class="d-flex">
            <v-file-input
              v-model="selectedMovie.newCoverImage"
              class="me-2"
              label="New Poster"
              accept="image/*"
              outlined
              show-size
              @change="onImageInput"
            />

            <img
              v-if="previewImg"
              :src="previewImg"
              alt="poster"
              height="100"
              width="100"
              contain
            />
            <img
              v-else-if="selectedMovie.coverImage"
              :src="selectedMovie.coverImage"
              alt="poster"
              height="100"
              width="100"
            />
            <div v-else class="text-center grey--text">
              No image <br />
              selected
            </div>
          </div>
        </v-form>
      </v-card-text>

      <div class="d-flex">
        <v-card-actions>
          <v-btn color="primary" @click="submitForm">Submit</v-btn>
        </v-card-actions>

        <router-link to="/" style="text-decoration: none">
          <v-card-actions>
            <v-btn color="secondary">Cancel</v-btn>
          </v-card-actions>
        </router-link>
      </div>

      <add-person-dialog
        v-model="showAddPersonDialog"
        :title="`Add ${addPersonType}`"
        @submit="onSubmitAddPerson"
      >
        <v-text-field
          v-model="newPerson.name"
          label="Name"
          :rules="[rules.required]"
          outlined
        />
        <v-text-field
          v-model="newPerson.dateOfBirth"
          label="Date of Birth"
          :rules="[rules.required, rules.validDOB]"
          type="date"
          outlined
        />
        <v-select
          v-model="newPerson.gender"
          :items="['Male', 'Female', 'Other']"
          label="Gender"
          :rules="[rules.required]"
          outlined
        />
        <v-textarea
          v-model="newPerson.bio"
          label="Bio"
          :rules="[rules.required]"
          outlined
        />
      </add-person-dialog>
    </v-card>
  </v-container>
</template>

<script>
import { mapActions, mapMutations, mapState } from "vuex";
import AddPersonDialog from "./AddPersonDialog.vue";

export default {
  name: "MovieForm",
  components: {
    AddPersonDialog,
  },
  props: ["title", "type"],
  data() {
    return {
      actorTouched: false,
      genreTouched: false,
      previewImg: null,
      valid: true,
      addPersonType: "",
      showAddPersonDialog: false,
      newPerson: {
        name: "",
        dateOfBirth: "",
        gender: "",
        bio: "",
      },
      rules: {
        required: (v) => !!v || "This field is required.",
        validDOB: (v) => {
          if (!v) return "Date of Birth is required.";
          const date = new Date(v);
          const year = date.getFullYear();
          const month = date.getMonth();
          const day = date.getDate();
          const currentYear = new Date().getFullYear();
          const currentMonth = new Date().getMonth();
          const currentDay = new Date().getDate();

          if (year < 1900 || year > currentYear) {
            return `Year must be between 1900 and ${currentYear}.`;
          }
          if (year == currentYear && month > currentMonth) {
            return `Invalid Month Selected`;
          }
          if (
            year == currentYear &&
            month >= currentMonth &&
            day > currentDay
          ) {
            return `Invalid Date Selected`;
          }

          return true;
        },

        validYear: (v) => {
          const year = parseInt(v);
          const currentYear = new Date().getFullYear();
          return (
            (year >= 1900 && year <= currentYear) ||
            `Enter a valid year (1900 - ${currentYear}).`
          );
        },
        requiredArray: (label) => (v) => {
          if (label == "Actor" && !this.actorTouched) return true;
          if (label == "Genre" && !this.genreTouched) return true;
          return (
            (Array.isArray(v) && v.length > 0) ||
            `Select at least one ${label}.`
          );
        },
      },
    };
  },
  computed: {
    ...mapState("actors", ["actors"]),
    ...mapState("producers", ["producers"]),
    ...mapState("genres", ["genres"]),
    ...mapState("movies", ["selectedMovie"]),
  },
  watch: {
    showAddPersonDialog(newVal){
      if(newVal == false){
        this.resetPersonDialogField();
      }
    }
  },
  methods: {
    ...mapMutations("ui", ["setLoader"]),
    ...mapMutations("movies", ["setSelectedMovie"]),
    ...mapActions("actors", ["fetchActors", "addActor"]),
    ...mapActions("producers", ["fetchProducers", "addProducer"]),
    ...mapActions("movies", ["createMovie", "editMovie", "fetchMovieById"]),
    ...mapActions("genres", ["fetchGenres"]),
    openAddPerson(type) {
      this.addPersonType = type; // actor or producer
      this.showAddPersonDialog = true;
    },
    onImageInput(file) {
      if (file) {
        this.previewImg = URL.createObjectURL(file);
      } else {
        this.previewImg = null;
      }
    },
    resetPersonDialogField() {
      this.newPerson = { name: "", dateOfBirth: "", gender: "", bio: "" };
    },
    async onSubmitAddPerson() {
      const person = { ...this.newPerson };
      if (this.addPersonType === "Actor") {
        await this.addActor(person);
      } else if (this.addPersonType === "Producer") {
        await this.addProducer(person);
      }
      await this.resetPersonDialogField();
    },
    async submitForm() {
      this.actorTouched = this.genreTouched = true;
      const isValid = this.$refs.form.validate();
      if (!isValid) {
        return;
      }

      if (this.type == "create") {
        this.setLoader(true);
        await this.createMovie();
        this.setLoader(false);
      } else if (this.type == "edit") {
        this.setLoader(true);
        await this.editMovie();
        this.setLoader(false);
      }
      this.$router.push("/");
    },
  },
  async created() {
    await this.fetchActors();
    await this.fetchProducers();
    await this.fetchGenres();

    if (this.type == "edit") {
      try {
        await this.fetchMovieById(this.$route.params.id);
      } catch (error) {
        this.$router.push("/notfound");
      }
    }
  },
  destroyed() {
    this.setSelectedMovie(null);
  },
};
</script>

<style scoped>
.v-card {
  max-width: 600px;
  margin: auto;
}
.drop-down {
  flex-basis: 500px;
}
.modal-btn {
  width: clamp(3.125rem, 2.305rem + 4.375vw, 7.5rem) !important;
  font-size: clamp(0.406rem, 0.354rem + 0.281vw, 0.688rem) !important;
}
</style>
