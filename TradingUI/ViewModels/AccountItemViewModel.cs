using TradingLibrary.Models;

namespace TradingUI.ViewModels
{
    public class AccountItemViewModel : ViewModelBase
    {
        private int _accountId;
        private string? _clientName;
        private string? _accountNumber;
        private decimal _cashBalance;

        public AccountItemViewModel()
        {
        }

        public AccountItemViewModel(Account account)
        {
            AccountId = account.AccountId;
            ClientName = account.ClientName;
            AccountNumber = account.AccountNumber;
            CashBalance = account.CashBalance;
        }
        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }
        public string? ClientName
        {
            get => _clientName;
            set => SetProperty(ref _clientName, value);
        }
        public string? AccountNumber
        {
            get => _accountNumber;
            set => SetProperty(ref _accountNumber, value);
        }

        public decimal CashBalance
        {
            get => _cashBalance;
            set => SetProperty(ref _cashBalance, value);
        }
    }
}
