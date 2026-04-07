using Microsoft.AspNetCore.Mvc;
using TradingApi.Data;

namespace TradingApi.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // GET: accounts
        [HttpGet]
        public IActionResult GetAccounts()
        {
            // Placeholder for getting accounts logic
            return Ok(TradingDataStore.Accounts);
        }
    }
}
