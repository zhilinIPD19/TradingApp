using System;
using System.Collections.Generic;
using System.Text;

namespace TradingLibrary.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string? ClientName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal CashBalance { get; set; }
        public List<PositionDto> Positions { get; set; } = new List<PositionDto>();
    }
}
