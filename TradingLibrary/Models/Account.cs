namespace TradingLibrary.Models;

public class Account
{
    public int AccountId { get; set; }
    public string? ClientName { get; set; }
    public string? AccountNumber { get; set; }
    public decimal CashBalance { get; set; }
}