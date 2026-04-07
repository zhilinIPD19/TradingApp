namespace TradingLibrary.Models;

public class Position
{
    public int PositionId { get; set; }
    public int AccountId { get; set; }
    public int EquityId { get; set; }
    public int Quantity { get; set; }
    public decimal AverageCostPerShare { get; set; }
}
