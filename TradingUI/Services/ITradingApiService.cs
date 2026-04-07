using TradingLibrary.Models;

namespace TradingUI.Services
{
    public interface ITradingApiService
    {
        Task<IList<Account>> GetAccountsAsync(CancellationToken cancellationToken = default);
        Task<AccountDto> GetAccountDetailsAsync(int accountId, CancellationToken cancellationToken = default);
        Task<IList<Equity>> GetEquitiesAsync(CancellationToken cancellationToken = default);
        Task<string> PlaceOrderAsync(PlaceOrderRequest newOrder, CancellationToken cancellationToken = default);
    }
}
