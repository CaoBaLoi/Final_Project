using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface ICartRepository
	{
		Task<IEnumerable<Cart>> GetCartItems(string user_id);
        Task AddToCart(string user_id, int product_id, int quantity);
        Task RemoveFromCart(string user_id, int product_id);
	}
    public class CartRepository(DataContext context) : ICartRepository
    {
        private readonly DataContext _context = context;
        public async Task AddToCart(string user_id, int product_id, int quantity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Cart (user_id, product_id, quantity)
                                VALUES (@user_id, @product_id, @quantity)
                                ON DUPLICATE KEY UPDATE
                                    quantity = IF(@quantity > quantity, quantity + (@quantity-quantity), quantity - (quantity-@quantity));";
            await connection.ExecuteAsync(sql, new { user_id, product_id, quantity });
        }
        public async Task<IEnumerable<Cart>> GetCartItems(string user_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                 SELECT 
                    ci.cart_id, 
                    ci.user_id, 
                    ci.product_id, 
                    ci.quantity, 
                    p.product_id AS Product_Id, 
                    p.category_id AS Category_Id,
                    p.product_name AS Product_Name, 
                    p.product_price AS Product_Price, 
                    p.product_quantity AS Product_Quantity,
                    p.product_description AS Product_Description,
                    pi.image_url 
                FROM Cart ci
                JOIN Products p ON ci.product_id = p.product_id
                LEFT JOIN ProductImage pi ON p.product_id = pi.product_id
                WHERE ci.user_id = @user_id
                ORDER BY ci.cart_id DESC";

            var cartDictionary = new Dictionary<int, Cart>();

            var result = await connection.QueryAsync<Cart, Product, string, Cart>(
                sql,
                (cart, product, imageUrl) =>
                {
                    if (!cartDictionary.TryGetValue(cart.cart_id, out var currentCart))
                    {
                        currentCart = cart;
                        currentCart.Product = product;
                        currentCart.Product.Images = new List<string>();
                        cartDictionary.Add(currentCart.cart_id, currentCart);
                    }
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        currentCart.Product.Images.Add(imageUrl);
                    }
                    return currentCart;
                },
                new { user_id },
                splitOn: "product_id,image_url"
            );

            return cartDictionary.Values;
        }


        public async Task RemoveFromCart(string user_id, int product_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"DELETE FROM Cart WHERE user_id = @user_id AND product_id = @product_id";
            await connection.ExecuteAsync(sql, new { user_id, product_id });
        }
    }
}