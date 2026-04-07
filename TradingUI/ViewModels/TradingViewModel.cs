using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using TradingLibrary.Models;
using TradingUI.Infrastructure;
using TradingUI.Services;

namespace TradingUI.ViewModels
{
    public class TradingViewModel : ViewModelBase
    {
        private readonly IUiDispatcher _uiDispatcher;
        private readonly ITradingApiService _tradingApiService;
        private readonly ITradingSignalRService _tradingSignalRService;
        private AccountItemViewModel _selectedAccount;
        private AccountDto _accountModel;
        private bool _isBalanceChanged;
        private string _connectionStatus;

        public TradingViewModel(ITradingApiService tradingApiService, ITradingSignalRService tradingSignalRService, IUiDispatcher uiDispatcher)
        {
            _tradingApiService = tradingApiService;
            _tradingSignalRService = tradingSignalRService;
            _uiDispatcher = uiDispatcher;
            ConnectionStatus = string.Empty;
            _selectedAccount = new AccountItemViewModel(); 
            _accountModel = new AccountDto();
            _ = LoadData();
            _tradingSignalRService.PositionUpdated += OnPositionUpdate;
            _tradingSignalRService.ConnectionStateChanged += OnSignalRConnectionStateChanged;
            PlaceOrderCommand = new RelayCommand(ShowPlaceOrderPopupView, CanPlaceOrder);
            RefreshCommand = new RelayCommand(async () => await LoadAccountDetails(SelectedAccount?.AccountId ?? 0));
        }

        public ObservableCollection<PositionItemViewModel> PositionDetails { get; set; } = [];
        public ObservableCollection<AccountItemViewModel> Accounts { get; set; } = [];
        public ICommand PlaceOrderCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public AccountItemViewModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                _ = LoadAccountDetails(_selectedAccount.AccountId);
                ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        public bool IsBalanceChanged
        {
            get => _isBalanceChanged;
            set => SetProperty(ref _isBalanceChanged, value);
        }

        private void OnSignalRConnectionStateChanged(string state)
        {
            _uiDispatcher.Invoke(() =>
            {
                ConnectionStatus = state;
            });
        }
        private bool CanPlaceOrder()
        {
            return SelectedAccount.CashBalance > 0 || PositionDetails?.Count > 0;
        }
        private async Task LoadData()
        {
            await LoadAccounts();

            if (Accounts.Count > 0)
            {
                SelectedAccount = Accounts[0]; // Set the first _account as selected by default
                await LoadAccountDetails(Accounts[0].AccountId); // Load positions for the first _account by default
            }
        }

        private async Task LoadAccountDetails(int accountId)
        {
            _accountModel = await _tradingApiService.GetAccountDetailsAsync(accountId);
            if(_accountModel == null)
            {
                return;
            }

            _uiDispatcher.Invoke(() => 
            { 
                PositionDetails.Clear();
                foreach (var position in _accountModel.Positions)
                {
                    PositionDetails.Add(new PositionItemViewModel(position));
                }
            });
        }
        private async Task LoadAccounts()
        {
            var accounts = await _tradingApiService.GetAccountsAsync();
            _uiDispatcher.Invoke(() =>
            {
                Accounts.Clear();
                foreach (var account in accounts)
                {
                    Accounts.Add(new AccountItemViewModel(account));
                }
            });
        }

        private void ShowPlaceOrderPopupView()
        {
            var placeOrderPopupViewModel = new PlaceOrderPopupViewModel(_tradingApiService,_uiDispatcher, _accountModel);
            var placeOrderPopupView = new PlaceOrderPopupView(placeOrderPopupViewModel);
            placeOrderPopupView.Show();
        }

        private void OnPositionUpdate(PositionDto position)
        {
            _uiDispatcher.Invoke(() =>
            {
                var existing = PositionDetails.FirstOrDefault(x => x.PositionId == position.PositionId);

                if (existing == null)
                {
                    PositionDetails.Add(new PositionItemViewModel(position));
                    _accountModel.Positions.Add(position);
                }
                else
                {
                    PositionDetails.Remove(existing);
                    _accountModel.Positions.Remove(position);
                    existing.Quantity = position.Quantity;
                    existing.AverageCostPerShare = position.AverageCostPerShare;
                    existing.CurrentValue = position.CurrentValue;

                    // add back the updated item
                    if (position.Quantity > 0)
                    {
                        PositionDetails.Add(existing);
                        _accountModel.Positions.Add(position);
                    }
                }

                _ = TriggerBalanceFlash();
                SelectedAccount.CashBalance = _accountModel.CashBalance = position.Account.CashBalance;
                ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
            });

        }

        private async Task TriggerBalanceFlash()
        {
            IsBalanceChanged = true;
            await Task.Delay(50);
            IsBalanceChanged = false;
        }
    }
}