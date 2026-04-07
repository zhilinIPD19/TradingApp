using System.Net.Http;
using System.Net.Http.Json;
using TradingLibrary.Models;

namespace TradingUI.Services
{
    public class TradingApiService(HttpClient httpClient) : ITradingApiService
    {
        public async Task<IList<Account>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            var result = await httpClient.GetFromJsonAsync<IList<Account>>("accounts", cancellationToken);
            return result ?? [];
        }

        public async Task<IList<Equity>> GetEquitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await httpClient.GetFromJsonAsync<IList<Equity>>("equities", cancellationToken);
            return result ?? [];
        }

        public async Task<AccountDto> GetAccountDetailsAsync(int accountId, CancellationToken cancellationToken = default)
        {
            var result = await httpClient.GetFromJsonAsync<AccountDto>($"accountDetails/{accountId}", cancellationToken);
            return result ?? new AccountDto();
        }

        public async Task<string> PlaceOrderAsync(PlaceOrderRequest newOrder, CancellationToken cancellationToken = default)
        {
            var key = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, "orders")
            {
                Content = JsonContent.Create(newOrder)
            };

            request.Headers.Add("Idempotency-Key", key);

            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                return "Order placed successfully";
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                return error;
            }
        }
    }
}
