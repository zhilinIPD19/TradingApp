using Microsoft.AspNetCore.Mvc;
using TradingApi.Data;
using TradingApi.Services;
using TradingLibrary.Models;

namespace TradingApi.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var valid = ValidateIdempotencyKey(request, out var message);
            if (!valid)
            {
                return BadRequest(new { success = false, message });
            }

            message = await _orderService.PlaceOrder(request);

            if (message == null)
            {
                return Ok(new { success = true, message = "Order executed" });
            }
            else
            {
                return BadRequest(new { success = false, message });
            }
        }

        private bool ValidateIdempotencyKey(PlaceOrderRequest request, out string? message)
        {
            message = null;
            var key = Request.Headers["Idempotency-Key"].FirstOrDefault();
            if(string.IsNullOrEmpty(key))
            {
                message = "Idempotency-Key header is required";
                return false;
            }

            if (IdempotencyStore.TryGet(key, out _))
            {
                message = "Duplicate request";
                return false;
            }

            return true;
        }
    }
}
