<template>
  <div class="register-container">
    <el-card class="register-card">
      <h2 class="title">Đăng ký</h2>
      <el-form :model="form" :rules="rules" ref="form" label-position="top" class="register-form">
        <el-form-item label="Tên đăng nhập" prop="UserName">
          <el-input v-model="form.UserName" placeholder="Nhập tên đăng nhập"></el-input>
        </el-form-item>
        <el-form-item label="Email" prop="Email">
          <el-input v-model="form.Email" placeholder="Nhập email"></el-input>
        </el-form-item>
        <el-form-item label="Mật khẩu" prop="Password">
          <el-input type="password" v-model="form.Password" placeholder="Nhập mật khẩu" show-password></el-input>
        </el-form-item>
        <el-form-item>
          <div class="button-container">
            <el-button type="primary" @click="onSubmit">Đăng ký</el-button>
          </div>
        </el-form-item>
        <div class="login-link-container">
          <span>Đã có tài khoản? <a><router-link to="/login">Đăng nhập</router-link></a></span>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import { ElMessage } from 'element-plus';
export default {
  data() {
    return {
      form: {
        UserName: '',
        Email: '',
        Password: '',
      },
      rules: {
        UserName: [
          { required: true, message: 'Vui lòng nhập tên đăng nhập', trigger: 'blur' }
        ],
        Email: [
          { required: true, message: 'Vui lòng nhập email', trigger: 'blur' },
          { type: 'email', message: 'Email không hợp lệ', trigger: ['blur', 'change'] }
        ],
        Password: [
          { required: true, message: 'Vui lòng nhập mật khẩu', trigger: 'blur' },
          { pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/, message: 'Mật khẩu 8 ký tự bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt', trigger: ['blur', 'change'] }
        ]
      }
    }
  },
  methods: {
    async onSubmit() {
      console.log(this.form.UserName, this.form.Email, this.form.Password);
      try {
          await this.$store.dispatch('register', { 
          UserName: this.form.UserName,
          Email: this.form.Email,
          Password: this.form.Password
          
        })
        ElMessage.success('Đăng ký thành công');
      } catch (error) {
        console.error('Đã xảy ra lỗi khi đăng ký:', error);
        ElMessage.error("Đăng ký không thành công")
      }
    },
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f2f2f2;
}

.register-card {
  width: 400px;
  padding: 20px;
  background-color: white;
}

.title {
  text-align: center;
  margin-bottom: 20px;
  font-size: 40px;
  font-weight: bold;
}

.register-form {
  width: 100%;
}

.el-form-item {
  margin-bottom: 15px;
  position: relative;
}

.button-container {
  display: flex;
  justify-content: center;
  margin-left: auto;
  margin-right: auto;
}

.login-link-container {
  text-align: center;
  margin-top: 20px;
}

.divider {
  margin: 20px 0;
}

.google-login-container {
  display: flex;
  justify-content: center;
}

.el-button.el-icon-google {
  background-color: #db4437;
  color: white;
  border-color: #db4437;
}

.el-button.el-icon-google:hover {
  background-color: #c33d2e;
  border-color: #c33d2e;
}
</style>
