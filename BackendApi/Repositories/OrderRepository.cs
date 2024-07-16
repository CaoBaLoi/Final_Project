using System.Transactions;
using Dapper;
using Google.Protobuf.WellKnownTypes;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Househole_shop.Services;

namespace Househole_shop.Repositories{
    public interface IOrderRepository
	{
        Task<int> Order (string userId, OrderDTO orderDTO);
        Task<IEnumerable<Order>> GetOrder(string userId);
        Task Update(string status, int order_id);
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetById(int id);
	}
    public class OrderRepository(DataContext context, ProductService productService): IOrderRepository{
        private readonly DataContext _context = context;
        private readonly ProductService _productService = productService;

        public async Task<IEnumerable<Order>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT *FROM Orders
                ORDER BY orders_id DESC
            ";

            var orders = (await connection.QueryAsync<Order>(sql)).ToList();

            var sqlOrderDetails = @"
                SELECT 
                    od.order_id ,
                    od.product_id ,
                    od.order_detail_quantity ,
                    od.order_detail_price ,
                    pi.image_url ,
                    p.product_name 
                FROM Orderdetail od
                LEFT JOIN productimage pi ON od.product_id = pi.product_id
                LEFT JOIN products p ON od.product_id = p.product_id
                WHERE od.order_id = @OrderId
            ";

            foreach (var order in orders)
            {
                var orderDetails = await connection.QueryAsync<OrderDetail>(sqlOrderDetails, new { OrderId = order.orders_id });
                order.orderDetails = orderDetails.ToList();
            }

            return orders;
}

        public async Task<Order?> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT *FROM Orders
                WHERE orders_id = @id
            ";

            var order = await connection.QueryFirstOrDefaultAsync<Order>(sql, new{id});

            if (order != null)
            {
                var sqlOrderDetails = @"
                    SELECT 
                        od.order_id,
                        od.product_id,
                        od.order_detail_quantity,
                        od.order_detail_price,
                        pi.image_url,
                        p.product_name
                    FROM Orderdetail od
                    LEFT JOIN productimage pi ON od.product_id = pi.product_id
                    LEFT JOIN products p ON od.product_id = p.product_id
                    WHERE od.order_id = @OrderId
                ";

                var orderDetails = await connection.QueryAsync<OrderDetail>(sqlOrderDetails, new { OrderId = order.orders_id });

                order.orderDetails = orderDetails.ToList();
            }
            return order;
        }
        public async Task<IEnumerable<Order>> GetOrder(string userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT *FROM Orders
                WHERE user_id = @userId
                ORDER BY orders_id DESC
            ";

            var orders = (await connection.QueryAsync<Order>(sql, new {userId})).ToList();

            var sqlOrderDetails = @"
                SELECT 
                    od.order_id ,
                    od.product_id ,
                    od.order_detail_quantity ,
                    od.order_detail_price ,
                    pi.image_url ,
                    p.product_name 
                FROM Orderdetail od
                LEFT JOIN productimage pi ON od.product_id = pi.product_id
                LEFT JOIN products p ON od.product_id = p.product_id
                WHERE od.order_id = @OrderId
            ";

            foreach (var order in orders)
            {
                var orderDetails = await connection.QueryAsync<OrderDetail>(sqlOrderDetails, new { OrderId = order.orders_id });
                order.orderDetails = orderDetails.ToList();
            }

            return orders;
        }

       public async Task<int> Order(string userId, OrderDTO ordersDTO)
        {
            using var connection = _context.CreateConnection();

            try
            {
                
                var orderDTO = new OrderDTO
                {
                    order_date = DateTime.Now,
                    order_status = ordersDTO.order_status, 
                    order_address = ordersDTO.order_address,
                    orderDetails = new List<OrderDetail>(),
                    order_total_price = ordersDTO.order_total_price,
                    payment_method = ordersDTO.payment_method
                };

                foreach (var item in ordersDTO.orderDetails)
                {
                    var product = await _productService.GetById(item.product_id);

                    if (product != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            product_id = item.product_id,
                            order_detail_quantity = item.order_detail_quantity,
                            order_detail_price = item.order_detail_price
                        };

                        orderDTO.orderDetails.Add(orderDetail);
                    }
                }

                // Save order and order details
                var sqlOrder = @"
                    INSERT INTO Orders (user_id, order_date, order_total_price, order_status, order_address, payment_method) 
                    VALUES (@user_id, @order_date, @order_total_price, @order_status, @order_address, @payment_method);
                    SELECT LAST_INSERT_ID();
                ";
                var order_id = await connection.QuerySingleAsync<int>(sqlOrder, new
                {
                    user_id = userId,
                    orderDTO.order_date,
                    orderDTO.order_total_price,
                    orderDTO.order_status,
                    orderDTO.order_address,
                    orderDTO.payment_method
                });

                var sqlOrderDetail = @"
                    INSERT INTO Orderdetail (order_id, product_id, order_detail_quantity, order_detail_price)
                    VALUES (@order_id, @product_id, @order_detail_quantity, @order_detail_price)
                ";
                foreach (var orderDetail in orderDTO.orderDetails)
                {
                    orderDetail.order_id = order_id;
                    await connection.ExecuteAsync(sqlOrderDetail, new
                    {
                        orderDetail.order_id,
                        orderDetail.product_id,
                        orderDetail.order_detail_quantity,
                        orderDetail.order_detail_price
                    });
                }
                return order_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Order transaction: {ex.Message}");
                throw;
            }
        }

        public async Task Update(string status, int order_id)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Orders
				SET order_status = @status
				WHERE orders_id = @order_id
			";
			await connection.ExecuteAsync(sql, new{ status, order_id});
        }
    }
}