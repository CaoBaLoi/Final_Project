import { createStore } from "vuex";
import state from "./state";
import actions from "./actions";
import mutations from "./mutations";
import getters from "./getters";
const store = createStore({
    state,
    actions,
    mutations,
    getters,
})
const loggedIn =localStorage.getItem('loggedIn');
const username = localStorage.getItem('username');
if (loggedIn && username) {
  store.commit('SET_LOGIN', username );
}
export default store;