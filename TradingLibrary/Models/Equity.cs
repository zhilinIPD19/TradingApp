namespace TradingLibrary.Models;

public class Equity
{
    public int EquityId { get; set; }
    public string? Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
}