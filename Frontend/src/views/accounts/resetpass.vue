<template>
    <el-card>
        <router-link to="/forgot-pass">
        <i class="fa-solid fa-arrow-left"></i>
        </router-link>
        <h2 style="text-align: center;">Đặt lại mật khẩu</h2>
        <el-form :model="form" :rules="rules" ref="form">
        <el-form-item prop="otp">
            <el-input v-model="form.otp" placeholder="Nhập OTP"></el-input>
        </el-form-item>
        <el-form-item prop="password">
            <el-input type="password" v-model="form.password" placeholder="Nhập mật khẩu mới" show-password></el-input>
        </el-form-item>
        <el-button type="primary" @click="resetPass">Xác nhận</el-button>
        </el-form>
    </el-card>
</template>
<script>
import { ElMessage } from 'element-plus';

export default{
    data(){
        return{
            form:{
                otp:'',
                password:'',
                email:''
            },
            rules: {
                otp: [
                    { required: true, message: 'Nhập OTP', trigger: 'blur' },
                ],
                password: [
                    { required: true, message: 'Nhập mật khẩu mới', trigger: 'blur' },
                    { pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/, message: 'Mật khẩu 8 ký tự bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt', trigger: ['blur', 'change'] }
                ],
            },
        }
    },
    methods:{
        resetPass(){
            this.$refs.form.validate((valid) => {
                if (valid) {
                this.$store.dispatch('resetPass', { email: localStorage.getItem('email') , otp: this.form.otp, password: this.form.password})
                    .then(() => {
                    localStorage.removeItem('email');
                    ElMessage.success('Đặt lại mật khẩu thành công')
                    })
                    .catch((error) => {
                    console.error('Error dispatching forgotPass:', error);
                    ElMessage.error('Otp hoặc mật khẩu không chính xác');
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