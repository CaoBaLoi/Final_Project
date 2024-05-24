using Grocery.Models;
using Grocery.Repositories;

namespace Grocery.Services{
    public interface ICategoryService
	{
		Task<IEnumerable<Category>> GetAll();
		Task<Category?> GetById(int id);
		Task Create(Category category);
		Task Update(Category category);
		Task Delete(int id);
		Task<Category?> GetByName(string categoryName);
		Task<int> GetOrCreateCategoryId(string categoryName);
		//Task<Category?> CategoryExits(string categoryName);
	}
	public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository = categoryRepository;


        public async Task Create(Category category)
		{
			await _categoryRepository.Create(category);
		}

		public async Task Delete(int id)
		{
			await _categoryRepository.Delete(id);
		}

		public async Task<IEnumerable<Category>> GetAll()
		{
			return await _categoryRepository.GetAll();
		}

		public async Task<Category?> GetById(int id)
		{
			return await _categoryRepository.GetById(id);
		}

        public async Task<Category?> GetByName(string categoryName)
        {
            return await _categoryRepository.GetByName(categoryName);
        }

        // public async Task<Category?> CategoryExits(string categoryName)
        // {
        //     return await _categoryRepository.CategoryExits(categoryName);
        // }

        public async Task<int> GetOrCreateCategoryId(string categoryName)
        {
            return await _categoryRepository.GetOrCreateCategoryId(categoryName);
        }

        public async Task Update(Category category)
		{
			await _categoryRepository.Update(category);
		}
	}
}