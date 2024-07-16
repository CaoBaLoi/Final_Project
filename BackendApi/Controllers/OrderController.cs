using System.Text.RegularExpressions;
using System.Web;
using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(OrderService orderService, UserManager<User> userManager, VnPayService vnPayService, ProductService productService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;
        private readonly UserManager<User> _userManager = userManager;
        private readonly VnPayService _vnPayService = vnPayService;
        private readonly ProductService _productService = productService;

        [HttpPost("place")]
        public async Task<IActionResult> Order([FromBody] OrderDTO ordersDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized(); 
            }
            try
            {
                if(ordersDTO.payment_method == "vnpay"){
                    
                    try
                    {
                        var order_id = await _orderService.Order(user.Id, ordersDTO);
                        var productIds = string.Join(",", ordersDTO.orderDetails.Select(d => d.product_id));
                        var quantities = string.Join(",", ordersDTO.orderDetails.Select(d => d.order_detail_quantity));
                        foreach (var detail in ordersDTO.orderDetails)
                        {
                            await _productService.UpdateQuantity(detail.product_id, -detail.order_detail_quantity);
                        }
                        var vnPayModel = new VnPaymentRequestModel{
                            Amount = ordersDTO.order_total_price,
                            CreatedDate = DateTime.Now,
                            order_id = order_id,
                            product_id = productIds,
                            order_detail_quantity = quantities
                        };
                        var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                        return Ok(new { paymentUrl });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "Error processing VNPay payment."+ex);
                    }
                }
                else
                {
                    var order_Id = await _orderService.Order(user.Id, ordersDTO);
                    foreach (var detail in ordersDTO.orderDetails)
                    {
                        await _productService.UpdateQuantity(detail.product_id, -detail.order_detail_quantity);
                    }
                    
                    return Ok(order_Id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("vnpay-callback")]
        public async Task<IActionResult> VNPayCallback()
        {
            try
            {
                var response = await Task.Run(() => _vnPayService.PaymentExecute(Request.Query));

                if (response == null || response.VnPayResponseCode != "00")
                {
                    var status = "Đã hủy";
                    string decodedVnpOrderInfo = HttpUtility.UrlDecode(response.OrderDescription ?? string.Empty);
                    int orderId = 0;
                    string productIds = "";
                    string quantities="";
                    string pattern = @"đơn hàng:(\d+) gồm sản phẩm:(.+) có số lượng:(.+)";
                    Match match = Regex.Match(decodedVnpOrderInfo, pattern);
                    if (match.Success)
                    {
                        orderId = int.Parse(match.Groups[1].Value);
                        productIds = match.Groups[2].Value;
                        quantities = match.Groups[3].Value;
                    }
                    else
                    {
                        Console.WriteLine("No match found.");
                    }
                    await _orderService.Update( status, orderId);
                    string[] productIdArray = productIds.Split(',');
                    string[] quantityArray = quantities.Split(',');
                    for (int i = 0; i < productIdArray.Length; i++)
                    {
                        string productId = productIdArray[i].Trim();
                        string quantity = quantityArray[i].Trim();

                        int productIdInt = int.Parse(productId);
                        int quantityInt = int.Parse(quantity);

                        await _productService.UpdateQuantity(productIdInt,quantityInt);
                    }
                    string script = "<script>window.location.href = 'http://localhost:5173/user/order';alert('Thanh to\\u00E1n kh\\u00F4ng th\\u00E0nh c\\u00F4ng. \\u0110\\u01A1n h\\u00E0ng \\u0111\\u00E3 b\\u1ECB h\\u1EE7y'); </script>";
                    return Content(script, "text/html");
                }
                else
                {
                    string script = "<script>window.location.href = 'http://localhost:5173/user/order';alert('\\u0110\\u1eb7t h\\u00E0ng th\\u00E0nh c\\u00F4ng'); </script>";
                    return Content(script, "text/html");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(){
            try
            {
                var orders = await _orderService.GetAll();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("by-id")]
        public async Task<IActionResult> GetById(){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); 
            }
            try
            {
                var orders = await _orderService.GetOrder(user.Id);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
         [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(string status, int order_id){
            try
            {
                await _orderService.Update(status, order_id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}