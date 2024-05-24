using Dapper;
using Grocery.DTOs;
using Grocery.Helpers;
using Grocery.Models;
using Grocery.Services;

namespace Grocery.Repositories{
    public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetAll();
		Task<Product?> GetById(int id);
		Task Create(ProductDTO productDTO);
		Task Update(ProductDTO productDTO);
		Task Delete(int id);
	}
    public class ProductRepository(DataContext context, TagService tagService, CategoryService categoryService, ProductTagService productTagService, ProductImageService productImageService, GoogleDriveService googleDriveService) : IProductRepository
	{
		private readonly DataContext _context = context;
        private readonly TagService _tagService = tagService;
		private readonly CategoryService _categoryService = categoryService;
		private readonly ProductTagService _productTagService = productTagService;
		private readonly ProductImageService _productImageService = productImageService;
		private readonly GoogleDriveService _googleDriveService = googleDriveService;
		public async Task<IEnumerable<Product>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Products
			";
			return await connection.QueryAsync<Product>(sql);
		}
		public async Task<Product?> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Products
				WHERE product_id = @id
			";
			return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { id });
		}
		public async Task Create(ProductDTO productDTO)
		{
			using var connection = _context.CreateConnection();
			var categoryName = productDTO.category_name;  		
			int categoryId = await _categoryService.GetOrCreateCategoryId(productDTO.category_name);

			var sql = @"
				INSERT INTO Products (product_name, category_id, product_price, product_quantity, product_description)
				VALUES (@product_name, @category_id, @product_price, @product_quantity, @product_description);
				SELECT LAST_INSERT_ID();
			";
			int productId = await connection.QuerySingleAsync<int>(sql, new {
				productDTO.product_name,
				category_id = categoryId,
				productDTO.product_price,
				productDTO.product_quantity,
				productDTO.product_description
			});

			List<int> tagIds = new List<int>();
			foreach (var tag in productDTO.tag_name)
			{
				int tagId;
				tagId = await _tagService.GetOrCreateTagId(tag);

				tagIds.Add(tagId);
			}

			foreach (int tagId in tagIds)
			{
				await _productTagService.Create(new ProductTag { product_id= productId, tag_id = tagId});
			}

			List<string> image_urls = new List<string>();
			foreach (var imageFile in productDTO.image_file)
			{
				string? imageUrl;
				imageUrl= _googleDriveService.UploadImageToGoogleDrive(imageFile);
				if(imageUrl != null){
					image_urls.Add(imageUrl);
				}
			}

			foreach (string image_url in image_urls)
			{
				await _productImageService.Create(new ProductImage { product_id = productId, image_url = image_url});
			}
		}

		public async Task Update(ProductDTO productDTO)
		{
			using var connection = _context.CreateConnection();

			// Cập nhật thông tin sản phẩm
			var categoryName = productDTO.category_name;
			int categoryId =  await _categoryService.GetOrCreateCategoryId(productDTO.category_name);

			var sql = @"
				UPDATE Products
				SET product_name     = @product_name,
					category_id      = @category_id,
					product_price    = @product_price,
					product_quantity = @product_quantity,
					product_description = @product_description
				WHERE product_id = @product_id;
			";
			await connection.ExecuteAsync(sql, new
			{
				productDTO.product_id,
				productDTO.product_name,
				category_id = categoryId,
				productDTO.product_price,
				productDTO.product_quantity,
				productDTO.product_description,
			});
			int productId = productDTO.product_id;
			//Xóa các tag gắn với sản phẩm
			List<int> producttagIds = await _productTagService.GetByProductId(productId);
			if (producttagIds != null){
				foreach (int producttagId in producttagIds){
					await _productTagService.Delete(producttagId);
				}
			}
			List<int> tagIds = new List<int>();
			foreach (var tag in productDTO.tag_name)
			{
				int tagId;
				tagId = await _tagService.GetOrCreateTagId(tag);

				tagIds.Add(tagId);
			}

			foreach (int tagId in tagIds)
			{
				await _productTagService.Create(new ProductTag { product_id= productId, tag_id = tagId});
			}
			// Xóa các hình ảnh của sản phẩm trên Google Drive
			List<string> old_urls = await _productImageService.GetUrlByProductId(productId);
			if (old_urls != null)
			{
				foreach (string old_url in old_urls)
				{
					if (old_url != null)
					{
						// Sử dụng Replace để thay thế phần cố định "https://drive.google.com/uc?id=" bằng chuỗi trống ""
						string fileId = old_url.Replace("https://drive.google.com/uc?id=", "");
						_googleDriveService.DeleteImageFromGoogleDrive(fileId);
					}
				}
			}
			// Xóa các bản ghi ProductImage
			List<int> productImageIds = await _productImageService.GetIdByProductId(productId);
			if (productImageIds != null){
				foreach (int productImageId in productImageIds){
					await _productImageService.Delete(productImageId);
				}
			}
			// Cập nhật thông tin hình ảnh sản phẩm
			List<string> image_urls = new List<string>();
			foreach (var imageFile in productDTO.image_file)
			{
				string? imageUrl = _googleDriveService.UploadImageToGoogleDrive(imageFile);
				if (imageUrl != null)
				{
					image_urls.Add(imageUrl);
				}
			}

			// Thêm các hình ảnh mới của sản phẩm
			foreach (string image_url in image_urls)
			{
				await _productImageService.Create(new ProductImage { product_id = productId, image_url = image_url});
			}
		}


		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			// Xóa các hình ảnh của sản phẩm trên Google Drive
			List<string> old_urls = await _productImageService.GetUrlByProductId(id);
			if (old_urls != null)
			{
				// List chứa các tác vụ xóa hình ảnh từ Google Drive
				var deleteTasks = new List<Task>();
				foreach (string old_url in old_urls)
				{
					if (old_url != null)
					{
						// Sử dụng Replace để thay thế phần cố định "https://drive.google.com/uc?id=" bằng chuỗi trống ""
						string fileId = old_url.Replace("https://drive.google.com/uc?id=", "");
						_googleDriveService.DeleteImageFromGoogleDrive(fileId);
					}
				}
				// Đợi tất cả các tác vụ xóa hình ảnh từ Google Drive hoàn thành
        		await Task.WhenAll(deleteTasks);
			}
			// Xóa các bản ghi ProductImage
			List<int> productImageIds = await _productImageService.GetIdByProductId(id);
			if (productImageIds != null){
				foreach (int productImageId in productImageIds){
					await _productImageService.Delete(productImageId);
				}
			}
			//Xóa các tag gắn với sản phẩm
			List<int> producttagIds = await _productTagService.GetByProductId(id);
			if (producttagIds != null){
				foreach (int producttagId in producttagIds){
					await _productTagService.Delete(producttagId);
				}
			}
			var sql = @"
				DELETE FROM Products
				WHERE product_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}
    }
}