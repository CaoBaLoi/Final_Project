using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IReceivedDateService{
        Task AddReceivedDate(int order_id, DateTime received_date);
        Task<ReceivedDate?> GetById(int id);
    }
    public class ReceivedDateService(IReceivedDateRepository receivedDateRepository) : IReceivedDateService
    {
        private readonly IReceivedDateRepository _receivedDateRepository = receivedDateRepository;
        public async Task AddReceivedDate(int order_id, DateTime received_date)
        {
            await _receivedDateRepository.AddReceivedDate(order_id, received_date);
        }

        public async Task<ReceivedDate?> GetById(int id)
        {
            return await _receivedDateRepository.GetById(id);
        }
    }
}