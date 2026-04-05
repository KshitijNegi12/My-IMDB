export function updateToast(commit, error, msg) {
  if(error != null){
    msg += `${error.message != undefined ? error.message : error.error != undefined ? error.error : ''}`
  }  
  commit('ui/setToast', { status: true, msg: msg }, { root: true })
  setTimeout(() => {
    commit('ui/setToast', { status: false }, { root: true })
  }, 5000);
}