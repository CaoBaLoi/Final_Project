using BackendApi.Migrations;
using Grocery.Data;
using Grocery.Helpers;
using Grocery.Models;
using Grocery.Repositories;
using Grocery.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 36)));
});
// builder.Services.AddIdentityApiEndpoints<User>()
// 	.AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddIdentity<User,IdentityRole>(options =>{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/api/account/login"; // Đường dẫn tới API đăng nhập
    // options.AccessDeniedPath = "/api/account/accessdenied"; // Đường dẫn tới API truy cập bị từ chối
    options.SlidingExpiration = true;
});
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
builder.Services.AddScoped<IProductTagService, ProductTagService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISaleDetailRepository, SaleDetailRepository>();
builder.Services.AddScoped<ISaleDetailService, SaleDetailService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductTagService>();
builder.Services.AddScoped<ProductImageService>();
builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddScoped<SaleDetailService>();
builder.Services.AddScoped<ProfileRepository>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<AuthenticationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
