
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System.Windows;
using TradingUI.Infrastructure;
using TradingUI.Services;
using TradingUI.ViewModels;

namespace TradingUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? host;
        protected override async void OnStartup(StartupEventArgs e)
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json",optional:false).Build();
                    services.Configure<ApiSettings>(config.GetSection("ApiSettings"));
                    services.AddHttpClient<ITradingApiService, TradingApiService>((sp,client) =>
                    {
                        client.BaseAddress = new Uri(uriString: sp.GetRequiredService<IOptions<ApiSettings>>().Value.ApiBaseUrl);
                        client.Timeout = TimeSpan.FromSeconds(30);
                    }).AddTransientHttpErrorPolicy(policy =>
                        policy.WaitAndRetryAsync(
                            3,
                            retryAttempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt)),
                            (result, timeSpan, retryCount, context) =>
                            {
                                var logger = context["Logger"] as ILogger;
                                logger?.LogWarning("Retry {Retry}", retryCount);
                            }));
                    services.AddSingleton<TradingSignalRService>();
                    services.AddSingleton<IUiDispatcher, UiDispatcher>();
                    services.AddSingleton<ITradingSignalRService>(sp =>sp.GetRequiredService<TradingSignalRService>());
                    services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<TradingSignalRService>());
                    services.AddSingleton<TradingViewModel>();
                    services.AddSingleton<TradingWindow>();
                })
                .Build();

            await host.StartAsync();
             var mainWindow = host.Services.GetRequiredService<TradingWindow>();
             mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
