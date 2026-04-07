namespace TradingLibrary.Models;

public class PlaceOrderRequest
{
    public int AccountId { get; set; }
    public int EquityId { get; set; }
    public int Quantity { get; set; }
    public decimal LimitPrice { get; set; }
    public OrderType OrderType { get; set; } // "Buy" or "Sell"
}