using TradingApi.Data;
using TradingLibrary.Models;

namespace TradingApi.Services;

public class OrderService(ITradingSignalRService tradingSignalRService, ILogger<OrderService> logger) : IOrderService
{
    public async Task<string?> PlaceOrder(PlaceOrderRequest request)
    {
        await Task.Delay(1000);

        var account = TradingDataStore.Accounts.FirstOrDefault(a => a.AccountId == request.AccountId);
        var equity = TradingDataStore.Equities.FirstOrDefault(e => e.EquityId == request.EquityId);
        var position = TradingDataStore.Positions.FirstOrDefault(p => p.AccountId == request.AccountId && p.EquityId == request.EquityId);

        if (account == null)
        {
            logger.LogWarning("Account not found: AccountId={0}", request.AccountId);
            return "Account not found";
        }

        if (equity == null)
        {
            logger.LogWarning("Equity not found: EquityId={0}", request.AccountId);
            return "Equity not found";
        }

        var cost = request.Quantity * request.LimitPrice;

        if (request.OrderType == OrderType.Buy)
        {
            if (account.CashBalance < cost)
            {
                logger.LogWarning("Insufficient funds for buy order: AccountId={0}, Required={1}, Available={2}",
                    request.AccountId, cost, account.CashBalance);
                return "Insufficient funds";
            }

            account.CashBalance -= cost;

            if (position != null)
            {
                var totalCost = position.Quantity * position.AverageCostPerShare + cost;
                position.Quantity += request.Quantity;
                position.AverageCostPerShare = totalCost / position.Quantity;
                logger.LogInformation("Existing position updated: {0}", position.ToString());
            }
            else
            {
                position = new Position
                {
                    PositionId = TradingDataStore.Positions.Count + 1,
                    AccountId = request.AccountId,
                    EquityId = request.EquityId,
                    Quantity = request.Quantity,
                    AverageCostPerShare = request.LimitPrice,
                };
                TradingDataStore.Positions.Add(position);
                logger.LogInformation("New position created: {0}", position.ToString());
            }
        }
        else if (request.OrderType == OrderType.Sell)
        {
            if (position == null || position.Quantity < request.Quantity)
            {
                logger.LogWarning("Attempt to sell more shares than owned: AccountId={0}, EquityId={1}, AttemptedQuantity={2}, OwnedQuantity={3}",
                    request.AccountId, request.EquityId, request.Quantity, position?.Quantity ?? 0);
                return "Insufficient shares to sell";
            }

            position.Quantity -= request.Quantity;
            account.CashBalance += cost;

            if (position.Quantity == 0)
            {
                TradingDataStore.Positions.Remove(position);
            }
        }
        else
        {
            logger.LogError("Invalid order type: {OrderType}", request.OrderType);
            return "Invalid order side";
        }

        var positionUpdate = new PositionDto()
        {
            PositionId = position.PositionId,
            Account = account,
            Equity = equity,
            AverageCostPerShare = position.AverageCostPerShare,
            Quantity = position.Quantity,
            CurrentValue = position.Quantity * equity.CurrentPrice,
        };

        logger.LogInformation("Position updated: {@PositionUpdate}", positionUpdate.ToString());
        await tradingSignalRService.SendPositionUpdate(positionUpdate);
        return null;
    }
}