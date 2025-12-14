using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Services
{
    public class StockCheckBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); 

        public StockCheckBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var alertService = scope.ServiceProvider.GetRequiredService<IAlertService>();

                    await alertService.CheckLowStockItemsAsync();
                    Console.WriteLine($"[AUTO-ALERT] Stock check completed at {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AUTO-ALERT] Error: {ex.Message}");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

    }
}