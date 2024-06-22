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
    },
    SET_PRODUCT(state, product){
      state.product = product
    },
    SET_CART(state, cart) {
      state.cart = cart;
    },
    ADD_TO_CART(state, item) {
      const existingItem = state.cart.find(i => i.product_id === item.product_id);
      if (existingItem) {
        existingItem.quantity += item.quantity;
      } else {
        state.cart.push(item);
      }
    },
    REMOVE_FROM_CART(state, product_id) {
      state.cart = state.cart.filter(i => i.product_id !== product_id);
    }
  };
  