using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IOrderService
	{
		Task<int> Order (string userId, OrderDTO ordersDTO);
        Task<IEnumerable<Order>> GetOrder(string userId);
        Task Update ( string status, int order_id);
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetById(int id);
	}
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order?> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task<IEnumerable<Order>> GetOrder(string userId)
        {
            return await _orderRepository.GetOrder(userId);
        }

        public async Task<int> Order(string userId, OrderDTO ordersDTO)
        {
            return await _orderRepository.Order(userId,ordersDTO);
        }

        public async Task Update( string status, int order_id)
        {
            await _orderRepository.Update(status, order_id);
        }
    }
}