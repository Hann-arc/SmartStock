using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Forecast
{
    public class ResForecastDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int PredictedUnits { get; set; }
        public double ConfidenceLevel { get; set; }
        public string ConfidenceLevelFormatted => $"{ConfidenceLevel:P1}";
        public string Recommendation { get; set; } = string.Empty;
        public int DaysAhead { get; set; }
        public int HistoricalDataPoints { get; set; }
        public DateTime ForecastDate { get; set; }
        public DateTime BasedOnDataUntil { get; set; }
    }
}