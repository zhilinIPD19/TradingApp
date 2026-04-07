using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TradingLibrary.Models;

namespace TradingUI.Services
{
    internal class TradingSignalRService : ITradingSignalRService, IHostedService
    {
        private readonly HubConnection _connection;
        public event Action<string>? ConnectionStateChanged;
        public event Action<PositionDto>? PositionUpdated;
        public TradingSignalRService(IOptions<ApiSettings> options)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(options.Value.HubUrl)
                .WithAutomaticReconnect(new[] {
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(30)})
                .Build();

            RegisterHandlers();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connection.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.DisposeAsync();
        }

        private void RegisterHandlers()
        {
            _connection.Reconnecting += error =>
            {
                ConnectionStateChanged?.Invoke("Trading Hub reconnecting...");
                return Task.CompletedTask;
            };

            _connection.Closed += error =>
            {
                ConnectionStateChanged?.Invoke("Trading Hub disconnected.");
                return Task.CompletedTask;
            };

            _connection.Reconnected += error =>
            {
                ConnectionStateChanged?.Invoke("Trading Hub reconnected.");
                return Task.CompletedTask;
            };

            _connection.On<PositionDto>("ReceivePositionUpdate", update =>
            {
                PositionUpdated?.Invoke(update);
            });
        }
    }
}
