using TradingLibrary.Models;

namespace TradingApi.Services;

public interface IOrderService
{
    Task<string?> PlaceOrder(PlaceOrderRequest request);
}