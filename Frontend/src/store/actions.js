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
      // Yêu cầu đăng nhập và nhận token từ phản hồi
      const response1 = await axios.post('http://localhost:5183/api/account/login', { UserName, Password, Remember });
      const token = response1.data.token;
  
      // Kiểm tra phản hồi đăng nhập có thành công hay không
      if (token != null) {
        // Lưu trữ token vào local storage
        localStorage.setItem('token', token);
  
        // Gửi yêu cầu GET để lấy thông tin người dùng
        const response2 = await axios.get('http://localhost:5183/api/user/userinfo', {
          headers: {
            Authorization: `Bearer ${token}` // Thêm token vào header Authorization
          }
        });
  
        // Kiểm tra phản hồi lấy thông tin người dùng có thành công hay không
        if (response2 != null) {
          // Lưu thông tin người dùng vào store
          //commit('Set_Users', response1.data);
          commit('SET_LOGIN', response2.data.username);
          localStorage.setItem('loggedIn', true);
          localStorage.setItem('username', response2.data.username);
          localStorage.setItem('role', response2.data.roles)
  
          // Kiểm tra và điều hướng người dùng dựa trên vai trò
          const roles = response2.data.roles; // Danh sách vai trò
          if (roles.includes('Admin')) {
            // Điều hướng tới trang dashboard nếu vai trò là Admin
            router.push('/dashboard');
          } else {
            // Điều hướng tới trang home nếu vai trò không phải là Admin
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
    }
  },
  
  async logout({ commit }) {
    try{
      await axios.post('http://localhost:5183/api/account/logout');
      commit('LOGOUT');
      localStorage.removeItem('loggedIn');
      localStorage.removeItem('token');
      localStorage.removeItem('username');

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
    }
  },
  async createCategory({commit},{category_id, category_name}){
    try{
      const response = await axios.post('http://localhost:5183/api/category', {category_id, category_name});
      commit('ADD_CATEGORIES',response.data)
      router.push('/category')
    }catch(error){
      console.error("Lỗi thêm danh mục",error);
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
  async createProduct({commit}, {product_id, product_name, category_name, tag_name, product_price, product_quantity, product_description, image_file}){
    try{
      const response = await axios.post('http://localhost:5183/api/product',{product_id, product_name, category_name, tag_name, product_price, product_quantity, product_description, image_file})
      commit('ADD_PRODUCTS', response.data)
      router.push('/product')
    }catch(error){
      console.error('Lỗi thêm sản phẩm', error)
    }
  },
  async getProduct({commit},id){
    try{
      const response = await axios.get(`http://localhost:5183/api/Product/${id}`)
      commit('SET_PRODUCT',response.data)
    }catch(error){
      console.error('Lỗi lấy sản phẩm', error)
    }
  },
  async fetchCart({ commit }) {
    const response = await axios.get('/api/cart');
    commit('SET_CART', response.data);
  },
  async addToCart({ commit }, item) {
    await axios.post('/api/cart', item);
    commit('ADD_TO_CART', item);
  },
  async removeFromCart({ commit }, product_id) {
    await axios.delete(`/api/cart/${product_id}`);
    commit('REMOVE_FROM_CART', product_id);
  }
};
