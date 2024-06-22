using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IProductImageService
	{
		Task<IEnumerable<ProductImage>> GetAll();
		//Task<ProductImage?> GetById(int id);
		Task Create(ProductImage productImage);
		Task Update(ProductImage productImage);
		Task<List<string>> GetUrlByProductId (int productId);
		Task<List<int>> GetIdByProductId(int productId);
		Task Delete(int id);
	}
	public class ProductImageService(IProductImageRepository productImageRepository) : IProductImageService
	{
		private readonly IProductImageRepository _productImageRepository = productImageRepository;


        public async Task Create(ProductImage productImage)
		{
			await _productImageRepository.Create(productImage);
		}

		public async Task Delete(int id)
		{
			await _productImageRepository.Delete(id);
		}

		public async Task<IEnumerable<ProductImage>> GetAll()
		{
			return await _productImageRepository.GetAll();
		}

        public async Task<List<int>> GetIdByProductId(int productId)
        {
            return await _productImageRepository.GetIdByProductId(productId);
        }

        public async Task<List<string>> GetUrlByProductId(int productId)
        {
            return await _productImageRepository.GetUrlByProductId(productId);
        }

        // public async Task<ProductImage?> GetById(int id)
        // {
        // 	return await _productImageRepository.GetById(id);
        // }

        public async Task Update(ProductImage productImage)
		{
			await _productImageRepository.Update(productImage);
		}
	}
}