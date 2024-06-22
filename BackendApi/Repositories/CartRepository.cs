using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface ICartRepository
	{
		Task<IEnumerable<Cart>> GetCartItems(int id);
        Task AddToCart(int user_id, int product_id, int quantity);
        Task RemoveFromCart(int user_id, int product_id);
	}
    public class CartRepository(DataContext context) : ICartRepository
    {
        private readonly DataContext _context = context;
        public async Task AddToCart(int user_id, int product_id, int quantity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"IF EXISTS (SELECT 1 FROM Cart WHERE user_id = @user_id AND product_id = @product_id)
                        BEGIN
                            UPDATE Cart SET quantity = quantity + @quantity 
                            WHERE user_id = @user_id AND product_id = @product_id
                        END
                        ELSE
                        BEGIN
                            INSERT INTO Cart (user_id, product_id, quantity) 
                            VALUES (@user_id, @product_id, @quantity)
                        END";
            await connection.ExecuteAsync(sql, new { user_id, product_id, cart_quantity = quantity });
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT ci.*, p.product_name, p.product_price 
                            FROM Cart ci
                            JOIN Products p ON ci.product_id = p.product_id
                            WHERE ci.user_id = @Id";
            return await connection.QueryAsync<Cart, Product, Cart>(
                sql,
                (cart, product) =>
                {
                    cart.Product = product;
                    return cart;
                },
                new { user_id = id },
                splitOn: "product_id"
            );
        }

        public async Task RemoveFromCart(int user_id, int product_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"DELETE FROM Cart WHERE user_id = @user_id AND product_id = @product_id";
            await connection.ExecuteAsync(sql, new { user_id, product_id });
        }
    }
}