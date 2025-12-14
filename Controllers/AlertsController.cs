using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.Alert;
using SmartStockAI.Extensions;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Controllers
{
    [Route("api/alerts")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;


        public AlertsController(IAlertService alertService)
        {
            _alertService = alertService;

        }

        /// <summary>
        /// Get all active alerts
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResAlertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveAlerts()
        {
            var alerts = await _alertService.GetActiveAlertsAsync();
            return Ok(alerts);
        }

        /// <summary>
        /// Manual trigger stock check
        /// </summary>
        [HttpPost("check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckStockLevels()
        {
            await _alertService.CheckLowStockItemsAsync();
            return Ok(new
            {
                message = "Stock check completed successfully"
            });
        }

        /// <summary>
        /// Resolve an alert
        /// </summary>
        [HttpPost("resolve")]
        [ProducesResponseType(typeof(ResAlertDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResolveAlert([FromBody] ReqResolveAlertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!User.TryGetUserId(out var userId))
                return Unauthorized("Invalid user token");

            dto.ResolvedByUserId = userId;
            var result = await _alertService.ResolveAlertAsync(dto);
            return Ok(result);
        }
    }
}