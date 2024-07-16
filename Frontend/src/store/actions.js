import axios from 'axios';
import router from '../router/router.js'
export default {
  async fetchCategories({ commit }) {
    try {
      const response = await axios.get('http://localhost:5183/api/category');
      commit('SET_CATEGORIES', response.data);
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  },
  async login({ commit }, { UserName, Password, Remember }) {
    try {
      const response1 = await axios.post('http://localhost:5183/api/account/login', { UserName, Password, Remember });
      const token = response1.data.token;
      if (token != null) {
        localStorage.setItem('token', token);
        const response2 = await axios.get('http://localhost:5183/api/user/info', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        if (response2 != null) {
          commit('SET_LOGIN', response2.data.username);
          localStorage.setItem('loggedIn', true);
          localStorage.setItem('username', response2.data.username);
          localStorage.setItem('role', response2.data.roles)
          const roles = response2.data.roles;
          if (roles.includes('Admin')) {
            router.push('/dashboard');
          } else {
            router.push('/home');
          }
        } else {
          console.error('Không thể lấy thông tin người dùng.');
        }
      } else {
        console.error('Đăng nhập thất bại.');
      }
    } catch (error) {
      console.error('Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại sau.', error);
      throw new Error('Login failed');
    }
  },
  async forgotPass({commit}, email){
    try{
      await axios.post('http://localhost:5183/api/account/forgot-password', email)
      router.push('/reset-pass')
    }catch(error){
      console.error('Forgot pass failed', error)
      throw new Error('Forgot pass failed');
    }
  },
  async resetPass({commit},{email, otp, password}){
    try{
      const url = `http://localhost:5183/api/account/reset-password`;
      const params = { email };
      const data = { otp, password };
      await axios.post(url, data, { params });
      router.push('/login')
    }catch(error){
      console.log('Reset pass failed',error)
      throw new Error('Reset pass failed')
    }
  },
  async logout({ commit }) {
    try{
      await axios.post('http://localhost:5183/api/account/logout');
      commit('LOGOUT');
      localStorage.removeItem('loggedIn');
      localStorage.removeItem('token');
      localStorage.removeItem('username');
      localStorage.removeItem('role');
      localStorage.removeItem('defaultActive');
      localStorage.removeItem('selectedMethod')
      router.push('/login');
    }catch (error){
      console.error('Đã xảy ra lỗi khi đăng xuất', error);
    }
  },
  async register({commit},{UserName, Email, Password }){
    try{
      const response = await axios.post('http://localhost:5183/api/account/register', {UserName, Email, Password});
      commit('Set_Users', response.data)
      router.push('/login')
    }catch(error){
      console.error("Lỗi đăng ký",error);
      throw new Error('Register failed');
    }
  },
  async createCategory({commit},{category_id, category_name}){
    try{
      const response = await axios.post('http://localhost:5183/api/category', {category_id, category_name});
      commit('ADD_CATEGORIES',response.data)
      router.push('/category')
    }catch(error){
      console.error("Lỗi thêm danh mục",error);
      throw new Error('Add category failed')
    }
  },
  async fetchProducts({ commit }) {
    try {
      const response = await axios.get('http://localhost:5183/api/product');
      commit('SET_PRODUCTS', response.data);
    } catch (error) {
      console.error('Error fetching products:', error);
    }
  },
  async getTagname({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/tag');
      commit('SET_TAGS', response.data)
    }catch (error) {
      console.error('Error get tag name:', error);
    }
  },
  async createProduct({ commit }, formData) {
    try {
      const response = await axios.post('http://localhost:5183/api/product', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      commit('ADD_PRODUCTS', response.data);
      router.push('/product');
    } catch (error) {
      console.error('Lỗi thêm sản phẩm', error);
      throw new Error('Add product failed')
    }
  },
  async updateProductQuantity({commit}, {product_id, quantity}){
    try{
      await axios.put(`http://localhost:5183/api/product/quantity`,null,{
        params:{
          product_id: product_id,
          quantity: quantity
        }
      })
    }catch(error){
      console.log('Update product quantity failed')
      throw new Error('Update product quantity failed')
    }
  },
  async importProduct({commit}, data){
    try{
      await axios.post('http://localhost:5183/api/importproduct', data)
    }catch(error){
      throw new Error('Import product failed')
    }
  },
  async getProduct({commit},id){
    try{
      const response = await axios.get(`http://localhost:5183/api/product/${id}`)
      commit('SET_PRODUCT',response.data)
    }catch(error){
      console.error('Lỗi lấy sản phẩm', error)
    }
  },
  async fetchCart({ commit }) {
    const response = await axios.get('http://localhost:5183/api/cart');
    commit('SET_CART', response.data);
  },
  async addToCart({ commit }, {product_id, quantity}) {
    const loggedIn = localStorage.getItem('loggedIn') === 'true';
    const role = localStorage.getItem('role');
    if (!loggedIn && role !== 'User') {
      router.push('/login');
      return;
    }
    try {
      const response = await axios.post('http://localhost:5183/api/cart', {
        product_id,
        quantity
      });
      commit('ADD_TO_CART', response.data); // Assuming the API returns the cart item
    } catch (error) {
      console.error('Error adding to cart:', error);
      throw new Error('Error adding to cart')
    }
  },
  async removeFromCart({ commit }, product_id) {
    try{
      await axios.delete(`http://localhost:5183/api/cart/${product_id}`);
      commit('REMOVE_FROM_CART', product_id);
    }catch(error){
      throw new Error('Remove cart failed')
    }
  },
  async getProfile({commit}){
    const response = await axios.get('http://localhost:5183/api/profile');
    commit('SET_PROFILE',response.data);
  },
  async addProfile({commit},{fullname, phone, address}){
    try {
      const response = await axios.post('http://localhost:5183/api/profile', {
        fullname,
        phone,
        address
      });
      commit('SET_PROFILE', response.data);
    } catch (error) {
      console.error('Error adding to profile:', error);
      throw new Error('Error adding to profile')
    }
  },
  async getAddress({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/address');
      commit('SET_ADDRESS',response.data);
    }catch(error){
      throw new Error('Error getting address')
    }
  },
  async addAddress({commit},{name, phone, address}){
    try {
      const response = await axios.post('http://localhost:5183/api/address', {
        name,
        phone,
        address
      });
      commit('ADD_ADDRESS', response.data);
    } catch (error) {
      console.error('Error adding to address', error);
      throw new Error('Error adding to address')
    }
  },
  async getOrders({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/order/all')
      commit('SET_ORDERS',response.data)
    }catch(error){
      console.error('Lỗi lấy đơn hàng', error)
    }
  },
  async getOrder({commit},id){
    try{
      const response = await axios.get(`http://localhost:5183/api/order/${id}`)
      commit('SET_ORDER',response.data)
      return response.data
    }catch(error){
      console.error('Lỗi lấy đơn hàng', error)
    }
  },
  async getOrdersByUser({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/order/by-id')
      commit('SET_ORDERS',response.data)
    }catch(error){
      console.error('Lỗi lấy đơn hàng', error)
    }
  },
  async addOrder({commit}, orders){
    const response = await axios.post('http://localhost:5183/api/order/place', orders);
    commit('ADD_ORDER', response.data);
    router.push({name:'user.order'}) 
  },
  async changeStatusOrder({commit},{status, order_id}){
    try{
      await axios.put(`http://localhost:5183/api/order`, null, {
        params: {
          status: status,
          order_id: order_id
        }
      });
    }catch(error){
      throw new Error('Change status failed')
    }
  },
  async addReceivedDate({commit},{order_id, received_date}){
    try{
      await axios.post(`http://localhost:5183/api/receiveddate`,null,{
        params:{
          order_id: order_id,
          received_date: received_date
        }
      })
    }catch(error){
      console.error(error)
      throw new Error('Add received date failed')
    }
  },
  async getReceivedDate({commit}, id){
    try{
      const respone = await axios.get(`http://localhost:5183/api/receiveddate/${id}`)
      commit('SET_RECEIVEDDATE',respone.data) 
    }catch(error){
      console.log('Get received date failed')
    }
  },
  async getSales({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/sale/by-time')
      commit('SET_SALES',response.data)
    }catch(error){
      console.error('Lỗi lấy sản phẩm giảm giá', error)
    }
  },
  async getAllSales({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/sale')
      commit('SET_SALES',response.data)
    }catch(error){
      console.error('Lỗi lấy sản phẩm giảm giá', error)
    }
  },
  async getSale({commit},id){
    try{
      const response = await axios.get(`http://localhost:5183/api/sale/${id}`)
      commit('SET_SALE',response.data)
    }catch(error){
      console.error('Lỗi lấy giảm giá', error)
    }
  },
  async addSale({commit},sale){
    try{
      const response = await axios.post('http://localhost:5183/api/sale', sale);
      commit('ADD_SALE', response.data);
      router.push({name:'sale.index'})
    }catch(error){
      throw new Error('Add sale failed')
    }
  },
  async getImportProduct({commit}){
    try{
      const response = await axios.get('http://localhost:5183/api/importproduct');
      commit('GET_IMPORTPRODUCT', response.data);
    }catch(error){
      throw new Error('get import product failed')
    }
  },
  async addFeedBack({commit},{product_id, feedback, feedback_date, feedback_point}){
    try{
      await axios.post('http://localhost:5183/api/feedback',{product_id, feedback, feedback_date, feedback_point})
    }catch(error){
      console.log('Feedback failed')
      throw new Error('Feedback failed')
    }
  },
  async getAllFeedBack({commit}){
    try{
      const respone = await axios.get('http://localhost:5183/api/feedback')
      commit('GET_FEEDBACK', respone.data)
    }catch(error){
      console.log('Get feedback failed')
    }
  },
  async getFeedBackByUser({commit}){
    try{
      const respone = await axios.get('http://localhost:5183/api/feedback/by-user')
      commit('GET_FEEDBACK', respone.data)
    }catch(error){
      console.log('Get feedback failed')
    }
  }
};
