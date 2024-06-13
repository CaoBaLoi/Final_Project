<template>
  <div class="register-container">
    <el-card class="register-card">
      <h2 class="title">Đăng nhập</h2>
      <el-form :model="form" :rules="rules" ref="form" label-position="top" class="register-form">
        <el-form-item label="Tên đăng nhập" prop="UserName">
          <el-input v-model="form.UserName" placeholder="Nhập tên đăng nhập"></el-input>
        </el-form-item>
        <el-form-item label="Mật khẩu" prop="Password">
          <el-input type="password" v-model="form.Password" placeholder="Nhập mật khẩu"></el-input>
        </el-form-item>
        <el-form-item>
          <el-checkbox v-model="form.Remember" prop="Remember">Nhớ mật khẩu</el-checkbox>
          <a href="#" class="forgot-password">Quên mật khẩu?</a>
        </el-form-item>
        <el-form-item>
          <div class="button-container">
            <el-button type="primary" @click="onSubmit">Đăng nhập</el-button>
          </div>
        </el-form-item>
        <div class="login-link-container">
          <span>Chưa có tài khoản? <a ><router-link to="/register">Đăng ký</router-link></a></span>
        </div>
        <hr class="divider" />
        <div class="google-login-container">
          <el-button type="danger" icon="el-icon-google" @click="onGoogleLogin">Đăng nhập với Google</el-button>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<script>
export default {
  data() {
    return {
      form: {
        UserName: '',
        Password: '',
        Remember: false
      },
      rules: {
        name: [
          { required: true, message: 'Vui lòng nhập họ và tên', trigger: 'blur' }
        ],
        password: [
          { required: true, message: 'Vui lòng nhập mật khẩu', trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    async onSubmit() {
      try {
         await this.$store.dispatch('login', { 
          UserName: this.form.UserName,
          Password: this.form.Password,
          Remember: this.form.Remember
        });
      } catch (error) {
        console.error('Đã xảy ra lỗi khi đăng nhập:', error);
        alert('Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại sau.');
      }
    },
    onGoogleLogin() {
      // Logic đăng nhập với Google
      alert('Đăng nhập với Google');
    }
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

.forgot-password {
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
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