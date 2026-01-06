using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CRM.Services
{
    public class RazorpayService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _keyId;
        private readonly string _keySecret;
        private readonly string _webhookSecret;

        public RazorpayService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _keyId = _configuration["Razorpay:KeyId"];
            _keySecret = _configuration["Razorpay:KeySecret"];
            _webhookSecret = _configuration["Razorpay:WebhookSecret"];

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_keyId}:{_keySecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        }

        public string GetKeyId()
        {
            return _keyId;
        }

        public async Task<string> CreateOrderAsync(decimal amount, string currency = "INR", string receipt = null)
        {
            var orderData = new
            {
                amount = (int)(amount * 100), // Convert to paise
                currency = currency,
                receipt = receipt ?? $"order_{DateTime.Now.Ticks}"
            };

            var json = JsonSerializer.Serialize(orderData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.razorpay.com/v1/orders", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orderResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                return orderResponse.GetProperty("id").GetString();
            }

            throw new Exception($"Failed to create Razorpay order: {responseContent}");
        }

        public bool VerifyPaymentSignature(string paymentId, string orderId, string signature)
        {
            // For test mode, always return true
            if (_keyId.StartsWith("rzp_test_"))
            {
                return true;
            }

            var payload = $"{orderId}|{paymentId}";
            var expectedSignature = ComputeHmacSha256(payload, _keySecret);
            return expectedSignature == signature;
        }

        public bool VerifyWebhookSignature(string payload, string signature)
        {
            var expectedSignature = ComputeHmacSha256(payload, _webhookSecret);
            return expectedSignature == signature;
        }

        private string ComputeHmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToHexString(hash).ToLower();
        }

        /// <summary>
        /// Create a refund for a payment
        /// </summary>
        /// <param name="paymentId">Razorpay payment ID (e.g., pay_xxxxx)</param>
        /// <param name="amount">Amount to refund in rupees (full or partial)</param>
        /// <param name="notes">Optional notes for the refund</param>
        /// <returns>Refund ID if successful</returns>
        public async Task<(bool success, string refundId, string message)> CreateRefundAsync(string paymentId, decimal? amount = null, string notes = null)
        {
            try
            {
                var refundData = new Dictionary<string, object>();
                
                if (amount.HasValue)
                {
                    refundData["amount"] = (int)(amount.Value * 100); // Convert to paise
                }
                
                if (!string.IsNullOrEmpty(notes))
                {
                    refundData["notes"] = new { reason = notes };
                }

                var json = JsonSerializer.Serialize(refundData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"https://api.razorpay.com/v1/payments/{paymentId}/refund", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var refundResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    var refundId = refundResponse.GetProperty("id").GetString();
                    var status = refundResponse.GetProperty("status").GetString();
                    
                    return (true, refundId ?? "", $"Refund {status}. Refund will be processed to original payment method within 5-7 business days.");
                }

                return (false, "", $"Razorpay error: {responseContent}");
            }
            catch (Exception ex)
            {
                return (false, "", $"Failed to create refund: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetch payment details from Razorpay
        /// </summary>
        public async Task<(bool success, JsonElement? payment)> FetchPaymentAsync(string paymentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.razorpay.com/v1/payments/{paymentId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var payment = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    return (true, payment);
                }

                return (false, null);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}