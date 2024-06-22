using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IProductTagService
	{
		Task<IEnumerable<ProductTag>> GetAll();
		Task<ProductTag?> GetById(int id);
		Task Create(ProductTag productTag);
		Task Update(ProductTag productTag);
		Task<List<int>> GetByProductId(int productId);
		//Task<bool> ProductTagExit(int productId, int tagId);
		Task Delete(int id);
	}
	public class ProductTagService(IProductTagRepository productTagRepository) : IProductTagService
	{
		private readonly IProductTagRepository _productTagRepository = productTagRepository;


        public async Task Create(ProductTag productTag)
		{
			await _productTagRepository.Create(productTag);
		}

		public async Task Delete(int id)
		{
			await _productTagRepository.Delete(id);
		}

		public async Task<IEnumerable<ProductTag>> GetAll()
		{
			return await _productTagRepository.GetAll();
		}

		public async Task<ProductTag?> GetById(int id)
		{
			return await _productTagRepository.GetById(id);
		}

        public async Task<List<int>> GetByProductId(int productId)
        {
            return await _productTagRepository.GetByProductId(productId);
        }

        // public async Task<bool> ProductTagExit(int productId, int tagId)
        // {
        //     return await _productTagRepository.ProductTagExit(productId, tagId);
        // }

        public async Task Update(ProductTag productTag)
		{
			await _productTagRepository.Update(productTag);
		}
	}
}