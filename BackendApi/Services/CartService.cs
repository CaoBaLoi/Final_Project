using Househole_shop.Models;
using Househole_shop.Repositories;

namespace  Househole_shop.Services{
    public interface ICartService
	{
		Task<IEnumerable<Cart>> GetCartItems(int id);
        Task AddToCart(int user_id, int product_id, int quantity);
        Task RemoveFromCart(int user_id, int product_id);
	}
    public class CartService(ICartRepository cartRepository) : ICartService{
        private readonly ICartRepository _cartRepository = cartRepository;

        public async Task AddToCart(int user_id, int product_id, int quantity)
        {
            await _cartRepository.AddToCart(user_id, product_id, quantity);
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int id)
        {
            return await _cartRepository.GetCartItems(id);
        }

        public async Task RemoveFromCart(int user_id, int product_id)
        {
            await _cartRepository.RemoveFromCart(user_id,product_id);
        }
    }
}