using Microsoft.AspNetCore.SignalR;
using TradingApi.Hubs;
using TradingLibrary.Models;

namespace TradingApi.Services
{
    public class TradingSignalRService(IHubContext<TradingHub> hubContext, ILogger<TradingSignalRService> logger) : ITradingSignalRService
    {
        public async Task SendPositionUpdate(PositionDto position)
        {
            try
            {
                logger.LogInformation("Sending position update: {0}", position.ToString());
                await hubContext.Clients.All.SendAsync("ReceivePositionUpdate", position);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending position update: {0}", position.ToString());
                throw;
            }
        }
    }
}
