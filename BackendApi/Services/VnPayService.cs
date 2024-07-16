using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Services{
    public interface IVnPayService{
        string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel vnPaymentRequestModel);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
    public class VnPayService : IVnPayService
    {
        private IConfiguration _config;

        public VnPayService(IConfiguration config){
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel vnPaymentRequestModel)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            if (_config == null)
            {
                throw new InvalidOperationException("Configuration is not loaded properly.");
            }

            string version = _config["VnPay:Version"] ?? throw new InvalidOperationException("VnPay:Version is not configured.");
            string command = _config["VnPay:Command"] ?? throw new InvalidOperationException("VnPay:Command is not configured.");
            string tmnCode = _config["VnPay:TmnCode"] ?? throw new InvalidOperationException("VnPay:TmnCode is not configured.");
            string currCode = _config["VnPay:CurrCode"] ?? throw new InvalidOperationException("VnPay:CurrCode is not configured.");
            string locale = _config["VnPay:Locale"] ?? throw new InvalidOperationException("VnPay:Locale is not configured.");
            string returnUrl = _config["VnPay:ReturnUrl"] ?? throw new InvalidOperationException("VnPay:ReturnUrl is not configured.");
            string baseUrl = _config["VnPay:BaseUrl"] ?? throw new InvalidOperationException("VnPay:BaseUrl is not configured.");
            string hashSecret = _config["VnPay:HashSecret"] ?? throw new InvalidOperationException("VnPay:HashSecret is not configured.");

            vnpay.AddRequestData("vnp_Version", version);
            vnpay.AddRequestData("vnp_Command", command);
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_Amount", (vnPaymentRequestModel.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", vnPaymentRequestModel.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", currCode);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
            vnpay.AddRequestData("vnp_Locale", locale);
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán cho đơn hàng:{vnPaymentRequestModel.order_id} gồm sản phẩm:{vnPaymentRequestModel.product_id} có số lượng:{vnPaymentRequestModel.order_detail_quantity}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(baseUrl, hashSecret);


            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            
            string hashSecret = _config["VnPay:HashSecret"] ?? throw new InvalidOperationException("Hash secret is missing.");
            string SecureHash = vnp_SecureHash.ToString() ?? throw new InvalidOperationException("Secure hash is missing.");

            bool checkSignature = vnpay.ValidateSignature(SecureHash, hashSecret);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }

            return new VnPaymentResponseModel {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
            };
        }
    }
}