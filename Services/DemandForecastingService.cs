using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Forecast;
using SmartStockAI.Exceptions;
using SmartStockAI.Helpers;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.Services
{
    public class DemandForecastingService : IDemandForecastingService
    {
        private readonly IStockTransactionRepository _stockTransactionRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;

        public DemandForecastingService(
            IStockTransactionRepository stockTransactionRepo,
            IItemRepository itemRepo,
            IMapper mapper)
        {
            _stockTransactionRepo = stockTransactionRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public async Task<ResForecastDto> GetDemandForecastAsync(Guid itemId, int daysAhead = 7)
        {
            // Validasi input
            if (daysAhead < 1 || daysAhead > 30)
                throw new ValidationException("Days ahead must be between 1 and 30");

            // Ambil item untuk validasi dan data stok
            var item = await _itemRepo.GetByIdAsync(itemId);
            if (item == null)
                throw new NotFoundException("Item", itemId);

            // Ambil data penjualan 30 hari terakhir
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var salesData = await _stockTransactionRepo.GetDailySalesDataAsync(
                itemId, 
                thirtyDaysAgo, 
                DateTime.UtcNow
            );

            // Hitung prediksi
            var forecastResult = CalculateDemandForecast(salesData, daysAhead, item.Stock, item.MinimumThreshold);

            // Create response DTO
            return new ResForecastDto
            {
                ItemId = itemId,
                ItemName = item.Name,
                PredictedUnits = forecastResult.PredictedUnits,
                ConfidenceLevel = forecastResult.ConfidenceLevel,
                Recommendation = forecastResult.Recommendation,
                DaysAhead = daysAhead,
                HistoricalDataPoints = salesData.Count,
                ForecastDate = DateTime.UtcNow.AddDays(daysAhead),
                BasedOnDataUntil = salesData.Any() ? salesData.Max(x => x.Date) : DateTime.UtcNow
            };
        }

        private (int PredictedUnits, double ConfidenceLevel, string Recommendation) 
            CalculateDemandForecast(List<DailySalesData> salesData, int daysAhead, int currentStock, int minimumThreshold)
        {
            // Graceful degradation jika data tidak cukup
            if (salesData.Count < 7)
            {
                return (
                    0,
                    0.0,
                    $"Butuh minimal {7 - salesData.Count} hari lagi data transaksi untuk prediksi akurat"
                );
            }

            // Count linear regression
            var (slope, intercept, rSquared) = CalculateLinearRegression(salesData);
            
            // Prediksi untuk hari ke-n
            double predictedValue = slope * (salesData.Count + daysAhead) + intercept;
            int predictedUnits = Math.Max(0, (int)Math.Round(predictedValue));
            
            // Hitung confidence level (R-squared + data points weight)
            double confidenceLevel = CalculateConfidenceLevel(rSquared, salesData.Count);
            
            // Business recommendation
            string recommendation = GenerateBusinessRecommendation(
                predictedUnits, 
                currentStock, 
                minimumThreshold,
                daysAhead
            );
            
            return (predictedUnits, confidenceLevel, recommendation);
        }

        private (double slope, double intercept, double rSquared) CalculateLinearRegression(List<DailySalesData> salesData)
        {
            int n = salesData.Count;
            double[] x = new double[n]; // Days (0, 1, 2, ...)
            double[] y = new double[n]; // Sales quantities
            
            // Prepare data
            for (int i = 0; i < n; i++)
            {
                x[i] = i;
                y[i] = salesData[i].TotalQuantity;
            }
            
            // Hitung means
            double xMean = x.Average();
            double yMean = y.Average();
            
            // Hitung slope (b) dan intercept (a)
            double numerator = 0;
            double denominator = 0;
            
            for (int i = 0; i < n; i++)
            {
                numerator += (x[i] - xMean) * (y[i] - yMean);
                denominator += Math.Pow(x[i] - xMean, 2);
            }
            
            double slope = numerator / denominator;
            double intercept = yMean - (slope * xMean);
            
            // Hitung R-squared untuk confidence level
            double ssTotal = 0;
            double ssResidual = 0;
            
            for (int i = 0; i < n; i++)
            {
                double yPred = slope * x[i] + intercept;
                ssTotal += Math.Pow(y[i] - yMean, 2);
                ssResidual += Math.Pow(y[i] - yPred, 2);
            }
            
            double rSquared = 1 - (ssResidual / ssTotal);
            return (slope, intercept, rSquared);
        }

        private double CalculateConfidenceLevel(double rSquared, int dataPoints)
        {
            // Weight R-squared dengan jumlah data points
            double dataWeight = Math.Min(1.0, dataPoints / 30.0); // Max 30 data points
            double confidenceLevel = rSquared * 0.7 + dataWeight * 0.3;
            return Math.Min(1.0, Math.Max(0.0, confidenceLevel));
        }

        private string GenerateBusinessRecommendation(int predictedUnits, int currentStock, int minimumThreshold, int daysAhead)
        {
            if (predictedUnits > currentStock)
            {
                int unitsNeeded = predictedUnits - currentStock;
                return $"PERLU TAMBAH STOK SEGERA! Prediksi kebutuhan {daysAhead} hari: {predictedUnits} unit. Stok saat ini hanya {currentStock} unit.";
            }
            else if (currentStock - predictedUnits < minimumThreshold)
            {
                int remainingStock = currentStock - predictedUnits;
                return $" STOK AKAN MENIPIS! Setelah {daysAhead} hari, stok tersisa: {remainingStock} unit (di bawah threshold: {minimumThreshold})";
            }
            else if (currentStock - predictedUnits < minimumThreshold * 2)
            {
                int remainingStock = currentStock - predictedUnits;
                return $"PERLU PERSIAPAN! Setelah {daysAhead} hari, stok tersisa: {remainingStock} unit. Pertimbangkan restock sebelum stok kritis.";
            }
            else
            {
                int remainingStock = currentStock - predictedUnits;
                return $" STOK AMAN! Prediksi kebutuhan {daysAhead} hari: {predictedUnits} unit. Stok setelah prediksi: {remainingStock} unit.";
            }
        }
    }

}