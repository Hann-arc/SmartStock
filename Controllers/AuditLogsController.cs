using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.AuditLog;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        /// <summary>
        /// Get audit logs with filtering and pagination
        /// </summary>
        /// <param name="query">Filter parameters</param>
        /// <returns>Paginated audit logs</returns>
        /// <response code="200">Returns paginated audit logs</response>
        /// <response code="403">Forbidden - only Admin can access</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ResAuditLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetLogs([FromQuery] AuditLogQueryDto query)
        {
            if (query.Page < 1) query.Page = 1;
            if (query.PageSize < 1) query.PageSize = 10;
            if (query.PageSize > 50) query.PageSize = 50; // limit

            var logs = await _auditLogService.GetLogsAsync(
                query.StartDate,
                query.EndDate,
                query.UserId,
                query.EntityType,
                query.Action,
                query.Page,
                query.PageSize
            );

            return Ok(logs);
        }

        /// <summary>
        /// Get single audit log by ID
        /// </summary>
        /// <param name="id">Audit log ID</param>
        /// <returns>Audit log details</returns>
        /// <response code="200">Returns the audit log</response>
        /// <response code="404">Audit log not found</response>
        /// <response code="403">Forbidden - only Admin can access</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ResAuditLogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _auditLogService.GetLogsAsync(null, null, null, null, null, 1, 1);
            var log = result.Items.FirstOrDefault(x => x.Id == id);

            if (log == null)
                return NotFound(new { message = "Audit log not found" });

            return Ok(log);
        }
    }
}