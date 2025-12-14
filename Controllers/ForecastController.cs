using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.Forecast;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ForecastController : ControllerBase
    {
        private readonly IDemandForecastingService _forecastingService;

        public ForecastController(IDemandForecastingService forecastingService)
        {
            _forecastingService = forecastingService;
        }

        /// <summary>
        /// Get demand forecast for an item
        /// </summary>
        /// <param name="itemId">Item ID to forecast</param>
        /// <param name="daysAhead">Number of days to forecast (1-30)</param>
        /// <returns>Demand forecast with business recommendation</returns>
        /// <response code="200">Returns demand forecast</response>
        /// <response code="404">Item not found</response>
        /// <response code="400">Invalid input parameters</response>
        [HttpGet("{itemId:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        [ProducesResponseType(typeof(ResForecastDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetForecast(Guid itemId, [FromQuery] int daysAhead = 7)
        {
            var result = await _forecastingService.GetDemandForecastAsync(itemId, daysAhead);
            return Ok(result);
        }

    }
}