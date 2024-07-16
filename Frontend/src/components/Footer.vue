<template>
    <el-footer class="footer">
        <div class="site-section">
        <div class="container">
            <div>
            <h3 href="#" class="footer-logo">Househole Shop</h3>
            <p class="copyright">
                <small>© 2024</small>
            </p>
            </div>
            <div>
            <h3>Về chúng tôi</h3>
            <ul class="links">
                <li><a href="#">Giới thiệu</a></li>
                <li><a href="#">Lĩnh vực hoạt động</a></li>
            </ul>
            </div>
            <div>
            <h3>Trợ giúp</h3>
            <ul class=" links">
                <li><a href="#">Điều khoản và điều kiện</a></li>
                <li><a href="#">Chính sách bảo mật</a></li>
                <li><a href="#">Chính sách bán hàng</a></li>
            </ul>
            </div>
            <div>
            <h3>Liên hệ</h3>
            <ul class=" links">
                <li><a href="#"><small><i class="fa-solid fa-location-dot"></i></small> Hà Nội</a></li>
                <li><a href="#"><small><i class="fa-solid fa-phone"></i></small> 0333333333</a></li>
                <li><a href="#"><small><i class="fa-regular fa-envelope"></i></small> abc@gmail.com</a></li>
            </ul>
            </div>
            <div>
            <h3>Theo dõi chúng tôi</h3>
            <ul class="social">
                <li><a href="#"><i class="fa-brands fa-facebook"></i></a></li>
                <li><a href="#"><i class="fa-brands fa-twitter"></i></a></li>
                <li><a href="#"><i class="fa-brands fa-linkedin-in"></i></a></li>
            </ul>
            </div>
            
        </div>
        </div>
    </el-footer>
</template>

<style scoped>
.footer {
  background-color: #333;
  color: #fff;
  padding: 40px 0;
  margin-top: 50px;
  height: fit-content;
}

.site-section {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.container {
  max-width: 1200px;
  width: 100%;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
}

.footer-logo {
  font-size: 1.5rem;
  font-weight: bold;
  color: #fff;
  text-decoration: none;
}

.footer-logo:hover {
  text-decoration: underline;
}

.copyright {
  font-size: 0.875rem;
  color: #bbb;
  margin-top: auto;
}

.container > div {
  margin: 20px;
  flex: 1;
}

h3 {
  font-size: 1.125rem;
  margin-bottom: 10px;
}

ul {
  list-style: none;
  padding: 0;
}

ul.links li,
ul.social li {
  margin-bottom: 5px;
}

ul.links a,
ul.social a {
  color: #bbb;
  text-decoration: none;
}

ul.links a:hover,
ul.social a:hover {
  text-decoration: underline;
}

.social {
  font-size: 1.5rem;
  display: flex;
}

.social a {
  margin-right: 10px;
}

@media (max-width: 768px) {
  .container {
    flex-direction: column;
    align-items: center;
  }

  .container > div {
    margin-bottom: 20px;
  }
}
</style>

