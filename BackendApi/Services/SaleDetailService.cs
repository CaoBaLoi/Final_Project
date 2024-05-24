using Grocery.Models;
using Grocery.Repositories;

namespace Grocery.Services{
    public interface ISaleDetailService
	{
		Task<IEnumerable<SaleDetail>> GetAll();
		Task Create(SaleDetail SaleDetail);
		Task Update(SaleDetail SaleDetail);
		Task Delete(int id);
	}
	public class SaleDetailService(ISaleDetailRepository saleDetailRepository) : ISaleDetailService
	{
		private readonly ISaleDetailRepository _saleDetailRepository = saleDetailRepository;


        public async Task Create(SaleDetail saleDetail)
		{
			await _saleDetailRepository.Create(saleDetail);
		}

		public async Task Delete(int id)
		{
			await _saleDetailRepository.Delete(id);
		}

		public async Task<IEnumerable<SaleDetail>> GetAll()
		{
			return await _saleDetailRepository.GetAll();
		}

        public async Task Update(SaleDetail saleDetail)
		{
			await _saleDetailRepository.Update(saleDetail);
		}
	}
}