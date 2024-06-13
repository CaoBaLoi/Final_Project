<template>
  <div class="block" m="t-4" style="width: 1400px; margin-left: auto; margin-right: auto;">
    <el-carousel trigger="click" height="450px">
      <el-carousel-item>
        <h3 class="small justify-center" text="2xl"></h3>
      </el-carousel-item>
    </el-carousel>
  </div>
  <div style="width: 1400px; margin-left: auto; margin-right: auto; align-items: center; justify-content: center;">
    <el-row class="row-bg" >
    <el-col :span="4"><div class="grid-content ep-bg-purple">a</div></el-col>
    <el-col :span="4"><div class="grid-content ep-bg-purple-light">a</div></el-col>
    <el-col :span="4"><div class="grid-content ep-bg-purple" >a</div></el-col>
    <el-col :span="4"><div class="grid-content ep-bg-purple-light" >a</div></el-col>
    <el-col :span="4"><div class="grid-content ep-bg-purple" >a</div></el-col>
    <el-col :span="4"><div class="grid-content ep-bg-purple-light" >a</div></el-col>
  </el-row>
  </div>
</template>
<style>
.el-row {
  margin-bottom: 20px;
}
.el-row:last-child {
  margin-bottom: 0;
}
.el-col {
  border-radius: 4px;
}

.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
</style>