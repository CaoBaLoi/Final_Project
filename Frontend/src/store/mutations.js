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
    ADD_PRODUCTS(state, product){
      state.products.push(product)
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
    },
    SET_PROFILE(state, profile){
      state.profile = profile;
    },
    SET_ADDRESS(state, address){
      state.address = address
    },
    ADD_ADDRESS(state, newAddress){
      state.address.push(newAddress)
    },
    SET_ORDERS(state, orders){
      state.orders = orders
    },
    SET_ORDER(state, order){
      state.order = order
    },
    ADD_ORDER(state, order){
      state.orders.push(order)
    },
    UPDATE_STATUS(state, status){
      state.status = status
    },
    SET_SALES(state, sales){
      state.sales = sales
    },
    SET_SALE(state, sale){
      state.sale = sale
    },
    ADD_SALE(state, sale){
      state.sales.push(sale)
    },
    SET_RECEIVEDDATE(state, receiveddate){
      state.receiveddate = receiveddate
    },
    GET_IMPORTPRODUCT(state, importproduct){
      state.importproduct = importproduct
    },
    GET_FEEDBACK(state, feedbacks){
      state.feedbacks = feedbacks
    },
  };
  