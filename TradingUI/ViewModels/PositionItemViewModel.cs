using TradingLibrary.Models;

namespace TradingUI.ViewModels
{
    public class PositionItemViewModel : ViewModelBase
    {
        private string? _symbol;
        private int _quantity;
        private decimal _averageCostPerShare;
        private decimal _currentValue;

        public PositionItemViewModel(PositionDto position)
        {
            PositionId = position.PositionId;
            AccountId = position.Account.AccountId;
            EquityId = position.Equity.EquityId;
            Symbol = position.Equity.Symbol;
            Quantity = position.Quantity;
            AverageCostPerShare = position.AverageCostPerShare;
            CurrentPrice = position.Equity.CurrentPrice;
            CurrentValue = position.CurrentValue;
        }

        public int PositionId { get; set; }
        public int AccountId { get; set; }
        public int EquityId { get; set; }
        public string? Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public decimal AverageCostPerShare
        {
            get => _averageCostPerShare;
            set => SetProperty(ref _averageCostPerShare, value);
        }

        public decimal CurrentPrice { get; set; }

        public decimal CurrentValue
        {
            get => _currentValue;
            set => SetProperty(ref _currentValue, value);
        }
    }
}
