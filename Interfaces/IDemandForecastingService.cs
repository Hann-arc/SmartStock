using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.Forecast;

namespace SmartStockAI.Interfaces
{
    public interface IDemandForecastingService
    {
        Task<ResForecastDto> GetDemandForecastAsync(Guid itemId, int daysAhead = 7);
    }
}