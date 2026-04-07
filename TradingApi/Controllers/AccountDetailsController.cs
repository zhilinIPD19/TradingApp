using Microsoft.AspNetCore.Mvc;
using TradingApi.Data;
using TradingLibrary.Models;

namespace TradingApi.Controllers
{
    [Route("accountDetails/{accountId}")]
    [ApiController]
    public class AccountDetailsController : ControllerBase
    {
        // GET: 
        [HttpGet]
        public IActionResult GetAccountDetails(int accountId)
        {
            return Ok(GetAccountDto(accountId));
        }

        private static AccountDto? GetAccountDto(int accountId)
        {
            var account = TradingDataStore.Accounts.FirstOrDefault(o => o.AccountId == accountId);
            if(account == null)
            {
                return null;
            }

            return new AccountDto
            {
                AccountId = account.AccountId,
                ClientName = account.ClientName,
                AccountNumber = account.AccountNumber,
                CashBalance = account.CashBalance,
                Positions = GetPositionDetails(accountId),
            };
        }

        private static List<PositionDto> GetPositionDetails(int accountId)
        {
            var positionDetails = TradingDataStore.Positions.Where(o => o.AccountId == accountId).Join(TradingDataStore.Equities, o => o.EquityId, e => e.EquityId, (o,e) => new PositionDto
            {
                PositionId = o.PositionId,
                Account = TradingDataStore.Accounts.FirstOrDefault(o => o.AccountId == accountId),
                Equity = TradingDataStore.Equities.FirstOrDefault(o => o.EquityId == e.EquityId),
                Quantity = o.Quantity,
                AverageCostPerShare = o.AverageCostPerShare,
                CurrentValue = e.CurrentPrice * o.Quantity
            }).ToList();

            return positionDetails ?? [];
        }
    }
}
