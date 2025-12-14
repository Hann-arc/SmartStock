using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.Stock;
using SmartStockAI.Extensions;

namespace SmartStockAI.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [Authorize]
    public class StockTransactionsController : ControllerBase
    {

        private readonly IStockTransactionService _stockService;
        public StockTransactionsController(IStockTransactionService stockService)
        {
            _stockService = stockService;
        }

        /// <summary>
        /// Add stock to an item (Stock In)
        /// </summary>
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("in")]
        [ProducesResponseType(typeof(ResStockTransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StockIn([FromBody] ReqStockInDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.TryGetUserId(out var userId))
                return Unauthorized("Invalid user token");

            var result = await _stockService.StockInAsync(userId, dto);
            return Created("", result);

        }

        /// <summary>
        /// Reduce stock from an item (Stock Out)
        /// </summary>
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("out")]
        [ProducesResponseType(typeof(ResStockTransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StockOut([FromBody] ReqStockOutDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.TryGetUserId(out var userId))
                return Unauthorized("Invalid user token");
            var result = await _stockService.StockOutAsync(userId, dto);

            return Created("", result);
        }

        /// <summary>
        /// Get stock transaction history for an item
        /// </summary>
        [HttpGet("{itemId:guid}/history")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetHistory(Guid itemId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;


            var transactions = await _stockService.GetItemHistoryAsync(itemId, page, pageSize);

            var totalCount = await _stockService.CountHistoryAsync(itemId);

            return Ok(new
            {
                items = transactions,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

    }
}