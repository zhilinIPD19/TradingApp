using System;
using System.Collections.Generic;
using System.Text;
using TradingLibrary.Models;

namespace TradingUI.Services
{
    public interface ITradingSignalRService
    {
        event Action<PositionDto>? PositionUpdated;
        event Action<string>? ConnectionStateChanged;
    }
}
