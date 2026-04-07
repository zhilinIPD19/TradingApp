using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TradingLibrary.Models;
using TradingUI.Infrastructure;
using TradingUI.Services;

namespace TradingUI.ViewModels
{
    public class PlaceOrderPopupViewModel : ViewModelBase
    {
        private readonly ITradingApiService _tradingApiService;
        private readonly IUiDispatcher _uiDispatcher;
        private readonly AccountDto _account;
        private string _limitPriceValidateMessage = string.Empty;
        private string _quantityValidateMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _equityMessage = string.Empty;

        public ObservableCollection<Equity> Equities { get; set; } = [];
        public List<OrderType> OrderTypes { get; set; } = [];

        public string LimitPriceValidateMessage
        {
            get => _limitPriceValidateMessage;
            set => SetProperty(ref _limitPriceValidateMessage, value);
        }
        public string QuantityValidateMessage
        {
            get => _quantityValidateMessage;
            set => SetProperty(ref _quantityValidateMessage, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public string EquityMessage
        {
            get => _equityMessage;
            set => SetProperty(ref _equityMessage, value);
        }
        public string AccountInfo { get; set; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand CancelCommand { get; }
        public string QuantityInput { get; set; }
        public string LimitPriceInput { get; set; }
        public OrderType SelectedOrderType { get; set; }
        public Equity SelectedEquity { get; set; }
        public event Action? RequestClose;
        
        public PlaceOrderPopupViewModel(ITradingApiService tradingApiService, IUiDispatcher uiDispatcher, AccountDto account) 
        { 
            _tradingApiService = tradingApiService;
            _uiDispatcher = uiDispatcher;
            _ = LoadEquities();
            this._account = account;
            QuantityInput = "0";
            LimitPriceInput = "0.00";
            SelectedEquity = new Equity();
            AccountInfo = $"Account Number: {this._account.AccountNumber} -  Current Balance: {this._account.CashBalance}";
            OrderTypes = [.. Enum.GetValues<OrderType>()];

            PlaceOrderCommand = new RelayCommand(OnPlaceOrder);
            CancelCommand = new RelayCommand(CloseDialog);
        }

        private async Task LoadEquities()
        {
            var equities = await _tradingApiService.GetEquitiesAsync();
            _uiDispatcher.Invoke(async () =>
            {
                Equities.Clear();
                foreach (var equity in equities)
                {
                    Equities.Add(equity);
                }
            });
        }

        private void OnPlaceOrder()
        {
            _ = PlaceOrderAsync();
        }
        
        private void CloseDialog()
        {
            RequestClose?.Invoke();
        }

        private async Task PlaceOrderAsync()
        {
            try
            {
                if(!ValidateOrder(out int quantity, out decimal limitPrice))
                {
                    return;
                }

                var newOrder = new PlaceOrderRequest
                {
                    AccountId = _account.AccountId,
                    Quantity = quantity,
                    OrderType = SelectedOrderType,
                    EquityId = SelectedEquity.EquityId,
                    LimitPrice = limitPrice,
                };

                LimitPriceValidateMessage = await _tradingApiService.PlaceOrderAsync(newOrder);
                if(LimitPriceValidateMessage.Contains("successfully"))
                {
                    CloseDialog();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show a message to the user)
                MessageBox.Show($"Error placing order: {ex.Message}");
            }
        }

        private bool ValidateOrder(out int quantity, out decimal limitPrice)
        {
            if(SelectedEquity.EquityId <= 0)
            {
                EquityMessage += "Please select an equity.";
            }

            QuantityValidateMessage = string.Empty;
            LimitPriceValidateMessage = string.Empty;
            ErrorMessage = string.Empty;
            if (!int.TryParse(QuantityInput, out quantity) || quantity <= 0)
            {
                QuantityValidateMessage += "Invalid Interger _quantity.";
            }

            if (SelectedOrderType == OrderType.Sell)
            {
                var position = _account.Positions.FirstOrDefault(o => o.Equity.EquityId == SelectedEquity.EquityId);
                if (position == null)
                {
                    QuantityValidateMessage += "Nothing to sell.";
                }
                else
                {
                    if(quantity > position.Quantity)
                    {
                        QuantityValidateMessage += "Not enough _quantity to sell.";
                    }
                }
            }

            if (!decimal.TryParse(LimitPriceInput, out limitPrice) || limitPrice <= 0)
            {
                LimitPriceValidateMessage += "Invalid limit price.";
            }

            if(quantity * limitPrice > _account.CashBalance)
            {
                ErrorMessage += "Insufficient funds to place this order.";
            }

            return string.IsNullOrEmpty(QuantityValidateMessage) && string.IsNullOrEmpty(LimitPriceValidateMessage) && string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
