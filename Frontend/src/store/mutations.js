export default {
    SET_CATEGORIES(state, categories) {
      state.categories = categories;
    },
    Set_Users(state, aspnetusers){
      state.aspnetusers = aspnetusers;
    },
    SET_LOGIN(state, username) {
      state.loggedIn = true;
      state.username = username;
    },
    LOGOUT(state) {
      state.loggedIn = false;
      state.username = '';
    },
    ADD_CATEGORIES(state, category){
      state.categories.push(category)
    },
    SET_PRODUCTS(state, products){
      state.products = products;
    },
    SET_TAGS(state, tags){
      state.tags = tags;
    }
  };
  