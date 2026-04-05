export default {
  namespaced: true,
  state() {
    return {
      loaderDisplay: false,
      toastDisplay: false,
      toastMessage: '',
    }
  },
  mutations: {
    setLoader(state, status) {
      state.loaderDisplay = status;
    },
    setToast(state, payload) {
      state.toastDisplay = payload.status;
      if (payload.msg) {
        state.toastMessage = payload.msg
      }
    },
  },
}
