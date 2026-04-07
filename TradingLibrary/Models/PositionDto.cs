namespace TradingLibrary.Models;

public class PositionDto
{
    public int PositionId { get; set; }
    public Account Account { get; set; } = new Account();
    public Equity Equity { get; set; } = new Equity();
    public int Quantity { get; set; }
    public decimal AverageCostPerShare { get; set; }
    public decimal CurrentValue { get; set; }

    public override string ToString()
    {
         return $"PositionId={PositionId}, AccountId={Account.AccountId}, EquityId={Equity.EquityId}, Quantity={Quantity}, AverageCostPerShare={AverageCostPerShare}, CurrentValue={CurrentValue}";
    }
}