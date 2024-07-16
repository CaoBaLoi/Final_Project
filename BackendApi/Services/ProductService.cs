using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IProductService
	{
		Task<IEnumerable<GetProductDTO>> GetAll();
		Task<GetProductDTO?> GetById(int id);
		Task Create(ProductDTO productDTO);
		Task Update(ProductDTO productDTO);
		Task Delete(int id);
		Task UpdateQuantity(int product_id, int quantity);
		Task<int> GetQuantity(int product_id);
	}
	public class ProductService(IProductRepository productRepository) : IProductService
	{
		private readonly IProductRepository _productRepository = productRepository;
		public async Task Create(ProductDTO productDTO)
		{
			await _productRepository.Create(productDTO);
		}

		public async Task Delete(int id)
		{
			await _productRepository.Delete(id);
		}

		public async Task<IEnumerable<GetProductDTO>> GetAll()
		{
			return await _productRepository.GetAll();
		}

		public async Task<GetProductDTO?> GetById(int id)
		{
			return await _productRepository.GetById(id);
		}

        public async Task<int> GetQuantity(int product_id)
        {
            return await _productRepository.GetQuantity(product_id);
        }

        public async Task Update(ProductDTO productDTO)
		{
			await _productRepository.Update(productDTO);
		}

        public async Task UpdateQuantity(int product_id, int quantity)
        {
            await _productRepository.UpdateQuantity(product_id,quantity);
        }
    }
}