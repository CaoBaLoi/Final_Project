using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IImportProductService{
        Task<IEnumerable<ImportProduct>> GetAll();
		Task Create(ImportProductDTO importProductDTO);
		Task Update(ImportProduct importProduct);
    }
    public class ImportProductService(IImportProductRepository importProductRepository) : IImportProductService
    {
        private readonly IImportProductRepository _importProductRepository = importProductRepository;
        public async Task Create(ImportProductDTO importProductDTO)
        {
            await _importProductRepository.Create(importProductDTO);
        }

        public async Task<IEnumerable<ImportProduct>> GetAll()
        {
            return await _importProductRepository.GetAll();
        }

        public async Task Update(ImportProduct importProduct)
        {
            await _importProductRepository.Update(importProduct);
        }
    }
}