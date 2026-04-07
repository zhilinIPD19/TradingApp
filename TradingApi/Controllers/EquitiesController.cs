using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingApi.Data;

namespace TradingApi.Controllers
{
    [Route("equities")]
    [ApiController]
    public class EquitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEquities()
        {
            // Placeholder for getting equities logic
            return Ok(TradingDataStore.Equities);
        }
    }
}
