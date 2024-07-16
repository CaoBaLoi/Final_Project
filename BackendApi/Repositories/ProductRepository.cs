using Dapper;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Househole_shop.Services;

namespace Househole_shop.Repositories{
    public interface IProductRepository
	{
		Task<IEnumerable<GetProductDTO>> GetAll();
		Task<GetProductDTO?> GetById(int id);
		Task Create(ProductDTO productDTO);
		Task Update(ProductDTO productDTO);
		Task Delete(int id);
		Task UpdateQuantity(int product_id, int quantity);
		Task<int> GetQuantity(int product_id);
	}
    public class ProductRepository(DataContext context, TagService tagService, CategoryService categoryService, ProductTagService productTagService, ProductImageService productImageService, CloudinaryService cloudinaryService) : IProductRepository
	{
		private readonly DataContext _context = context;
        private readonly TagService _tagService = tagService;
		private readonly CategoryService _categoryService = categoryService;
		private readonly ProductTagService _productTagService = productTagService;
		private readonly ProductImageService _productImageService = productImageService;
		private readonly CloudinaryService _cloudinaryService = cloudinaryService;
		public async Task<IEnumerable<GetProductDTO>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT 
					p.*, 
					pi.image_url, 
					c.category_name,
					GROUP_CONCAT(t.tag_name SEPARATOR ', ') AS tag_names
				FROM 
					Products p
				LEFT JOIN 
					ProductImage pi ON p.product_id = pi.product_id
				LEFT JOIN 
					Categories c ON p.category_id = c.category_id
				LEFT JOIN 
					ProductTag pt ON p.product_id = pt.product_id
				LEFT JOIN 
					Tags t ON pt.tag_id = t.tag_id
				GROUP BY 
					p.product_id, pi.image_url, c.category_name
				ORDER BY product_quantity;

			";
			return await connection.QueryAsync<GetProductDTO>(sql);
		}


		public async Task<GetProductDTO?> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT 
					p.*, 
					pi.image_url, 
					c.category_name,
					GROUP_CONCAT(t.tag_name SEPARATOR ', ') AS tag_names
				FROM 
					Products p
				LEFT JOIN 
					ProductImage pi ON p.product_id = pi.product_id
				LEFT JOIN 
					Categories c ON p.category_id = c.category_id
				LEFT JOIN 
					ProductTag pt ON p.product_id = pt.product_id
				LEFT JOIN 
					Tags t ON pt.tag_id = t.tag_id
				WHERE p.product_id = @id
				GROUP BY 
					p.product_id, pi.image_url, c.category_name;
				
			";
			var result =  await connection.QuerySingleOrDefaultAsync<GetProductDTO>(sql, new { id });
			if (result == null)
			{
				throw new Exception($"Không tìm thấy sản phẩm với id = {id}");
			}
			return result;
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
				imageUrl = _cloudinaryService.UploadImage(imageFile);
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
			List<string> old_urls = await _productImageService.GetUrlByProductId(productId);
			if (old_urls != null)
			{
				foreach (string old_url in old_urls)
				{
					if (old_url != null)
					{
						string fileId = old_url.Replace("http://res.cloudinary.com/dkhf0nsxl/image/upload/v1718208527/", "");
						int extensionIndex = fileId.LastIndexOf(".");
						if (extensionIndex != -1)
						{
							fileId = fileId.Substring(0, extensionIndex);
						}
						_cloudinaryService.DeleteImage(fileId);
					}
				}
			}
			List<int> productImageIds = await _productImageService.GetIdByProductId(productId);
			if (productImageIds != null){
				foreach (int productImageId in productImageIds){
					await _productImageService.Delete(productImageId);
				}
			}
			List<string> image_urls = new List<string>();
			foreach (var imageFile in productDTO.image_file)
			{
				string? imageUrl = _cloudinaryService.UploadImage(imageFile);
				if (imageUrl != null)
				{
					image_urls.Add(imageUrl);
				}
			}

			foreach (string image_url in image_urls)
			{
				await _productImageService.Create(new ProductImage { product_id = productId, image_url = image_url});
			}
		}


		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			List<string> old_urls = await _productImageService.GetUrlByProductId(id);
			if (old_urls != null)
			{
				var deleteTasks = new List<Task>();
				foreach (string old_url in old_urls)
				{
					if (old_url != null)
					{
						string fileId = old_url.Replace("http://res.cloudinary.com/dkhf0nsxl/image/upload/v1718208527/", "");
						int extensionIndex = fileId.LastIndexOf(".");
						if (extensionIndex != -1)
						{
							fileId = fileId.Substring(0, extensionIndex);
						}

						_cloudinaryService.DeleteImage(fileId);
					}
				}
        		await Task.WhenAll(deleteTasks);
			}
			List<int> productImageIds = await _productImageService.GetIdByProductId(id);
			if (productImageIds != null){
				foreach (int productImageId in productImageIds){
					await _productImageService.Delete(productImageId);
				}
			}

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

        public async Task UpdateQuantity(int product_id, int quantity)
        {
            using var connection = _context.CreateConnection();
			var product_quantity = await GetQuantity(product_id);
			var sql = @"
				UPDATE Products
				SET product_quantity = @quantity
				WHERE product_id = @product_id;
			";
			await connection.ExecuteAsync(sql, new{product_id, quantity = quantity+product_quantity});
        }

        public async Task<int> GetQuantity(int product_id)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				SELECT product_quantity FROM Products
				WHERE product_id = @product_id;
			";
			return await connection.QueryFirstOrDefaultAsync<int>(sql, new{product_id});
        }
    }
}