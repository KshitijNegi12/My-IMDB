import Vue from "vue"
import VueRouter from "vue-router"
import MoviesContainerView from "@/views/MoviesContainerView.vue";
import PageNotFoundView from "@/views/PageNotFoundView.vue";
import PersonCard from "@/components/PersonCard.vue";
import MovieForm from "@/components/MovieForm.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'movies',
    component: MoviesContainerView
  },
  {
    path: '/actors',
    name: 'actors',
    component: PersonCard
  },
  {
    path: '/producers',
    name: 'producers',
    component: PersonCard
  },
  {
    path: '/create',
    name: 'create',
    component: MovieForm,
    props: { title: 'Enter Movie Details', type: 'create' },
    meta: { title: 'Add Movie' }
  },
  {
    path: '/edit/:id',
    name: 'edit',
    component: MovieForm,
    props: { title: 'Update Movie Details', type: 'edit' },
    meta: { title: 'Edit Movie' }
  },
  {
    path: '*',
    name: 'NotFound',
    component: PageNotFoundView
  }
]

const router = new VueRouter({
  mode: 'history',
  routes: routes,
  // eslint-disable-next-line
  scrollBehavior(to, from, savedPosition) {
    return { x: 0, y: 0 };
  }
})

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title
  } else {
    document.title = 'IMDB'
  }
  next()
})

export default router;