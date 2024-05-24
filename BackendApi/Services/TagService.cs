using Grocery.Models;
using Grocery.Repositories;

namespace Grocery.Services{
    public interface ITagService
	{
		Task<IEnumerable<Tag>> GetAll();
		Task<Tag?> GetById(int id);
		Task Create(Tag tag);
		Task Update(Tag tag);
		Task Delete(int id);
		Task<Tag?> GetByName(string tagName);
		Task<int> GetOrCreateTagId(string tagName);
		// Task<Tag?> TagExits(string tagName);
	}
	public class TagService(ITagRepository tagRepository) : ITagService
	{
		private readonly ITagRepository _tagRepository = tagRepository;
		public async Task Create(Tag tag)
		{
			await _tagRepository.Create(tag);
		}

		public async Task Delete(int id)
		{
			await _tagRepository.Delete(id);
		}

		public async Task<IEnumerable<Tag>> GetAll()
		{
			return await _tagRepository.GetAll();
		}

		public async Task<Tag?> GetById(int id)
		{
			return await _tagRepository.GetById(id);
		}

        public async Task<Tag?> GetByName(string tagName)
        {
            return await _tagRepository.GetByName(tagName);
        }

        public async Task<int> GetOrCreateTagId(string tagName)
        {
            return await _tagRepository.GetOrCreateTagId(tagName);
        }

        // public async Task<Tag?> TagExits(string tagName)
        // {
        //     return await _tagRepository.TagExits(tagName);
        // }

        public async Task Update(Tag tag)
		{
			await _tagRepository.Update(tag);
		}
    }
}