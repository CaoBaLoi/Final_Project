using Grocery.DTOs;
using Grocery.Models;
using Grocery.Repositories;

namespace Grocery.Services{
    public interface ISaleService
	{
		Task<IEnumerable<Sale>> GetAll();
		Task Create(SaleDTO saleDTO);
		Task Update(Sale sale);
		Task Delete(int id);
	}
	public class SaleService(ISaleRepository saleRepository) : ISaleService
	{
		private readonly ISaleRepository _saleRepository = saleRepository;


        public async Task Create(SaleDTO saleDTO)
		{
			await _saleRepository.Create(saleDTO);
		}

		public async Task Delete(int id)
		{
			await _saleRepository.Delete(id);
		}

		public async Task<IEnumerable<Sale>> GetAll()
		{
			return await _saleRepository.GetAll();
		}

        public async Task Update(Sale sale)
		{
			await _saleRepository.Update(sale);
		}
	}
}