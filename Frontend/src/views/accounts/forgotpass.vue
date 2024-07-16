<template>
    <el-card>
      <router-link to="/login">
        <i class="fa-solid fa-arrow-left"></i>
      </router-link>
      <h2 style="text-align: center;">Đặt lại mật khẩu</h2>
      <el-form :model="form" :rules="rules" ref="form">
        <el-form-item prop="email">
          <el-input v-model="form.email" placeholder="Nhập email đăng ký"></el-input>
        </el-form-item>
        <el-button type="primary" @click="sendOTP">Nhận OTP</el-button>
      </el-form>
    </el-card>
  </template>
<script>
import { ElMessage } from 'element-plus';
export default{
    data(){
        return{
            form:{
                email:''
            },
            rules: {
                email: [
                    { required: true, message: 'Vui lòng nhập email', trigger: 'blur' },
                    { type: 'email', message: 'Email không hợp lệ', trigger: ['blur', 'change'] }
                ],
            },
        }
    },
    methods:{
        sendOTP() {
            this.$refs.form.validate((valid) => {
                if (valid) {
                this.$store.dispatch('forgotPass', { email: this.form.email })
                    .then(() => {
                    localStorage.setItem('email', this.form.email);
                    })
                    .catch((error) => {
                    console.error('Error dispatching forgotPass:', error);
                    ElMessage.error('Email không được đăng ký');
                    });
                } else {
                console.log('error submit!!');
                }
            });
            }
    }
}
</script>
<style scoped>
.el-card{
    width: 25%;margin-top: 15%; margin-left: 35%;
}
.el-button{
    margin-top: 20px;
    margin-left: 40%;
}
</style>