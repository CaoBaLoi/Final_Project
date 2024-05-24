<template>
<div class="fixed-top w-100 bg-nav">
<nav class="navbar navbar-expand-md w-75 mx-auto">
  <a class="navbar-brand" href="#">WebSiteName</a>
  <button class="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navb" aria-expanded="true">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div id="navb" class="navbar-collapse collapse hide justify-content-md-end">
    <ul class="navbar-nav pe-5">
      <li class="nav-item active">
        <a class="nav-link" href="#">Home</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="#">Page 1</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="#">Page 2</a>
      </li>
    </ul>
    <form class="d-flex" role="search">
        <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search">
        <button class="btn btn-outline-success" type="submit">Search</button>
    </form>
    <div class="d-grid gap-2 d-md-flex justify-content-md-end ps-5">
        <button class="btn btn-primary me-md-2" type="button">Đăng nhập</button>
        <button class="btn btn-primary" type="button">Đăng ký</button>
    </div>
  </div>
</nav>
</div>
</template>
<style>
.bg-nav{
  background-image: linear-gradient(to right, #30CFD0 10%, #330867 100%);
}
</style>