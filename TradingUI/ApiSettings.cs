using System;
using System.Collections.Generic;
using System.Text;

namespace TradingUI
{
    public class ApiSettings
    {
        public string ApiBaseUrl { get; set; } = "https://localhost:7007/";
        public string HubUrl { get; set; } = "https://localhost:7007/tradingHub";
    }
}
