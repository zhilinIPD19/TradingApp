using TradingLibrary.Models;

namespace TradingApi.Services;

public interface ITradingSignalRService
{
    Task SendPositionUpdate(PositionDto position);
}